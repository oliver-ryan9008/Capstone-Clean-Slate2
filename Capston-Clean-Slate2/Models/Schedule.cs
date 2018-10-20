using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capston_Clean_Slate2.Models
{
    public class Schedule
    {
        [Key]
        public string ScheduleId { get; set; }

        public string Text { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("Employee")]
        public string Id { get; set; }

        public Employee Employee { get; set; }
    }
}