namespace AggregationService.Filters
{
    using System.Data.Entity.Infrastructure;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using AggregationService.Contracts.Responses;
    using NLog;

    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext context)
        {
            ApiError error;
            HttpStatusCode statusCode;

            if (context.Exception is DbUpdateException)
            {
                Logger.Warn(context.Exception, "Database conflict on {Method} {Path}",
                    context.Request.Method, context.Request.RequestUri);

                statusCode = HttpStatusCode.Conflict;
                error = new ApiError
                {
                    Message = "A conflicting record already exists.",
                    Code = "CONFLICT"
                };
            }
            else
            {
                Logger.Error(context.Exception, "Unhandled exception on {Method} {Path}",
                    context.Request.Method, context.Request.RequestUri);

                statusCode = HttpStatusCode.InternalServerError;
                error = new ApiError
                {
                    Message = "An unexpected error occurred.",
                    Code = "INTERNAL_ERROR"
                };
            }

            context.Response = context.Request.CreateResponse(statusCode, error);
        }
    }
}
