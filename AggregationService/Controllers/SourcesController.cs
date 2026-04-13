namespace AggregationService.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using NLog;
    using AggregationService.Domain.Models;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Contracts.Requests;
    using System.Data.Entity.Infrastructure;

    [Authorize]
    [RoutePrefix("api/sources")]
    public class SourcesController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISourceService _sourceService;

        public SourcesController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        [Route("~/api/supportedsourcetypes")]
        public IHttpActionResult GetSupportedTypes()
        {
            try
            {
                return Json(_sourceService.GetSupportedTypes());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to retrieve supported source types");
                return InternalServerError(ex);
            }
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(CreateSourceRequest request)
        {
            try
            {
                var source = MapperConfig.Mapper.Map<Source>(request);
                return Json(await _sourceService.AddAsync(source));
            }
            catch (DbUpdateException ex)
            {
                Logger.Warn(ex, "Duplicate source conflict on POST");
                return Conflict();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to create source");
                return InternalServerError(ex);
            }
        }
    }
}
