using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiveplus.Kicker.Helpers
{
    public class JsonResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public IEnumerable<JsonValidationError> Errors { get; set; }

        public JsonResponse()
        {
            Errors = new List<JsonValidationError>();
        }
    }

    public class JsonValidationError
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }
}