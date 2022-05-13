using API_test.RequestSpecs;
using API_test.Helpers;
using NUnit.Framework;
using System.Net;

namespace API_test
{
    public class HttpResponseNUnitTests
    {
        private RestHelper restHelper;

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
        public void PostRequestIsProcessedCorrectly()
        {
            var response = restHelper.PostQuery(RequestSpec.Query);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed), "POST query returned unexpected status code");
        }

        [Test]
        public void PutRequestIsProcessedCorrectly()
        {
            var response = restHelper.PutQuery(RequestSpec.Query);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed), "PUT query returned unexpected status code");
        }
    }
}