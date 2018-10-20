using Capston_Clean_Slate2.Models;
using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using System;
using System.Data.Entity;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult CalendarView()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Terrace;

            scheduler.Config.first_hour = 5;
            scheduler.Config.last_hour = 20;

            scheduler.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);

            try
            {
                var changedEvent = DHXEventsHelper.Bind<Appointment>(actionValues);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        db.Appointments.Add(changedEvent);
                        break;

                    case DataActionTypes.Delete:
                        db.Entry(changedEvent).State = EntityState.Deleted;
                        break;

                    default:// "update"
                        db.Entry(changedEvent).State = EntityState.Modified;
                        break;
                }
                db.SaveChanges();
                action.TargetId = changedEvent.Id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }

        //// GET: Appointments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Appointment appointment = db.Appointments.Find(id);
        //    if (appointment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(appointment);
        //}

        //// GET: Appointments/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Appointments/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Description,StartDate,EndDate")] Appointment appointment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Appointments.Add(appointment);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(appointment);
        //}

        //// GET: Appointments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Appointment appointment = db.Appointments.Find(id);
        //    if (appointment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(appointment);
        //}

        //// POST: Appointments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Description,StartDate,EndDate")] Appointment appointment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(appointment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(appointment);
        //}

        //// GET: Appointments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Appointment appointment = db.Appointments.Find(id);
        //    if (appointment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(appointment);
        //}

        //// POST: Appointments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Appointment appointment = db.Appointments.Find(id);
        //    db.Appointments.Remove(appointment);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
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