namespace AggregationService.Contracts.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateSourceRequest
    {
        [Required]
        public int Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Uri { get; set; }

        [Required]
        public Guid CollectionId { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Link { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
