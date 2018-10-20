namespace Capston_Clean_Slate2.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    FirstName = c.String(),
                    LastName = c.String(),
                    Email = c.String(),
                    Password = c.String(),
                    DateOfBirth = c.String(),
                    SocialSecurityNumber = c.String(),
                    FullStreetAddress = c.String(),
                    City = c.String(),
                    ZipCode = c.String(),
                    MailingAddress = c.String(),
                    VacationDayBalance = c.Int(nullable: false),
                    PersonalDayBalance = c.Int(nullable: false),
                    UnexcusedDaysOff = c.Int(nullable: false),
                    WeeklyRegularPay = c.Double(nullable: false),
                    HoursWorked = c.Double(nullable: false),
                    WeeklyBonusPay = c.Double(nullable: false),
                    WeeklyCommissionSold = c.Int(nullable: false),
                    IsHourlyEmployee = c.Boolean(nullable: false),
                    TaxMaritalStatus = c.String(),
                    FederalIncomeTax = c.Double(nullable: false),
                    StateIncomeTax = c.Double(nullable: false),
                    SchoolDistrictTax = c.Double(nullable: false),
                    CityIncomeTax = c.Double(nullable: false),
                    UnemploymentCompensation = c.Double(nullable: false),
                    EmployeePayGarnishments = c.Double(nullable: false),
                    OwnerContributedRetirement = c.Int(nullable: false),
                    WillEmployeeContributeRetirement = c.Boolean(nullable: false),
                    EmployeeContributedRetirement = c.Int(nullable: false),
                    RetirementAccountNumber = c.String(),
                    RetirementAccountBalance = c.Double(nullable: false),
                    EmployeeHourlyPayRate = c.Double(nullable: false),
                    EmployeeSalaryPayRate = c.Double(nullable: false),
                    EmployeeCommissionBonus = c.Double(nullable: false),
                    DirectDeposit = c.Boolean(nullable: false),
                    EmployeeBankAccountNumber = c.String(),
                    EmployeeRoutingNumber = c.String(),
                    DayRequestWasMade = c.String(),
                    RequestDatesToString = c.String(),
                    DayOffRequestType = c.String(),
                    HasBeenAssignedVacationPersonalDays = c.Boolean(nullable: false),
                    Approver = c.String(),
                    ApprovalStatus = c.Boolean(),
                    WeeklyNetPay = c.Double(nullable: false),
                    CurrencyAsWords = c.String(),
                    ItemsSold = c.Int(nullable: false),
                    TotalDeductions = c.Int(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}