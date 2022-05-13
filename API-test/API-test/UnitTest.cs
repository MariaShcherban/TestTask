using API_test.RequestSpecs;
using API_test.Helpers;
using NUnit.Framework;
using System.Net;
using API_test.ApiResponseModels;
using System.Collections.Generic;

namespace API_test
{
    public class Tests
    {
        private RestHelper restHelper;
        private const int defaultPageSize = 15;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [Test]
        public void ServiceIsAvailable()
        {
            var response = restHelper.GetQuery(RequestSpec.Query);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Service is not available");
        }

        [Test]
        public void DefaultPageSizeIsCorrect()
        {
            var response = restHelper.GetQuery(RequestSpec.Query);
            var itemsList = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            Assert.AreEqual(defaultPageSize, itemsList.Count, "Default page size is not as expected");
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        public void AllPageSizesWorkAsExpected(int pageSize)
        {
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageSizeParameter, pageSize.ToString());
            var itemsList = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
            Assert.AreEqual(pageSize, itemsList.Count, "Returned page size is not as expected");
        }

        [Test]
        public void PostRequestIsProcessedCorrectly()
        {
            var response = restHelper.PostQuery(RequestSpec.Query);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed), "POST query is expected to return 405 'Method Not allowed' got '" + response.StatusCode + "' instead");
        }

        [Test]
        public void PutRequestIsProcessedCorrectly()
        {
            var response = restHelper.PutQuery(RequestSpec.Query);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed), "PUT query is expected to return 405 'Method Not allowed' got '" + response.StatusCode + "' instead");
        }
    }
}