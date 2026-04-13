namespace AggregationService.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Moq;
    using NUnit.Framework;
    using AggregationService.Controllers;
    using AggregationService.Domain.Models;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Contracts.Requests;

    [TestFixture]
    public class SourcesControllerTests
    {
        private Mock<ISourceService> _mockService;
        private SourcesController _controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MapperConfig.Initialize();
        }

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<ISourceService>();
            _controller = new SourcesController(_mockService.Object)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void GetSupportedTypes_ReturnsJsonResult_WithDictionary()
        {
            var types = new Dictionary<string, int> { { "RSS", 0 } };
            _mockService.Setup(s => s.GetSupportedTypes()).Returns(types);

            var result = _controller.GetSupportedTypes();

            Assert.That(result, Is.TypeOf<JsonResult<IDictionary<string, int>>>());
        }

        [Test]
        public void GetSupportedTypes_ThrowsException_WhenServiceThrows()
        {
            _mockService.Setup(s => s.GetSupportedTypes()).Throws(new Exception("Error"));

            Assert.Throws<Exception>(() => _controller.GetSupportedTypes());
        }

        [Test]
        public async Task Post_ReturnsJsonResult_WithGuid_WhenModelIsValid()
        {
            var expectedId = Guid.NewGuid();
            _mockService.Setup(s => s.AddAsync(It.IsAny<Source>())).ReturnsAsync(expectedId);

            var request = new CreateSourceRequest
            {
                Type = (int)SourceType.RSS,
                Uri = "https://example.com/feed",
                CollectionId = Guid.NewGuid()
            };
            var result = await _controller.Post(request);

            Assert.That(result, Is.TypeOf<JsonResult<Guid>>());
            var jsonResult = (JsonResult<Guid>)result;
            Assert.That(jsonResult.Content, Is.EqualTo(expectedId));
        }

        [Test]
        public void Post_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var controllerContext = new System.Web.Http.Controllers.HttpControllerContext(
                _controller.Configuration,
                new System.Web.Http.Routing.HttpRouteData(new System.Web.Http.Routing.HttpRoute()),
                _controller.Request);
            var actionContext = new System.Web.Http.Controllers.HttpActionContext(
                controllerContext,
                new Mock<System.Web.Http.Controllers.HttpActionDescriptor>().Object);
            actionContext.ModelState.AddModelError("Uri", "Required");

            var filter = new AggregationService.Filters.ValidateModelAttribute();
            filter.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_ThrowsDbUpdateException_WhenDuplicateSource()
        {
            _mockService.Setup(s => s.AddAsync(It.IsAny<Source>()))
                .ThrowsAsync(new DbUpdateException("Duplicate"));

            var request = new CreateSourceRequest
            {
                Type = (int)SourceType.RSS,
                Uri = "https://example.com/feed",
                CollectionId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<DbUpdateException>(() => _controller.Post(request));
        }

        [Test]
        public void Post_ThrowsException_WhenGeneralExceptionThrown()
        {
            _mockService.Setup(s => s.AddAsync(It.IsAny<Source>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var request = new CreateSourceRequest
            {
                Type = (int)SourceType.RSS,
                Uri = "https://example.com/feed",
                CollectionId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<Exception>(() => _controller.Post(request));
        }
    }
}
