using System;
using System.Collections.Generic;

namespace Capston_Clean_Slate2.Models
{
    public class EmployeeScheduleVM
    {
        public List<Employee> EmployeeList { get; set; }
        public List<DateTime> Dates { get; set; }
        public Employee Employee { get; set; }
    }
}