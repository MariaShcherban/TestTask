using API_test.ApiResponseModels;
using API_test.Helpers;
using API_test.RequestSpecs;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

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
                Assert.That(item.Name.Normalize(), Contains.Substring(searchParam.Normalize()), "Got incorrectly filtered item in search results");
            }
        }

        [Test]
        public void RegionSearchShouldBeCaseInsensitive()
        {
            const string lowerCaseParam = "рск";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.SearchParameter, lowerCaseParam);
            List<ItemDto> lowerCaseItems = response.Content.SelectToken("items").ToObject<List<ItemDto>>();

            const string mixedCaseParam = "рСК";
            response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.SearchParameter, mixedCaseParam);
            List<ItemDto> mixedCaseItems = response.Content.SelectToken("items").ToObject<List<ItemDto>>();

            lowerCaseItems.Should().BeEquivalentTo(mixedCaseItems);
        }

        [Test]
        public void SearchWithLessThanThreeSymbolsShouldBeHandled()
        {
            const string searchParam = "уу";
            const string expectedErrorMessage = "Параметр 'q' должен быть не менее 3 символов";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.SearchParameter, searchParam);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }

        [Test]
        public void SearchWithMoreThanThirtySymbolsShouldBeHandled()
        {
            const string searchParam = "EgrnIxHeaHAkOWPavYbgmhQRxYsLrra";
            const string expectedErrorMessage = "Параметр 'q' должен быть не более 30 символов";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.SearchParameter, searchParam);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }
    }
}
