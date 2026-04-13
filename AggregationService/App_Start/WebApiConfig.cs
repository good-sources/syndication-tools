namespace AggregationService
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using AggregationService.Contracts.Mapping;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            MapperConfig.Initialize();

            // Web API configuration and services
            config.DependencyResolver = new DependencyResolver();
            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
