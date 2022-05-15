using API_test.ApiResponseModels;
using API_test.Helpers;
using API_test.RequestSpecs;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace API_test.Tests
{
    public class CountryCodeParamNUnitTests
    {
        private RestHelper restHelper;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [TestCase("ru")]
        [TestCase("kg")]
        [TestCase("kz")]
        [TestCase("cz")]
        public void FilteringShouldWorkAsExpected(string countryCode)
        {
            ResponseDataRetriever dataRetriever = new ResponseDataRetriever();
            List<ItemDto> filteredItems = dataRetriever.GetFilteredItemsByCountryCode(restHelper, countryCode);
            foreach (ItemDto item in filteredItems)
            {
                Assert.AreEqual(countryCode, item.Country.Code, "Got unexpected country code in filtered list");
            }
        }

        [Test]
        public void UnexpectedCountryCodeShouldBeHandledCorrectly()
        {
            const string wrongCountryCode = "www";
            const string expectedErrorMessage = "Параметр 'country_code' может быть одним из следующих значений: ru, kg, kz, cz";

            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.CountryCodeParameter, wrongCountryCode);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }
    }
}
