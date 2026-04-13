namespace AggregationService.Contracts.Mapping
{
    using System;
    using AutoMapper;
    using AggregationService.Domain.Models;
    using AggregationService.Contracts.Requests;
    using AggregationService.Contracts.Responses;

    public class AggregationServiceMappingProfile : Profile
    {
        public AggregationServiceMappingProfile()
        {
            // Entity -> Response mappings

            CreateMap<Collection, CollectionResponse>();

            CreateMap<RssFeed, SourceResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<Source, SourceResponse>()
                .IncludeAllDerived();

            CreateMap<RssItem, ContentResponse>();

            CreateMap<FeedContent, ContentResponse>()
                .IncludeAllDerived();

            CreateMap<Content, ContentResponse>()
                .IncludeAllDerived();

            // Request -> Entity mappings

            CreateMap<CreateCollectionRequest, Collection>();

            CreateMap<CreateSourceRequest, Source>()
                .ConvertUsing((src, dest, ctx) =>
                {
                    switch ((SourceType)src.Type)
                    {
                        case SourceType.RSS:
                            return new RssFeed
                            {
                                Uri = src.Uri,
                                CollectionId = src.CollectionId,
                                Title = src.Title,
                                Link = src.Link,
                                Description = src.Description
                            };
                        default:
                            throw new NotSupportedException("Unknown source type: " + src.Type);
                    }
                });
        }
    }
}
