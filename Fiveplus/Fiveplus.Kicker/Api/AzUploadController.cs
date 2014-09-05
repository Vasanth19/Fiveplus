using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Fiveplus.Data.Models;
using Fiveplus.Data.Repo;
using Fiveplus.Data.Uow;
using Fiveplus.Storage;
using Microsoft.AspNet.Identity;


namespace Fiveplus.Kicker.Api
{

    public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
    {
        private NameValueCollection _formData = new NameValueCollection();
        private List<HttpContent> _fileContents = new List<HttpContent>();

        // Set of indexes of which HttpContents we designate as form data
        private Collection<bool> _isFormData = new Collection<bool>();

        /// <summary>
        /// Gets a <see cref="NameValueCollection"/> of form data passed as part of the multipart form data.
        /// </summary>
        public NameValueCollection FormData
        {
            get { return _formData; }
        }

        /// <summary>
        /// Gets list of <see cref="HttpContent"/>s which contain uploaded files as in-memory representation.
        /// </summary>
        public List<HttpContent> Files
        {
            get { return _fileContents; }
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            // For form data, Content-Disposition header is a requirement
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                // We will post process this as form data
                _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

                return new MemoryStream();
            }

            // If no Content-Disposition header was present.
            throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
        }

        /// <summary>
        /// Read the non-file contents as form data.
        /// </summary>
        /// <returns></returns>
        public override async Task ExecutePostProcessingAsync()
        {
            // Find instances of non-file HttpContents and read them asynchronously
            // to get the string content and then add that as form data
            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];
                    // Extract name from Content-Disposition header. We know from earlier that the header is present.
                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;

                    // Read the contents as string data and add to form data
                    string formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
                else
                {
                    _fileContents.Add(Contents[index]);
                }
            }
        }

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        /// <param name="token">Token to unquote.</param>
        /// <returns>Unquoted token.</returns>
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }

    [RoutePrefix("api/upload")]
    
    [Route("user/{userId}")]
    [Route("gig/{gigId}")]
    [Route("user/{userId}/gig/{gigId}")]
    //[EnableCors("*", "*", "GET, PUT, POST")]
    public class UploadController : ApiController
    {

        private IMediaRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public UploadController(ExplorerUow explorerUow, IMediaRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }

        private class FlowMeta
        {
            public string flowChunkNumber { get; set; }
            public string flowChunkSize { get; set; }
            public string flowCurrentChunkSize { get; set; }
            public string flowTotalSize { get; set; }
            public string flowIdentifier { get; set; }
            public string flowFilename { get; set; }
            public string flowRelativePath { get; set; }
            public string flowTotalChunks { get; set; }

            public FlowMeta(Dictionary<string, string> values)
            {
                flowChunkNumber = values["flowChunkNumber"];
                flowChunkSize = values["flowChunkSize"];
                flowCurrentChunkSize = values["flowCurrentChunkSize"];
                flowTotalSize = values["flowTotalSize"];
                flowIdentifier = values["flowIdentifier"];
                flowFilename = values["flowFilename"];
                flowRelativePath = values["flowRelativePath"];
                flowTotalChunks = values["flowTotalChunks"];
            }

            public FlowMeta(NameValueCollection values)
            {
                flowChunkNumber = values["flowChunkNumber"];
                flowChunkSize = values["flowChunkSize"];
                flowCurrentChunkSize = values["flowCurrentChunkSize"];
                flowTotalSize = values["flowTotalSize"];
                flowIdentifier = values["flowIdentifier"];
                flowFilename = values["flowFilename"];
                flowRelativePath = values["flowRelativePath"];
                flowTotalChunks = values["flowTotalChunks"];
            }
        }


        public HttpResponseMessage Get()
        {

            return Request.CreateResponse(HttpStatusCode.NotFound);
           
        }

        //http://stackoverflow.com/questions/15842496/is-it-possible-to-override-multipartformdatastreamprovider-so-that-is-doesnt-sa/15843410#15843410

        public async Task<IEnumerable<FileDesc>> Post( string gigId,string userId = "")
        {
            var meta = new FlowMeta(HttpContext.Current.Request.Form);
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }


            if (Request.Content.IsMimeMultipartContent())
            {
                var filename = string.Format(@"{0}", meta.flowFilename);

                var listFiles = await StoreFileToAzure(gigId, filename);

                var insertCount = SaveMedia(listFiles[0], gigId);

                return listFiles;
            }
            else
            {   
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not formatted correctly or understood"));
            }
        }

        private async Task<List<FileDesc>> StoreFileToAzure(string gigId, string filename)
        {
            var provider =
                await
                    Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(
                        new InMemoryMultipartFormDataStreamProvider());

            //access form data
            NameValueCollection formData = provider.FormData;

            //access files
            IList<HttpContent> files = provider.Files;

            List<FileDesc> listFiles = new List<FileDesc>();

            FileDesc fileDesc = new FileDesc()
            {
                GigId = gigId,
                Name = filename,
                UserName = User.Identity.Name,
                ContentType = "image/" + Path.GetExtension(filename).ToLower().Substring(1)
            };
            //Example: reading a file's stream like below
            foreach (var file in files)
            {
                Stream fileStream = await file.ReadAsStreamAsync();

                fileDesc.Path = StorageManager.UploadImage(fileStream, fileDesc);
                fileDesc.Size = fileStream.Length/1024;
                listFiles.Add(fileDesc);
            }
            return listFiles;
        }


        private int SaveMedia(FileDesc fileDesc, string gigId)
        {
            Media media = new Media()
            {
                GigId = Convert.ToInt32(gigId),
                State = State.Added,
                Type = MediaType.Image,
                Url = fileDesc.Path
            };

            _repo.InsertOrUpdateGraph(media);

            try
            {
                int i = _explorerUow.Save();
                return i;
            }
            catch (DbUpdateConcurrencyException e)
            {
                return 0;
            }

        }

        private async Task<int> SaveMediaAsync(FileDesc fileDesc,string gigId)
        {
            Media media = new Media()
            {
                GigId = Convert.ToInt32(gigId),
                State = State.Added,
                Type = MediaType.Image,
                Url = fileDesc.Path
            };

            _repo.InsertOrUpdateGraph(media);
          
            try
            {
               int i = await _explorerUow.SaveAsync();
                return i;
            }
            catch (DbUpdateConcurrencyException e)
            {
               return 0;
            }

        }
    }
}
