using Newtonsoft.Json.Linq;
using System.Net;

namespace API_test.Helpers
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public JToken Content { get; set; }
        public string ContentType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
