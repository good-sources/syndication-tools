namespace AggregationService.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using AggregationService.Domain.Models;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Contracts.Requests;

    [Authorize]
    [RoutePrefix("api/sources")]
    public class SourcesController : ApiController
    {
        private readonly ISourceService _sourceService;

        public SourcesController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        [Route("~/api/supportedsourcetypes")]
        public IHttpActionResult GetSupportedTypes()
        {
            return Json(_sourceService.GetSupportedTypes());
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(CreateSourceRequest request)
        {
            var source = MapperConfig.Mapper.Map<Source>(request);
            return Json(await _sourceService.AddAsync(source));
        }
    }
}
