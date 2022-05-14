using API_test.RequestSpecs;
using API_test.Helpers;
using NUnit.Framework;
using API_test.ApiResponseModels;
using System.Net;

namespace API_test.Tests
{
    public class PageParamNUnitTests
    {
        private RestHelper restHelper;

        [SetUp]
        public void Setup()
        {
            restHelper = new RestHelper(RequestSpec.BackendUrl);
        }

        [TestCase("1")]
        [TestCase("2")]
        public void SwitchingBetweenPagesShouldWorkAsExpected(string pageNumber)
        {
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageNumberParameter, pageNumber);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
        }

        //В данном кейсе я полагаю, что факт того, что каждая предыдущая страница заканчивается на неком item, 
        //а следующая страница начинается с него же - ожидаемое поведение
        [Test]
        public void SwitchingBetweenPagesReturnsCorrectTotalNumberOfItems()
        {
            ResponseDataRetriever dataRetriever = new ResponseDataRetriever();
            int expectedNumberOfItems = dataRetriever.GetExpectedlNumberOfItems(restHelper);
            int retrievedNumberOfItems = dataRetriever.GetAllUniqueItems(restHelper).Count;
            Assert.AreEqual(expectedNumberOfItems, retrievedNumberOfItems, "Got unexpected total number of items");
        }

        [Test]
        public void NegativePageSizeShouldBeHandledCorrectly()
        {
            const string negativePageNumber = "-1";
            const string expectedErrorMessage = "Параметр 'page' должен быть больше 0";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageNumberParameter, negativePageNumber);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }

        [Test]
        public void ZeroPageSizeShouldBeHandledCorrectly()
        {
            const string negativePageNumber = "0";
            const string expectedErrorMessage = "Параметр 'page' должен быть больше 0";
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageNumberParameter, negativePageNumber);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Got unexpected status code");
            var error = response.Content.SelectToken("error").ToObject<ErrorDto>();
            Assert.AreEqual(expectedErrorMessage.Normalize(), error.Message.Normalize(), "Got unexpected error message");
        }
    }
}
