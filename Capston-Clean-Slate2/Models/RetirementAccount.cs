﻿using System.ComponentModel.DataAnnotations;

namespace Capston_Clean_Slate2.Models
{
    public class RetirementAccount
    {
        [Key]
        public string RetirementAccountNumber { get; set; }

        public int TotalRetirementBalance { get; set; }
        public int EmployeeContribution { get; set; }
        public int OwnerContribution { get; set; }
        public bool CatchUpAge { get; set; }
    }
}