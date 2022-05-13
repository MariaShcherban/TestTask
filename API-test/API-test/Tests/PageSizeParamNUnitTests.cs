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
        private const int defaultPageSize = 15;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [Test]
        public void DefaultPageSizeShouldBeCorrect()
        {
            var response = restHelper.GetQuery(RequestSpec.Query);
            var itemsList = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            Assert.AreEqual(defaultPageSize, itemsList.Count, "Default page size is not as expected");
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        public void AllPageSizesShouldWorkAsExpected(int pageSize)
        {
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, pageSize.ToString());
            var itemsList = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            Assert.AreEqual(pageSize, itemsList.Count, "Returned page size is not as expected");
        }

        [Test]
        public void WrongTypeOfPageSizeParameterShouldBeHandled()
        {
            string expectedErrorMessage = "Параметр 'page_size' должен быть целым числом";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, "r");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
                Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
            });
        }

        [Test]
        public void IncorrectPageSizeParameterShouldBeHandled()
        {
            string expectedErrorMessage = "Параметр 'page_size' может быть одним из следующих значений: 5, 10, 15";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, "4");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
                Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
            });
        }
    }
}
