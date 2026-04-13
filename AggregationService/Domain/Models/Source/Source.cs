namespace AggregationService.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Source
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Uri { get; set; }

        public abstract SourceType Type { get; }

        public DateTimeOffset? LastlyModified { get; set; }
        public DateTime? Expires { get; set; }

        [ForeignKey("Collection")]
        public Guid CollectionId { get; set; }

        public Collection Collection { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
    }

    public enum SourceType
    {
        RSS
    }
}
