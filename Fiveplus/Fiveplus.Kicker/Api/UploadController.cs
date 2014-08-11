using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;


namespace Fiveplus.Kicker.Api
{

    public class FileDesc
    {

        public string name { get; set; }
        
        public string path { get; set; }
        
        public long size { get; set; }
        public FileDesc(string n, string p, long s)
        {
            name = n;
            path = p;
            size = s;
        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public readonly string _filename;
        public CustomMultipartFormDataStreamProvider(string path, string filename)
            : base(path)
        {
            _filename = filename;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            return _filename;
        }
    }

    [RoutePrefix("api")]
    //[EnableCors("*", "*", "GET, PUT, POST")]
    public class UploadController : ApiController
    {
        private readonly string BASEFOLDERNAME = "uploads";
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
            var meta = new FlowMeta(Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value));
            var PATH = HttpContext.Current.Server.MapPath(string.Format(@"~/{0}/{1}", BASEFOLDERNAME, meta.flowIdentifier));
            var filename = string.Format(@"{0}_{1}", meta.flowFilename, meta.flowChunkNumber.PadLeft(4, '0'));
            if ("1".Equals(meta.flowTotalChunks))
            {
                filename = string.Format(@"{0}", meta.flowFilename);
            }
            if (File.Exists(Path.Combine(PATH, filename)))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        public Task<IEnumerable<FileDesc>> Post()
        {
            var meta = new FlowMeta(HttpContext.Current.Request.Form);
            var PATH = HttpContext.Current.Server.MapPath(string.Format(@"~/{0}/{1}", BASEFOLDERNAME, meta.flowIdentifier));
            var variables = System.Web.HttpContext.Current.Request.ServerVariables;
            var physPath = variables["APPL_PHYSICAL_PATH"];

            Directory.CreateDirectory(PATH);
            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            if (Request.Content.IsMimeMultipartContent())
            {
                var filename = string.Format(@"{0}_{1}", meta.flowFilename, meta.flowChunkNumber.PadLeft(4, '0'));
                if ("1".Equals(meta.flowTotalChunks))
                {
                    filename = string.Format(@"{0}", meta.flowFilename);
                }

                var streamProvider = new CustomMultipartFormDataStreamProvider(PATH, filename);
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IEnumerable<FileDesc>>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    if (!"1".Equals(meta.flowTotalChunks) && meta.flowTotalChunks.Equals(meta.flowChunkNumber))
                    {
                        var dest = Path.Combine(PATH, meta.flowFilename);
                        using (var fileS = new FileStream(dest, FileMode.Create))
                        {
                            var files = Directory.EnumerateFiles(PATH)
                                .Where(s => !s.Equals(dest))
                                                 .OrderBy(s => s);
                            foreach (var file in files)
                            {
                                using (var sourceStream = File.OpenRead(file))
                                    sourceStream.CopyTo(fileS);
                            }
                            fileS.Flush();
                            int expectedBytes;
                            if (int.TryParse(meta.flowTotalSize, out expectedBytes))
                            {
                                var info = new FileInfo(dest);
                                if (info.Length == expectedBytes)
                                {
                                    foreach (var file in files)
                                    {
                                        File.Delete(file);
                                    }
                                }
                            }
                        }
                        var fileInfo = streamProvider.FileData.Select(i =>
                        {
                            var uri = dest.Replace(physPath, "~/").Replace(@"\", "/");
                            var info = new FileInfo(dest);
                            return new FileDesc(info.Name, uri, info.Length / 1024);
                        });
                        return fileInfo;
                    }
                    else
                    {
                        var fileInfo = streamProvider.FileData.Select(i =>
                        {
                            var uri = i.LocalFileName.Replace(physPath, "~/").Replace(@"\", "/");
                            var info = new FileInfo(i.LocalFileName);
                            return new FileDesc(info.Name, uri, info.Length / 1024);
                        });
                        return fileInfo;
                    }

                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not formatted correctly or understood"));
            }
        }
    }
}
