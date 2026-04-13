namespace AggregationService.Contracts.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCollectionRequest
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
