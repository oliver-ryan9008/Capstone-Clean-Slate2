using Capston_Clean_Slate2.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class TaxesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Taxes
        public ActionResult Index()
        {
            var taxes = db.Taxes.Include(t => t.Employee);
            return View(taxes.ToList());
        }

        // GET: Taxes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // GET: Taxes/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Taxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxId,FederalIncomeRate,StateIncomeRate,SchoolDistrict,CityIncomeRate,UnemploymentCompensation,Garnishment,GarnishmentAmount,DeductionStatus,Id")] Tax tax)
        {
            var employee = (from e in db.Employees where e.Id == tax.Id select e).First();
            var oldTaxes = (from t in db.Taxes where t.Id == tax.Id select t).ToList();
            if (oldTaxes.Count() != 0)
            {
                foreach (var o in oldTaxes)
                {
                    db.Taxes.Remove(o);
                }
            }
            tax.Employee = employee;
            tax.TaxId = Guid.NewGuid();
            db.Taxes.Add(tax);
            db.SaveChanges();
            SaveTaxInfo(tax.Id);

            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", tax.Id);
            return RedirectToAction("Index", "Home");
        }

        public void SaveTaxInfo(string id)
        {
            if (id != null)
            {
                var emp = (from e in db.Employees where e.Id == id select e).First();
                var tax = (from t in db.Taxes where t.Id == id select t).First();
                emp.FederalIncomeTax = tax.FederalIncomeRate;
                emp.StateIncomeTax = tax.StateIncomeRate;
                emp.SchoolDistrictTax = tax.SchoolDistrict;
                emp.CityIncomeTax = tax.CityIncomeRate;
                emp.UnemploymentCompensation = tax.UnemploymentCompensation;
                emp.EmployeePayGarnishments = tax.Garnishment;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: Taxes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", tax.Id);
            return View(tax);
        }

        // POST: Taxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxId,FederalIncomeRate,StateIncomeRate,SchoolDistrict,CityIncomeRate,UnemploymentCompensation,Garnishment,GarnishmentAmount,DeductionStatus,Id")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tax).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Employees, "Id", "FirstName", tax.Id);
            return View(tax);
        }

        // GET: Taxes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // POST: Taxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tax tax = db.Taxes.Find(id);
            db.Taxes.Remove(tax);
            db.SaveChanges();
            return RedirectToAction("Index");
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