namespace AggregationService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Contracts.Responses;

    [Authorize]
    [RoutePrefix("api/contents")]
    public class ContentsController : ApiController
    {
        private readonly IContentService _contentService;

        public ContentsController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [Route("bycollection/{id:guid}")]
        public async Task<IHttpActionResult> GetByCollection(Guid id)
        {
            var contents = await _contentService.GetByCollectionAsync(id);
            var response = MapperConfig.Mapper.Map<IEnumerable<ContentResponse>>(contents);
            return Json(response);
        }
    }
}
