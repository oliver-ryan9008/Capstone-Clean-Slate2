using Capston_Clean_Slate2.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class SaleItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SaleItems
        public ActionResult Index()
        {
            return View(db.SaleItems.ToList());
        }

        // GET: SaleItems/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleItem saleItem = db.SaleItems.Find(id);
            if (saleItem == null)
            {
                return HttpNotFound();
            }
            return View(saleItem);
        }

        // GET: SaleItems/Create
        public ActionResult CreateSaleItem()
        {
            return View();
        }

        // POST: SaleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSaleItem([Bind(Include = "ItemId,ItemName,ProfitMargin")] SaleItem sale)
        {
            var currentItem = db.SaleItems.ToList().Count + 1;
            sale.ItemId = currentItem.ToString();

            if (ModelState.IsValid)
            {
                db.SaleItems.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index", "SaleItems");
            }

            return View(sale);
        }

        // GET: SaleItems/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleItem saleItem = db.SaleItems.Find(id);
            if (saleItem == null)
            {
                return HttpNotFound();
            }
            return View(saleItem);
        }

        // POST: SaleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,ItemName,ProfitMargin")] SaleItem saleItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saleItem);
        }

        // GET: SaleItems/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleItem saleItem = db.SaleItems.Find(id);
            if (saleItem == null)
            {
                return HttpNotFound();
            }
            return View(saleItem);
        }

        // POST: SaleItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SaleItem saleItem = db.SaleItems.Find(id);
            db.SaleItems.Remove(saleItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult EnterSales()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Authorize(Roles = "Manager, Admin")]
        //public ActionResult EnterSales(string id)
        //{
        //    SaleItem employeeSales = (from s in db.SaleItems where s.ItemId == id select s).FirstOrDefault();
        //    var employeesWithSales = (from e in db.Employees where e.ItemsSold != 0 select e).ToList();

        //    var itemsSold = 0;

        //    return View();
        //}

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