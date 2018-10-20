using Capston_Clean_Slate2.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class RetirementAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RetirementAccounts
        public ActionResult Index()
        {
            return View(db.RetirementAccounts.ToList());
        }

        // GET: RetirementAccounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RetirementAccount retirementAccount = db.RetirementAccounts.Find(id);
            if (retirementAccount == null)
            {
                return HttpNotFound();
            }
            return View(retirementAccount);
        }

        //// GET: RetirementAccounts/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: RetirementAccounts/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "RetirementAccountNumber,TotalRetirementBalance,EmployeeContribution,OwnerContribution,CatchUpAge")] RetirementAccount retirementAccount)
        //{
        //    var retirementNumber = retirementAccount.RetirementAccountNumber;
        //    if (ModelState.IsValid)
        //    {
        //        Employee employee = (from e in db.Employees where (e.FirstName + e.LastName) == retirementNumber select e).First();
        //        employee.RetirementAccountNumber = retirementNumber;
        //        db.RetirementAccounts.Add(retirementAccount);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(retirementAccount);
        //}

        // GET: RetirementAccounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RetirementAccount retirementAccount = db.RetirementAccounts.Find(id);
            if (retirementAccount == null)
            {
                return HttpNotFound();
            }
            return View(retirementAccount);
        }

        // POST: RetirementAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RetirementAccountNumber,TotalRetirementBalance,EmployeeContribution,OwnerContribution,CatchUpAge")] RetirementAccount retirementAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(retirementAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(retirementAccount);
        }

        // GET: RetirementAccounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RetirementAccount retirementAccount = db.RetirementAccounts.Find(id);
            if (retirementAccount == null)
            {
                return HttpNotFound();
            }
            return View(retirementAccount);
        }

        // POST: RetirementAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RetirementAccount retirementAccount = db.RetirementAccounts.Find(id);
            db.RetirementAccounts.Remove(retirementAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChooseEmployeeRetirementContribution()
        {
            return View();
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