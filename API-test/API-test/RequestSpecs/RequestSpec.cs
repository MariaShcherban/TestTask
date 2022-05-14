using System.Collections.Generic;

namespace API_test.RequestSpecs
{
    public static class RequestSpec
    {
        private static string url = "https://regions-test.2gis.com/";
        private static string endpoint = "1.0/regions";
        private const string searchParameter = "q";
        private const string countryCodeParameter = "country_code";
        private const string pageNumberParameter = "page";
        private const string pageSizeParameter = "page_size";

        public static string Query => endpoint;
        public static string BackendUrl => url;
        public static string SearchParameter => searchParameter;
        public static string CountryCodeParameter => countryCodeParameter;
        public static string PageNumberParameter => pageNumberParameter;
        public static string PageSizeParameter => pageSizeParameter;

        public static Dictionary<string, string> Params(string region, string countryCode, string pageNumber, string pageSize)
        {
            return new Dictionary<string, string>()
            {
                { searchParameter, region},
                { countryCodeParameter, countryCode },
                { pageNumberParameter, pageNumber },
                { pageSizeParameter, pageSize }
            };
        }

        public static Dictionary<string, string> PageSizeNumberParam(string pageNumber, string pageSize)
        {
            return new Dictionary<string, string>()
            {
                { pageNumberParameter, pageNumber },
                { pageSizeParameter, pageSize }
            };
        }
    }
}
