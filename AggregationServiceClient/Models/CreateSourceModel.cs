namespace AggregationServiceClient.Models
{
    using System;

    public class CreateSourceModel
    {
        public int Type { get; set; }
        public string Uri { get; set; }
        public Guid CollectionId { get; set; }
    }
}
