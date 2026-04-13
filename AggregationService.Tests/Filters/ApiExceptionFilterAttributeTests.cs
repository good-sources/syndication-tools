namespace AggregationService.Tests.Filters
{
    using AggregationService.Contracts.Responses;
    using AggregationService.Filters;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Filters;

    [TestFixture]
    public class ApiExceptionFilterAttributeTests
    {
        private ApiExceptionFilterAttribute _filter;

        [SetUp]
        public void SetUp()
        {
            _filter = new ApiExceptionFilterAttribute();
        }

        [Test]
        public void OnException_DbUpdateException_ReturnsConflictWithApiError()
        {
            var context = CreateContext(new DbUpdateException("Duplicate"));

            _filter.OnException(context);

            Assert.That(context.Response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            var error = ReadApiError(context.Response);
            Assert.That(error.Code, Is.EqualTo("CONFLICT"));
            Assert.That(error.Message, Is.Not.Empty);
        }

        [Test]
        public void OnException_GeneralException_ReturnsInternalServerErrorWithApiError()
        {
            var context = CreateContext(new InvalidOperationException("Something broke"));

            _filter.OnException(context);

            Assert.That(context.Response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            var error = ReadApiError(context.Response);
            Assert.That(error.Code, Is.EqualTo("INTERNAL_ERROR"));
            Assert.That(error.Message, Does.Not.Contain("Something broke"));
        }

        private static HttpActionExecutedContext CreateContext(Exception exception)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/test");
            request.SetConfiguration(config);

            var controllerContext = new System.Web.Http.Controllers.HttpControllerContext(
                config,
                new System.Web.Http.Routing.HttpRouteData(new System.Web.Http.Routing.HttpRoute()),
                request);

            var actionContext = new System.Web.Http.Controllers.HttpActionContext(
                controllerContext,
                new Moq.Mock<System.Web.Http.Controllers.HttpActionDescriptor>().Object);

            return new HttpActionExecutedContext(actionContext, exception);
        }

        private static ApiError ReadApiError(HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiError>(json);
        }
    }
}
