namespace AggregationService.Contracts.Mapping
{
    using AutoMapper;

    public static class MapperConfig
    {
        private static IMapper _mapper;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    Initialize();
                }
                return _mapper;
            }
        }

        public static void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AggregationServiceMappingProfile>();
            });

            _mapper = config.CreateMapper();
        }
    }
}
