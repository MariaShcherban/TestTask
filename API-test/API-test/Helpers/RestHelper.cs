using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace API_test.Helpers
{
    public class RestHelper
    {
        private static RestClient endpoint;

        public RestHelper(string url)
        {
            endpoint = new RestClient(url);
        }

        public Response GetQuery(string query)
        {
            var request = new RestRequest(query, Method.GET);
            return ParseResponse(endpoint.Execute(request));
        }

        public Response GetQuery(string query, Dictionary<string, string> queryParams)
        {
            var request = new RestRequest(query, Method.GET);
            foreach (var param in queryParams)
            {
                request.AddQueryParameter(param.Key, param.Value);
            }
            return ParseResponse(endpoint.Execute(request));
        }

        public Response PostQuery(string query)
        {
            var request = new RestRequest(query, Method.POST);
            return ParseResponse(endpoint.Execute(request));
        }

        public Response PutQuery(string query)
        {
            var request = new RestRequest(query, Method.PUT);
            return ParseResponse(endpoint.Execute(request));
        }

        public Response ParseResponse (IRestResponse response)
        {
            JToken content = null;
            try
            {
                content = JToken.Parse(response.Content);
                return new Response()
                {
                    StatusCode = response.StatusCode,
                    Content = response.ContentLength != 0 ? JToken.Parse(response.Content) : JToken.Parse("{}"),
                    ContentType = response.ContentType,
                    ErrorMessage = response.ErrorMessage
                };
            }
            catch (JsonReaderException ex)
            {
                return new Response()
                {
                    StatusCode = response.StatusCode,
                    Content = JToken.Parse("{}"),
                    ContentType = response.ContentType,
                    ErrorMessage = response.ErrorMessage
                };
            }
        }
    }
}
