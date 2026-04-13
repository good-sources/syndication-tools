namespace AggregationService.Contracts.Responses
{
    using System;

    public class SourceResponse
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public Guid CollectionId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }
}
