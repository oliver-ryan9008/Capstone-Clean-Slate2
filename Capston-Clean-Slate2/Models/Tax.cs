using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capston_Clean_Slate2.Models
{
    public class Tax
    {
        [Key]
        public Guid TaxId { get; set; }

        [Display(Name = "Federal Income Tax")]
        public double FederalIncomeRate { get; set; }

        [Display(Name = "State Income Tax")]
        public double StateIncomeRate { get; set; }

        [Display(Name = "School District Tax")]
        public double SchoolDistrict { get; set; }

        [Display(Name = "City Tax")]
        public double CityIncomeRate { get; set; }

        [Display(Name = "Unemployment Insurance")]
        public double UnemploymentCompensation { get; set; }

        [Display(Name = "Misc. Garnishments")]
        public double Garnishment { get; set; }

        public double? GarnishmentAmount { get; set; }

        [Display(Name = "Total Deductions")]
        public double DeductionStatus { get; set; }

        [ForeignKey("Employee")]
        public string Id { get; set; }

        public Employee Employee { get; set; }
    }
}