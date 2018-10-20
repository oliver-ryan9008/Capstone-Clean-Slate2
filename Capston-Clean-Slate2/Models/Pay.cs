using System.ComponentModel.DataAnnotations;

namespace Capston_Clean_Slate2.Models
{
    public class Pay
    {
        [Key]
        public int Id { get; set; }

        public int SalaryRate { get; set; }
        public int HourlyRate { get; set; }
        public int HoursWorked { get; set; }
        public int OvertimeRate { get; set; }
        public int SpecialPay { get; set; }
    }
}