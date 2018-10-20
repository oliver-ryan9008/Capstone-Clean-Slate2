using Capston_Clean_Slate2.Models;
using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class SchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.InitialDate = DateTime.Now.Date;

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ContentResult Data()
        {
            try
            {
                var details = db.Events.ToList();

                return new SchedulerAjaxData(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);

            try
            {
                var changedEvent = (CalendarEvent)DHXEventsHelper.Bind(typeof(CalendarEvent), actionValues);
                //var employee = (from e in db.Employees where e.FirstName)

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        Event EV = new Event();
                        EV.id = changedEvent.id;
                        EV.start_date = changedEvent.start_date;
                        EV.end_date = changedEvent.end_date;
                        EV.text = changedEvent.text;
                        db.Events.Add(EV);
                        db.SaveChanges();

                        break;

                    case DataActionTypes.Delete:
                        var details = db.Events.Where(x => x.id == id).FirstOrDefault();
                        db.Events.Remove(details);
                        db.SaveChanges();

                        break;

                    default:// "update"
                        var data = db.Events.Where(x => x.id == id).FirstOrDefault();
                        data.start_date = changedEvent.start_date;
                        data.end_date = changedEvent.end_date;
                        data.text = changedEvent.text;
                        db.SaveChanges();

                        break;
                }
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (ContentResult)new AjaxSaveResponse(action);
        }

        public ActionResult ViewSchedule(string id)
        {
            var employee = (from e in db.Employees where e.Id == id select e).First();
            var employeeSchedules = (from s in db.Schedules where s.Id == employee.Id select s).ToList();

            return View(employeeSchedules);
        }

        public JsonResult AddEmployeeWorkDays(Schedule schedule)
        {
            var employee = (from e in db.Employees where e.Id == schedule.Id select e).First();
            var employeeSchedules = (from s in db.Schedules where s.Id == employee.Id select s).ToList();

            return Json(employeeSchedules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEmployeeSchedule(string id)
        {
            Schedule schedule = new Schedule();
            var employee = (from e in db.Employees where e.Id == id select e).First();
            schedule.Id = new Guid().ToString();
            schedule.Employee = employee;

            return View(schedule);
        }

        [HttpPost]
        [ActionName("AddEmployeeSchedule")]
        public ActionResult AddEmployeeScheduleConfirmed([Bind(Include = "Id, ScheduleId, StartTime, EndTime, Text")] Schedule schedule)
        {
            schedule.ScheduleId = schedule.Id;
            schedule.Employee = (from e in db.Employees where e.Id == schedule.Id select e).First();
            var today = DateTime.Now;
            if (schedule.StartTime >= today)
            {
                //ViewSchedule(schedule);
                db.Schedules.Add(schedule);
                db.SaveChanges();
            }

            return RedirectToAction("DisplayAllEmployees", "Admins");
        }

        //private Employee[] employeeData = {
        //                 new Employee {Name:"Suraj Sahoo",EmpCode:"EMP042"},
        //                 new Employee {Name:"Suraj Sharma",EmpCode:"EMP044"},
        //                 new Employee {Name:"Suraj Ray",EmpCode:"EMP041"}
        //            };
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public JsonResult GetEmployeeDataJson(string empCode)
        //{
        //    var employee = employeeData.Where(emp =>
        //                              emp.EmpCode == empCode).SingleOrDefault();
        //    return Json(employee, JsonRequestBehaviour.AllowGet);
        //}
    }
}