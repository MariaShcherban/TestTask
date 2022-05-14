using API_test.ApiResponseModels;
using API_test.Helpers;
using API_test.RequestSpecs;
using NUnit.Framework;
using System.Collections.Generic;

namespace API_test.Tests
{
    public class SearchParamNUnitTest
    {
        private RestHelper restHelper;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [Test]
        public void RegionSearchShouldWork()
        {
            const string searchParam = "рск";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.SearchParameter, searchParam);
            List<ItemDto> items = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            foreach (ItemDto item in items)
            {
                Assert.That(item.Name, Contains.Substring(searchParam), "Got incorrectly filtered item in search results");
            }
        }
    }
}
