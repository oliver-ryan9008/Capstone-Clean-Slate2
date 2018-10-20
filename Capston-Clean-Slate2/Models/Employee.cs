using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Capston_Clean_Slate2.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        public string DateOfBirth { get; set; }

        [Display(Name = "Social Security Number")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Street Address")]
        public string FullStreetAddress { get; set; }

        public string City { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }

        [Display(Name = "Vacation Balance")]
        public int VacationDayBalance { get; set; }

        [Display(Name = "PTO Balance")]
        public int PersonalDayBalance { get; set; }

        [Display(Name = "Occurences")]
        public int UnexcusedDaysOff { get; set; }

        [Display(Name = "Weekly Regular Gross Pay")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double WeeklyRegularPay { get; set; }

        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Display(Name = "Weekly Bonuses")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double WeeklyBonusPay { get; set; }

        [Display(Name = "Weekly Commission")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int WeeklyCommissionSold { get; set; }

        [Display(Name = "Hourly Employee")]
        public bool IsHourlyEmployee { get; set; }

        public string TaxMaritalStatus { get; set; }

        [Display(Name = "Federal Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double FederalIncomeTax { get; set; }

        [Display(Name = "State Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double StateIncomeTax { get; set; }

        [Display(Name = "School District Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double SchoolDistrictTax { get; set; }

        [Display(Name = "City Income Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double CityIncomeTax { get; set; }

        [Display(Name = "Unemployment")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double UnemploymentCompensation { get; set; }

        [Display(Name = "Garnished Wages")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double EmployeePayGarnishments { get; set; }

        [Display(Name = "Company Retirement Match %")]
        public int OwnerContributedRetirement { get; set; }

        [Display(Name = "Retirement Opt-In")]
        public bool WillEmployeeContributeRetirement { get; set; }

        [Display(Name = "Retirement Contribution %")]
        public int EmployeeContributedRetirement { get; set; }

        [Display(Name = "Retirement Account Number")]
        public string RetirementAccountNumber { get; set; }

        [Display(Name = "Retirement Account Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double RetirementAccountBalance { get; set; }

        [Display(Name = "Employee Hourly Pay")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double EmployeeHourlyPayRate { get; set; }

        [Display(Name = "Employee Salary")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double EmployeeSalaryPayRate { get; set; }

        [Display(Name = "Commission Bonus")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double EmployeeCommissionBonus { get; set; }

        [Display(Name = "Direct Deposit Opt-In")]
        public bool DirectDeposit { get; set; }

        public string EmployeeBankAccountNumber { get; set; }
        public string EmployeeRoutingNumber { get; set; }

        [Display(Name = "Requested Since")]
        public string DayRequestWasMade { get; set; }

        [Display(Name = "Dates Requested Off")]
        public List<string> RequestDates { get; set; }

        [Display(Name = "Off Dates Requested")]
        public string RequestDatesToString { get; set; }

        [Display(Name = "Request Type")]
        public string DayOffRequestType { get; set; }

        public bool HasBeenAssignedVacationPersonalDays { get; set; }

        [Display(Name = "Approved By")]
        public string Approver { get; set; }

        [Display(Name = "Approval Status")]
        public bool? ApprovalStatus { get; set; }

        [Display(Name = "Total Net Pay")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double WeeklyNetPay { get; set; }

        public string CurrencyAsWords { get; set; }

        [Display(Name = "Commission Items Sold")]
        public int ItemsSold { get; set; }

        public int? TotalDeductions { get; set; }
    }
}