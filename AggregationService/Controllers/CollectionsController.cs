namespace AggregationService.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AggregationService.Domain.Models;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Contracts.Requests;
    using AggregationService.Contracts.Responses;

    [Authorize]
    [RoutePrefix("api/collections")]
    public class CollectionsController : ApiController
    {
        private readonly ICollectionService _collectionService;

        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var collections = await _collectionService.GetAsync();
            var response = MapperConfig.Mapper.Map<IEnumerable<CollectionResponse>>(collections);
            return Json(response);
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(CreateCollectionRequest request)
        {
            var collection = MapperConfig.Mapper.Map<Collection>(request);
            return Json(await _collectionService.AddAsync(collection));
        }
    }
}
