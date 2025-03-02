using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PricePrediction.Core.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        [Column(Order = 0)]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
