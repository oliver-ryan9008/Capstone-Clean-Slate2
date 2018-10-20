using Capston_Clean_Slate2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        private EmployeesController employeesClass = new EmployeesController();

        //SaleItem saleItem = new SaleItem();
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AdminDashboard()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult Scheduling()
        {
            var employee = (from e in db.Employees where e.Id != null select e).First();
            var schedule = new Schedule();
            schedule.Employee = employee;
            schedule.Id = employee.Id;
            schedule.Employee.Id = employee.Id;

            return View(schedule);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DisplayAllEmployees()
        {
            return View(db.Employees.ToList());
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, FirstName, LasterName, UserName, Email, Password, ApprovalStatus")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.Id = User.Identity.GetUserId();
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult GetEmployeeWeeklyHours(Employee employee)
        {
            employee = (from e in db.Employees where e.Id == employee.Id select e).FirstOrDefault();

            return View(employee);
        }

        [HttpPost, ActionName("GetEmployeeWeeklyHours")]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeeHoursWorked(Employee employee, string id)
        {
            Employee currentEmployee = (from e in db.Employees where e.Id == id select e).FirstOrDefault();
            var user = (from u in db.Users where u.Id == employee.Id select u).First();
            var userId = user.Id;
            var saleItem = new SaleItem();
            ResetTaxBalance(currentEmployee);
            currentEmployee.HoursWorked = employee.HoursWorked;
            currentEmployee.ItemsSold = employee.ItemsSold;
            employeesClass.CalculatePreTaxPay(currentEmployee, userId);
            employeesClass.CalculateEmployeeCommission(currentEmployee, saleItem);
            employeesClass.CalculateEmployeeBonus(currentEmployee);
            employeesClass.CalculateWeeklyTaxes(employee);

            if (ModelState.IsValid)
            {
                db.Entry(currentEmployee).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("GetEmployeeInformation");
        }

        public ActionResult DisplayHoursWorked(Employee employee, string id)
        {
            Employee currentEmployee = (from e in db.Employees where e.Id == id select e).FirstOrDefault();
            var hours = employee.HoursWorked;
            if (currentEmployee != null)
            {
                currentEmployee.HoursWorked = hours;
                return View(currentEmployee);
            }
            else
            {
                return RedirectToAction("GetEmployeeInformation");
            }
        }

        public ActionResult GetEmployeeInformation()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult ChooseEmployeeForRetirementInfo()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult CreateRetirementAccount(string id)
        {
            Employee employee = db.Employees.Find(id);
            RetirementAccount account = new RetirementAccount();
            Random rnd = new Random();
            var accountNum = rnd.Next(100000, 999999).ToString();
            account.RetirementAccountNumber = accountNum;

            if (employee != null)
            {
                employee.RetirementAccountNumber = accountNum;
            }

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return View(account);
        }

        [HttpPost, ActionName("CreateRetirementAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRetirementAccountConfirm(RetirementAccount account)
        {
            Employee employee = (from e in db.Employees where e.RetirementAccountNumber == account.RetirementAccountNumber select e).First();
            if (employee != null)
            {
                employee.EmployeeContributedRetirement = account.EmployeeContribution;
                employee.OwnerContributedRetirement = account.OwnerContribution;
                employee.WillEmployeeContributeRetirement = true;

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ChooseEmployeeForRetirementInfo");
        }

        public ActionResult SetEmployeePayScale(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        public ActionResult SetEmployeePayScaleConfirm([Bind(Include = "Id, EmployeeHourlyPayRate, EmployeeSalaryPayRate, IsHourlyEmployee")] Employee employee)
        {
            var updateEmployee = (from e in db.Employees where e.Id == employee.Id select e).FirstOrDefault();

            //var isHourlyBool = employee.IsHourlyEmployee;

            if (updateEmployee != null)
            {
                updateEmployee.IsHourlyEmployee = employee.IsHourlyEmployee;

                if (employee.IsHourlyEmployee)
                {
                    updateEmployee.EmployeeHourlyPayRate = employee.EmployeeHourlyPayRate;
                }
                else if (!employee.IsHourlyEmployee)
                {
                    updateEmployee.EmployeeSalaryPayRate = employee.EmployeeSalaryPayRate;
                }
            }
            else
            {
                return RedirectToAction("SetEmployeePayScale");
            }

            //employee = (from e in db.Employees where e.Id == employee.Id select e).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(updateEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetEmployeeInformation");
            }
            return RedirectToAction("GetEmployeeInformation");
        }

        public ActionResult SetEmployeeTaxes()
        {
            return View();
        }

        public void ResetTaxBalance(Employee employee)
        {
            var taxRateGroup = (from t in db.Taxes where t.Id == employee.Id select t).First();
            employee.FederalIncomeTax = taxRateGroup.FederalIncomeRate;
            employee.StateIncomeTax = taxRateGroup.StateIncomeRate;
            employee.SchoolDistrictTax = taxRateGroup.SchoolDistrict;
            employee.CityIncomeTax = taxRateGroup.CityIncomeRate;
            employee.UnemploymentCompensation = taxRateGroup.UnemploymentCompensation;
            employee.EmployeePayGarnishments = taxRateGroup.Garnishment;
        }

        [HttpPost, ActionName("SetEmployeeTaxes")]
        public ActionResult SetEmployeeTaxesConfirmed(string id)
        {
            var taxRateGroup = (from t in db.Taxes where t.Id == id select t).First();
            var employee = (from e in db.Employees where e.Id == id select e).First();
            var user = (from u in db.Users where u.Id == employee.Id select u).First();
            var userId = user.Id;
            employee.FederalIncomeTax = taxRateGroup.FederalIncomeRate;
            employee.StateIncomeTax = taxRateGroup.StateIncomeRate;
            employee.SchoolDistrictTax = taxRateGroup.SchoolDistrict;
            employee.CityIncomeTax = taxRateGroup.CityIncomeRate;
            employee.UnemploymentCompensation = taxRateGroup.UnemploymentCompensation;
            employee.EmployeePayGarnishments = taxRateGroup.Garnishment;

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();

            return View();
        }

        public ActionResult ShowDeductions()
        {
            var deductions = db.Deductions.ToList();

            return View(deductions);
        }

        public ActionResult AddEmployeeDeductions()
        {
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName");
            ViewBag.deductions = db.Deductions.ToList().ToString();
            return View();
        }

        [HttpPost, ActionName("AddEmployeeDeductions")]
        public ActionResult AddEmployeeDeductionsConfirmed([Bind(Include = "DeductionId, DeductionDescription, DeductiblePercentage, AmountDeducted, Id")] Deduction deduction)
        {
            var number = db.Deductions.ToList().Count() + 1;
            var employee = (from e in db.Employees where e.Id == deduction.Id select e).First();
            var pay = employee.WeeklyRegularPay;
            var percent = (double)1 / 100;
            deduction.AmountDeducted = (deduction.DeductiblePercentage * percent) * pay;
            deduction.DeductionId = number.ToString();
            deduction.Employee = employee;
            db.Deductions.Add(deduction);
            db.SaveChanges();
            return RedirectToAction("ShowDeductions");
        }

        public MultiSelectList GetDeductions(string[] selectedValues)
        {
            List<Deductible> deductions = new List<Deductible>()
            {
                new Deductible() {Amount = 6300, Description = "Standard Deduction"},
                new Deductible() {Amount = 10000, Description = "Medical Expenses"},
                new Deductible() {Amount = 5000, Description = "Mortgage Interest"},
                new Deductible() {Amount = 5000, Description = "Donations/Charity"},
                new Deductible() {Amount = 500, Description = "Tax Preparation Fees"}
            };
            return new MultiSelectList(deductions, "Amount", "Description", selectedValues);
        }

        public ActionResult ChooseDeductions()
        {
            var employees = db.Employees.ToList();
            ViewData["deductionslist"] = GetDeductions(null);
            return View(employees);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

//double federalRate = 0;
//double stateRate = 0;
//double cityRate = 0;
//double schoolRate = 0;
//var garnishment = employee.EmployeePayGarnishments;
//double totalTaxes;

//double employeesAtBusiness = db.Employees.Count();
//double unemploymentRate = 9 / employeesAtBusiness;

//var taxMarital = employee.TaxMaritalStatus;
//bool married;

//if (taxMarital.ToLower() == "single")
//{
//    married = false;
//}
//else
//{
//    married = true;
//}

//if (employee.ZipCode == "43130")
//{
//    cityRate = 6;
//    schoolRate = 2;
//}
//else if (employee.ZipCode == "45640")
//{
//    cityRate = 1.5;
//    schoolRate = 0;
//}
//else if (employee.ZipCode == "43215")
//{
//    cityRate = 2.5;
//    schoolRate = 0.75;
//}

//else
//{
//    cityRate = 2;
//    schoolRate = 1;
//}

//if (employee.WeeklyRegularPay <= 183)
//{
//    federalRate = 10;
//    stateRate = 0;
//}
//else if (employee.WeeklyRegularPay > 183 && employee.WeeklyRegularPay <= 744)
//{
//    federalRate = 12;
//    stateRate = 1.98;
//}
//else if (employee.WeeklyRegularPay > 745 && employee.WeeklyRegularPay <= 1587)
//{
//    federalRate = 22;
//    stateRate = 2.48;
//}
//else if (employee.WeeklyRegularPay > 1588 && employee.WeeklyRegularPay <= 3029)
//{
//    federalRate = 24;
//    stateRate = 3;
//}
//else if (employee.WeeklyRegularPay > 3030 && employee.WeeklyRegularPay <= 3846)
//{
//    federalRate = 32;
//    stateRate = 3.47;
//}
//else if (employee.WeeklyRegularPay > 3847 && employee.WeeklyRegularPay <= 9615)
//{
//    federalRate = 35;
//    stateRate = 4;
//}
//else
//{
//    federalRate = 37;
//    stateRate = 4.6;
//}

//if (!married)
//{
//    totalTaxes = federalRate + stateRate + cityRate + schoolRate + unemploymentRate;
//}
//else
//{
//    totalTaxes = (federalRate + stateRate + cityRate + schoolRate + unemploymentRate) * 1.25;
//}
//var totalTaxesInt = totalTaxes;
//double percent = (double)1 / 100;
//var fedIncome = employee.WeeklyRegularPay * (federalRate * percent);
//var stateIncome = employee.WeeklyRegularPay * (stateRate * percent);
//var cityIncome = employee.WeeklyRegularPay * (cityRate * percent);
//var schoolDist = employee.WeeklyRegularPay * (schoolRate * percent);
//var unemployment = employee.WeeklyRegularPay * (unemploymentRate * percent);
//employee.FederalIncomeTax = Math.Round(fedIncome, 2);
//employee.StateIncomeTax = Math.Round(stateIncome, 2);
//employee.CityIncomeTax = Math.Round(cityIncome, 2);
//employee.SchoolDistrictTax = Math.Round(schoolDist, 2);
//employee.UnemploymentCompensation = Math.Round(unemployment, 2);

//var totalTaxRemoved = Math.Round((fedIncome + stateIncome + cityIncome + schoolDist + unemployment), 2);

//employee.WeeklyNetPay = employee.WeeklyRegularPay - totalTaxRemoved - garnishment;

//db.Entry(employee).State = EntityState.Modified;
//db.SaveChanges();

public class Deductible
{
    public int Amount { get; set; }
    public string Description { get; set; }
}