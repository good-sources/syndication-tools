namespace AggregationService.Filters
{
    using AggregationService.Contracts.Responses;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value.Errors.Select(err =>
                            string.IsNullOrEmpty(err.ErrorMessage)
                                ? err.Exception?.Message ?? "Invalid value"
                                : err.ErrorMessage
                        ).ToArray()
                    );

                var apiError = new ApiError
                {
                    Message = "Validation failed.",
                    Code = "VALIDATION_ERROR",
                    Errors = errors
                };

                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest, apiError);
            }
        }
    }
}
