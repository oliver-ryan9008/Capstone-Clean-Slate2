using Capston_Clean_Slate2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //Employee employee = new Employee();
        //Manager manager = new Manager();

        public ActionResult HomeWithPartials()
        {
            var userId = User.Identity.GetUserId();
            var employee = (from e in db.Employees where e.Id == userId select e).First();

            return View(employee);
        }

        [Authorize(Roles = "Employee")]
        public ActionResult EmployeeHome()
        {
            var userId = User.Identity.GetUserId();
            var employee = (from e in db.Employees where e.Id == userId select e).First();
            return View(employee);
        }

        // GET: Employees

        public ActionResult Index()
        {
            return RedirectToAction("EmployeeHome");
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
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

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Password,DateOfBirth,SocialSecurityNumber,FullStreetAddress,ZipCode,MailingAddress,VacationDayBalance,PersonalDayBalance,UnexcusedDaysOff,WeeklyRegularPay,WeeklyBonusPay,WeeklyCommissionSold,IsHourlyEmployee,TaxMaritalStatus,FederalIncomeTax,StateIncomeTax,SchoolDistrictTax,CityIncomeTax,UnemploymentCompensation,EmployeePayGarnishments,OwnerContributedRetirement,EmployeeContributedRetirement,EmployeeHourlyPayRate,EmployeeSalaryPayRate,DirectDeposit,EmployeeBankAccountNumber,EmployeeRoutingNumber, DayRequestWasMade, DayOffRequestType, ApprovalStatus, City")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = (from u in db.Users where u.Id == userId select u).First();

                employee.Email = user.Email;
                employee.Password = user.PasswordHash;
                employee.RetirementAccountBalance = 0;
                employee.VacationDayBalance = 10;
                employee.PersonalDayBalance = 10;
                employee.HasBeenAssignedVacationPersonalDays = true;

                employee.Id = userId;
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,FullStreetAddress,ZipCode,MailingAddress,TaxMaritalStatus,EmployeeContributedRetirement,DirectDeposit,EmployeeBankAccountNumber,EmployeeRoutingNumber,City,HasBeenAssignedVacationPersonalDays,PersonalDayBalance,VacationDayBalance")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
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

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("DisplayAllEmployees");
        }

        public void CalculatePreTaxPay(Employee employee, string id)
        {
            double grossPay;

            if (employee.IsHourlyEmployee)
            {
                grossPay = employee.HoursWorked * employee.EmployeeHourlyPayRate;
            }
            else
            {
                grossPay = employee.EmployeeSalaryPayRate / 52.00;
            }

            var grossInt = Math.Round(grossPay, 2);

            if (employee.WillEmployeeContributeRetirement == true)
            {
                var percent = (double)1 / 100;
                double empRetirementCont = (employee.EmployeeContributedRetirement * grossPay) * percent;
                double ownerRetirementCont = (employee.OwnerContributedRetirement * percent) * empRetirementCont;
                double totalRetirement = (empRetirementCont + ownerRetirementCont);
                employee.RetirementAccountBalance = Math.Round((employee.RetirementAccountBalance + totalRetirement), 2);
                grossInt = grossInt - totalRetirement;
            }

            employee.WeeklyRegularPay = grossInt;

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }

        [Authorize(Roles = "Employee")]
        public ActionResult DisplayNetPay(SaleItem saleItem)
        {
            var userId = User.Identity.GetUserId();
            Employee employee = (from e in db.Employees where e.Id == userId select e).FirstOrDefault();

            //CalculatePreTaxPay(employee, userId);
            //CalculateEmployeeCommission(employee, saleItem);
            //CalculateEmployeeBonus(employee);
            //CalculateWeeklyTaxes(employee);
            var roundedPay = Math.Round(employee.WeeklyNetPay, 2);
            employee.WeeklyNetPay = roundedPay;

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return View(employee);
        }

        public void CalculateWeeklyTaxes(Employee passedEmployee)
        {
            var employee = (from e in db.Employees where e.Id == passedEmployee.Id select e).First();
            double federalRate = employee.FederalIncomeTax;
            double stateRate = employee.StateIncomeTax;
            double cityRate = employee.CityIncomeTax;
            double schoolRate = employee.SchoolDistrictTax;
            double unemploymentRate = employee.UnemploymentCompensation;
            var garnishment = employee.EmployeePayGarnishments;

            double percent = (double)1 / 100;
            var fedIncome = employee.WeeklyRegularPay * (federalRate * percent);
            var stateIncome = employee.WeeklyRegularPay * (stateRate * percent);
            var cityIncome = employee.WeeklyRegularPay * (cityRate * percent);
            var schoolDist = employee.WeeklyRegularPay * (schoolRate * percent);
            var unemployment = employee.WeeklyRegularPay * (unemploymentRate * percent);
            employee.FederalIncomeTax = Math.Round(fedIncome, 2);
            employee.StateIncomeTax = Math.Round(stateIncome, 2);
            employee.CityIncomeTax = Math.Round(cityIncome, 2);
            employee.SchoolDistrictTax = Math.Round(schoolDist, 2);
            employee.UnemploymentCompensation = Math.Round(unemployment, 2);

            var totalTaxRemoved = Math.Round((fedIncome + stateIncome + cityIncome + schoolDist + unemployment), 2);
            employee.WeeklyNetPay = 0;
            employee.WeeklyNetPay = employee.WeeklyRegularPay - totalTaxRemoved - garnishment;
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var stringPay = employee.WeeklyNetPay.ToString("C", nfi);
            var netPayDec = double.Parse(stringPay, NumberStyles.Currency);
            employee.WeeklyNetPay = netPayDec;

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }

        //public void ConvertDoubleToCurrency(double value)
        //{
        //    var stringVal = value.ToString()
        //}

        public void CalculateEmployeeCommission(Employee employee, SaleItem saleItem)
        {
            double commissionRate = 0;
            employee.EmployeeCommissionBonus = 0;
            employee.WeeklyCommissionSold = employee.ItemsSold;
            var employeesWithCommission = (from e in db.Employees where e.WeeklyCommissionSold != 0 select e).ToList();

            foreach (var e in employeesWithCommission)
            {
                if (e.WeeklyCommissionSold >= 25 && e.WeeklyCommissionSold < 40)
                {
                    commissionRate = 0.25;
                }
                else if (e.WeeklyCommissionSold >= 41 && e.WeeklyCommissionSold < 60)
                {
                    commissionRate = 0.40;
                }
                else if (e.WeeklyCommissionSold >= 61 && e.WeeklyCommissionSold < 100)
                {
                    commissionRate = 0.50;
                }
                else if (e.WeeklyCommissionSold >= 101 && e.WeeklyCommissionSold < 149)
                {
                    commissionRate = 0.60;
                }
                else if (e.WeeklyCommissionSold >= 150)
                {
                    commissionRate = 0.75;
                }
                else if (e.WeeklyCommissionSold <= 24)
                {
                    commissionRate = 0;
                }
                e.EmployeeCommissionBonus = e.WeeklyCommissionSold * commissionRate;
            }

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }

        public ActionResult SeeCheck(string id)
        {
            var employee = (from e in db.Employees where e.Id == id select e).First();

            return View(employee);
        }

        public void CalculateEmployeeBonus(Employee employee)
        {
            double weeklyCommission = employee.EmployeeCommissionBonus;
            double holidayPay = 0;
            double randomBonus = 0;
            double totalBonus;
            totalBonus = weeklyCommission + holidayPay + randomBonus;
            employee.WeeklyBonusPay = Convert.ToInt32(totalBonus);
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult DisplayDayOffRequests()
        {
            var allEmployees = db.Employees.ToList();

            List<Employee> employeesWhoRequested = new List<Employee>();

            foreach (var a in allEmployees)
            {
                if (a.RequestDatesToString != null)
                {
                    employeesWhoRequested.Add(a);
                }
            }

            return View(employeesWhoRequested);
        }

        //Get

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ApproveSpecificDayGroup(string id)
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

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ApproveDaysOffConfirm(Manager manager, Admin admin, Employee employeeWhoRequested, string id)
        {
            var approvalBool = employeeWhoRequested.ApprovalStatus;
            employeeWhoRequested = db.Employees.Find(id);

            string approval = null;
            if (approvalBool == true)
            {
                approval = "Approved";
            }
            else
            {
                approval = "Denied";
            }

            var dateString = employeeWhoRequested.RequestDatesToString;
            string[] dates = dateString.Split(',');
            List<string> datesList = new List<string>(dates);
            employeeWhoRequested.RequestDates = datesList;
            var numberOfDatesRequested = datesList.Count - 1;

            string currentApproverName;

            if (User.IsInRole("Manager"))
            {
                var currentManagerId = User.Identity.GetUserId();
                manager = (from m in db.Managers where m.Id == currentManagerId select m).First();
                currentApproverName = manager.FirstName;
            }
            else
            {
                var currentAdminId = User.Identity.GetUserId();
                admin = (from a in db.Admins where a.Id == currentAdminId select a).First();
                currentApproverName = admin.FirstName;
            }

            string approvalStatus;

            if (approval == "Approved")
            {
                approvalStatus = "approved";
            }
            else if (approval == "Denied")
            {
                approvalStatus = "denied";
            }
            else
            {
                approvalStatus = "pending";
            }

            if (approvalStatus == "approved")
            {
                if (employeeWhoRequested.DayOffRequestType == "vacation")
                {
                    if (employeeWhoRequested.VacationDayBalance > 0)
                    {
                        int remainingVacationBalance = employeeWhoRequested.VacationDayBalance - numberOfDatesRequested;
                        employeeWhoRequested.VacationDayBalance = remainingVacationBalance;
                        employeeWhoRequested.RequestDatesToString = null;
                        employeeWhoRequested.RequestDates = null;
                        db.Entry(employeeWhoRequested).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else if (employeeWhoRequested.DayOffRequestType == "personal")
                {
                    if (employeeWhoRequested.PersonalDayBalance > 0)
                    {
                        int remainingPersonalDayBalance = employeeWhoRequested.PersonalDayBalance - numberOfDatesRequested;
                        employeeWhoRequested.PersonalDayBalance = remainingPersonalDayBalance;
                        employeeWhoRequested.RequestDatesToString = null;
                        employeeWhoRequested.RequestDates = null;
                        db.Entry(employeeWhoRequested).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            else if (approvalStatus == "denied")
            {
                employeeWhoRequested.RequestDatesToString = null;
                employeeWhoRequested.RequestDates = null;
                db.Entry(employeeWhoRequested).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (User.IsInRole("Admin"))
            {
                admin.ApprovalStatus = approvalStatus;
            }
            else if (User.IsInRole("Manager"))
            {
                manager.ApprovalStatus = approvalStatus;
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetPaycheck(Employee employee)
        {
            employee = (from e in db.Employees where e.Id == employee.Id select e).FirstOrDefault();

            return View(employee);
        }

        public ActionResult GetPaycheck(string id)
        {
            Employee employee = new Employee();
            var userId = User.Identity.GetUserId();
            employee = (from e in db.Employees where e.Id == userId select e).FirstOrDefault();

            return View(employee);
        }

        public ActionResult DisplayError()
        {
            if (Error.ErrorType == "Not enough days avaiable.")
            {
                var errorDisplayed = "There are not enough days available for this employee for this approval to be made. Be sure to check available vacation/personal days available before trying again.";
                ViewBag(errorDisplayed);
            }

            return View("DisplayError");
        }

        [Authorize(Roles = "Employee")]
        public ActionResult _RequestDays()
        {
            var employeeId = User.Identity.GetUserId();
            var employee = (from e in db.Employees where e.Id == employeeId select e).First();
            return PartialView();
        }

        [Authorize(Roles = "Employee")]
        public ActionResult RequestDayOff()
        {
            var employeeId = User.Identity.GetUserId();
            var employee = (from e in db.Employees where e.Id == employeeId select e).First();
            return View(employee);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult RequestDayOff(DateTime start, DateTime end, string request)
        {
            var employeeId = User.Identity.GetUserId();
            Employee employee = db.Employees.First(c => c.Id == employeeId);
            employee.DayOffRequestType = request;
            if (employee.HasBeenAssignedVacationPersonalDays == false)
            {
                var vacationBalance = 10;
                employee.VacationDayBalance = vacationBalance;
                var personalDayBalance = 10;
                employee.PersonalDayBalance = personalDayBalance;
                employee.HasBeenAssignedVacationPersonalDays = true;
            }

            if (ModelState.IsValid)
            {
                var startDate = start.Date;
                var endDate = end.Date;
                var datesRequested = new List<string>();

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    datesRequested.Add(date.ToShortDateString());
                }

                // joining dates employee requested to a string
                // to store in database
                StringBuilder builder = new StringBuilder();
                foreach (var date in datesRequested)
                {
                    builder.Append(date).Append(",   ");
                }
                var dateString = builder.ToString();
                employee.RequestDatesToString = dateString;

                // assigining property with date request was made
                var employeeRequestMadeDate = DateTime.Now.Date;
                employee.DayRequestWasMade = employeeRequestMadeDate.ToShortDateString();

                if (request == "vacation")
                {
                    if (employee.VacationDayBalance <= 0)
                    {
                        return View("NotEnoughVacation");
                    }
                }
                else if (request == "personal")
                {
                    if (employee.PersonalDayBalance <= 0)
                    {
                        return View("NotEnoughPersonalDays");
                    }
                }
            }
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult BankCheck(string id)
        {
            var employee = (from e in db.Employees where e.Id == id select e).First();
            RunCurrencyConverter(employee);

            return View(employee);
        }

        private void RunCurrencyConverter(Employee employee)
        {
            string convertedCurrency;
            string isNegative = "";

            var number = Convert.ToDouble(employee.WeeklyNetPay).ToString();

            if (number.Contains("-"))
            {
                isNegative = "Minus ";
                number = number.Substring(1, number.Length - 1);
            }
            if (number == "0")
            {
                convertedCurrency = "Zero";
            }
            else
            {
                convertedCurrency = isNegative + ConvertToWords(number);
            }

            employee.CurrencyAsWords = convertedCurrency;
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }

        private static string Ones(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {
                case 1:
                    name = "One";
                    break;

                case 2:
                    name = "Two";
                    break;

                case 3:
                    name = "Three";
                    break;

                case 4:
                    name = "Four";
                    break;

                case 5:
                    name = "Five";
                    break;

                case 6:
                    name = "Six";
                    break;

                case 7:
                    name = "Seven";
                    break;

                case 8:
                    name = "Eight";
                    break;

                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static string Tens(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;

                case 11:
                    name = "Eleven";
                    break;

                case 12:
                    name = "Twelve";
                    break;

                case 13:
                    name = "Thirteen";
                    break;

                case 14:
                    name = "Fourteen";
                    break;

                case 15:
                    name = "Fifteen";
                    break;

                case 16:
                    name = "Sixteen";
                    break;

                case 17:
                    name = "Seventeen";
                    break;

                case 18:
                    name = "Eighteen";
                    break;

                case 19:
                    name = "Nineteen";
                    break;

                case 20:
                    name = "Twenty";
                    break;

                case 30:
                    name = "Thirty";
                    break;

                case 40:
                    name = "Fourty";
                    break;

                case 50:
                    name = "Fifty";
                    break;

                case 60:
                    name = "Sixty";
                    break;

                case 70:
                    name = "Seventy";
                    break;

                case 80:
                    name = "Eighty";
                    break;

                case 90:
                    name = "Ninety";
                    break;

                default:
                    if (_Number > 0)
                    {
                        name = Tens(Number.Substring(0, 1) + "0") + " " + Ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static string ConvertWholeNumber(string Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range

                            word = Ones(Number);
                            isDone = true;
                            break;

                        case 2://tens' range
                            word = Tens(Number);
                            isDone = true;
                            break;

                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;

                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;

                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;

                        case 10://Billions's range
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        private static string ConvertToWords(string numb)
        {
            string val = "", wholeNo = numb, points = "", andStr = "Dollars and", pointStr = "";
            string endStr = "Zero Cents ";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "Dollars and";// just to separate whole numbers from points/cents
                        endStr = "Cents ";//Cents
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = string.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static string ConvertDecimals(string number)
        {
            string cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = Ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        public ActionResult ViewDayOffStatus()
        {
            var id = User.Identity.GetUserId();
            var employee = (from e in db.Employees where e.Id == id select e).First();
            return View(employee);
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