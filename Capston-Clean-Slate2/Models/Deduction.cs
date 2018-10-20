using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capston_Clean_Slate2.Models
{
    public class Deduction
    {
        [Key]
        public string DeductionId { get; set; }

        [Display(Name = "Deduction Type")]
        public string DeductionDescription { get; set; }

        [Display(Name = "Percent to Deduct")]
        public double DeductiblePercentage { get; set; }

        [Display(Name = "Total Amount Deducted")]
        public double AmountDeducted { get; set; }

        [ForeignKey("Employee")]
        public string Id { get; set; }

        public Employee Employee { get; set; }
    }
}