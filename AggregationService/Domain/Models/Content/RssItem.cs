namespace AggregationService.Domain.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class RssItem : FeedContent
    {
        public string Description { get; set; }

        [ForeignKey("SourceId")]
        public RssFeed Feed
        {
            get => (RssFeed)Source;
            set => Source = value;
        }
    }
}
