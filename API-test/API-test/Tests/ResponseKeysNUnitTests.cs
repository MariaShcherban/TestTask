using API_test.ApiResponseModels;
using API_test.Helpers;
using API_test.RequestSpecs;
using NUnit.Framework;
using System.Collections.Generic;

namespace API_test.Tests
{
    public class ResponseKeysNUnitTests
    {
        private RestHelper restHelper;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [Test]
        public void KeysInJsonShouldBeCorrect()
        {
            var response = restHelper.GetQuery(RequestSpec.Query);

            var total = response.Content.ToObject<TotalDto>();
            Assert.IsNotNull(total.Total);

            var items = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            foreach (ItemDto item in items)
            {
                Assert.IsNotNull(item.Code);
                Assert.IsNotNull(item.Id);
                Assert.IsNotNull(item.Name);
                Assert.IsNotNull(item.Country.Code);
                Assert.IsNotNull(item.Country.Name);
            }    
        }
    }
}
