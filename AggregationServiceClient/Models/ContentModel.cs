namespace AggregationServiceClient.Models
{
    using System;

    public class ContentModel
    {
        public Guid Id { get; set; }
        public DateTime? Published { get; set; }
        public Guid SourceId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}
