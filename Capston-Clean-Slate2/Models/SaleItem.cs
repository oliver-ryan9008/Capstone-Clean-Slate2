using System.ComponentModel.DataAnnotations;

namespace Capston_Clean_Slate2.Models
{
    public class SaleItem
    {
        [Key]
        public string ItemId { get; set; }

        public string ItemName { get; set; }
        public int ProfitMargin { get; set; }
        public int EmployeeItemsSold { get; set; }
    }
}