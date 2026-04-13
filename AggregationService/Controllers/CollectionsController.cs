namespace AggregationService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using NLog;
    using AggregationService.Domain.Models;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Contracts.Requests;
    using AggregationService.Contracts.Responses;

    [Authorize]
    [RoutePrefix("api/collections")]
    public class CollectionsController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICollectionService _collectionService;

        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var collections = await _collectionService.GetAsync();
                var response = MapperConfig.Mapper.Map<IEnumerable<CollectionResponse>>(collections);
                return Json(response);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to retrieve collections");
                return InternalServerError(ex);
            }
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(CreateCollectionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var collection = MapperConfig.Mapper.Map<Collection>(request);
                return Json(await _collectionService.AddAsync(collection));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to create collection");
                return InternalServerError(ex);
            }
        }
    }
}
