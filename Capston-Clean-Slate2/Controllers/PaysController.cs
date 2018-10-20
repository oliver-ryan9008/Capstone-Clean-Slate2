using Capston_Clean_Slate2.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class PaysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pays
        //public ActionResult Index()
        //{
        //    return View(db.Pays.ToList());
        //}

        // GET: Pays/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Pay pay = db.Pays.Find(id);
        //    if (pay == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pay);
        //}

        // GET: Pays/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Pays/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,SalaryRate,HourlyRate,HoursWorked,OvertimeRate,SpecialPay")] Pay pay)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Pays.Add(pay);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(pay);
        //}

        // GET: Pays/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pay pay = db.Pays.Find(id);
            if (pay == null)
            {
                return HttpNotFound();
            }
            return View(pay);
        }

        // POST: Pays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SalaryRate,HourlyRate,HoursWorked,OvertimeRate,SpecialPay")] Pay pay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pay);
        }

        // GET: Pays/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pay pay = db.Pays.Find(id);
            if (pay == null)
            {
                return HttpNotFound();
            }
            return View(pay);
        }

        // POST: Pays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Pay pay = db.Pays.Find(id);
            db.Pays.Remove(pay);
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