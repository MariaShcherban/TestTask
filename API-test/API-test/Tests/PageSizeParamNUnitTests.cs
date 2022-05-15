using API_test.RequestSpecs;
using API_test.Helpers;
using NUnit.Framework;
using System.Net;
using API_test.ApiResponseModels;
using System.Collections.Generic;

namespace API_test.Tests
{
    public class PageSizeParamNUnitTests
    {
        private RestHelper restHelper;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [Test]
        public void DefaultPageSizeShouldBeCorrect()
        {
            const int defaultPageSize = 15;

            var response = restHelper.GetQuery(RequestSpec.Query);
            var itemsList = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            Assert.AreEqual(defaultPageSize, itemsList.Count, "Got unexpected default page size");
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        public void AllPageSizesShouldWorkAsExpected(int pageSize)
        {
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, pageSize.ToString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var itemsList = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            Assert.AreEqual(pageSize, itemsList.Count, "Got unexpected page size");
        }

        [Test]
        public void WrongTypeOfPageSizeParameterShouldBeHandled()
        {
            const string expectedErrorMessage = "Параметр 'page_size' должен быть целым числом";
            const string charPageSize = "r";

            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, charPageSize);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }

        [Test]
        public void IncorrectPageSizeParameterShouldBeHandled()
        {
            const string expectedErrorMessage = "Параметр 'page_size' может быть одним из следующих значений: 5, 10, 15";
            const string invalidPageSize = "4";

            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, invalidPageSize);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }
    }
}
