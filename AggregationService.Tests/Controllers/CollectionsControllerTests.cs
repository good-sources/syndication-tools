namespace AggregationService.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
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
    using AggregationService.Contracts.Responses;

    [TestFixture]
    public class CollectionsControllerTests
    {
        private Mock<ICollectionService> _mockService;
        private CollectionsController _controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MapperConfig.Initialize();
        }

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<ICollectionService>();
            _controller = new CollectionsController(_mockService.Object)
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
        public async Task Get_ReturnsJsonResult_WithCollections()
        {
            var collections = new List<Collection>
            {
                new Collection { Id = Guid.NewGuid(), Name = "Test" }
            };
            _mockService.Setup(s => s.GetAsync()).ReturnsAsync(collections);

            var result = await _controller.Get();

            Assert.That(result, Is.TypeOf<JsonResult<IEnumerable<CollectionResponse>>>());
        }

        [Test]
        public async Task Get_ReturnsInternalServerError_WhenServiceThrows()
        {
            _mockService.Setup(s => s.GetAsync()).ThrowsAsync(new Exception("DB error"));

            var result = await _controller.Get();

            Assert.That(result, Is.TypeOf<ExceptionResult>());
        }

        [Test]
        public async Task Post_ReturnsJsonResult_WithGuid_WhenModelIsValid()
        {
            var expectedId = Guid.NewGuid();
            _mockService.Setup(s => s.AddAsync(It.IsAny<Collection>())).ReturnsAsync(expectedId);

            var result = await _controller.Post(new CreateCollectionRequest { Name = "New" });

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
            actionContext.ModelState.AddModelError("Name", "Required");

            var filter = new Filters.ValidateModelAttribute();
            filter.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Post_ReturnsInternalServerError_WhenServiceThrows()
        {
            _mockService.Setup(s => s.AddAsync(It.IsAny<Collection>()))
                .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.Post(new CreateCollectionRequest { Name = "Test" });

            Assert.That(result, Is.TypeOf<ExceptionResult>());
        }
    }
}
