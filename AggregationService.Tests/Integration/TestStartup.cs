namespace AggregationService.Tests.Integration
{
    using System;
    using System.Web.Http;
    using Microsoft.Owin;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Security.OAuth;
    using Owin;
    using AggregationService.Auth;
    using AggregationService.Domain.Interfaces;
    using AggregationService.Contracts.Mapping;
    using AggregationService.Filters;

    public class TestStartup
    {
        private readonly ICollectionService _collectionService;
        private readonly IContentService _contentService;
        private readonly ISourceService _sourceService;

        public TestStartup(
            ICollectionService collectionService,
            IContentService contentService,
            ISourceService sourceService)
        {
            _collectionService = collectionService;
            _contentService = contentService;
            _sourceService = sourceService;
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/auth/token"),
                Provider = new SimpleAuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(oAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            MapperConfig.Initialize();

            var config = new HttpConfiguration();
            config.DependencyResolver = new TestDependencyResolver(
                _collectionService, _contentService, _sourceService);
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new ApiExceptionFilterAttribute());
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}
