using System;
using System.ComponentModel.DataAnnotations;

namespace Capston_Clean_Slate2.Models
{
    public class Event
    {
        [Key]
        public int id { get; set; }

        public string text { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}