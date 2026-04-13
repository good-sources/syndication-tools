namespace AggregationService
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Filters;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            MapperConfig.Initialize();

            // Web API configuration and services
            config.DependencyResolver = new DependencyResolver();
            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
            config.Filters.Add(new ValidateModelAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
