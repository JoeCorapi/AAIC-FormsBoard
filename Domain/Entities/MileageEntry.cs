using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormsBoard.Domain.Entities
{
    [Table("MileageLineItem")]
    public class MileageLineItem
    {
        public int Id { get; set; }

        public int MileageFormId { get; set; }
        public virtual MileageForm MileageForm { get; set; }

        [Required]
        public DateTime TravelDate { get; set; }

        [Required]
        [StringLength(100)]
        public string StartLocation { get; set; }

        [Required]
        [StringLength(100)]
        public string EndLocation { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.1, 1000)]
        public decimal TotalMiles { get; set; }
    }
}
