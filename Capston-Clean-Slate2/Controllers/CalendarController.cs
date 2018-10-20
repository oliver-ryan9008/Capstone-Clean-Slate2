using Capston_Clean_Slate2.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Capston_Clean_Slate2.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            //Being initialized in that way, scheduler will use CalendarController.Data as a the datasource and CalendarController.Save to process changes
            var scheduler = new DHXScheduler(this);

            /*
             * It's possible to use different actions of the current controller
             *      var scheduler = new DHXScheduler(this);
             *      scheduler.DataAction = "ActionName1";
             *      scheduler.SaveAction = "ActionName2";
             *
             * Or to specify full paths
             *      var scheduler = new DHXScheduler();
             *      scheduler.DataAction = Url.Action("Data", "Calendar");
             *      scheduler.SaveAction = Url.Action("Save", "Calendar");
             */

            /*
             * The default codebase folder is ~/Scripts/dhtmlxScheduler. It can be overriden:
             *      scheduler.Codebase = Url.Content("~/customCodebaseFolder");
             */

            scheduler.InitialDate = new DateTime(2012, 09, 03);

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData(
                    new List<CalendarEvent>{
                        new CalendarEvent{
                            id = 1,
                            text = "Sample Event",
                            start_date = new DateTime(2012, 09, 03, 6, 00, 00),
                            end_date = new DateTime(2012, 09, 03, 8, 00, 00)
                        },
                        new CalendarEvent{
                            id = 2,
                            text = "New Event",
                            start_date = new DateTime(2012, 09, 05, 9, 00, 00),
                            end_date = new DateTime(2012, 09, 05, 12, 00, 00)
                        },
                        new CalendarEvent{
                            id = 3,
                            text = "Multiday Event",
                            start_date = new DateTime(2012, 09, 03, 10, 00, 00),
                            end_date = new DateTime(2012, 09, 10, 12, 00, 00)
                        }
                    }
                );
            return (ContentResult)data;
        }

        //public ContentResult Save(int? id, FormCollection actionValues)
        //{
        //    var action = new DataAction(actionValues);

        //    try
        //    {
        //        var changedEvent = (CalendarEvent)DHXEventsHelper.Bind(typeof(CalendarEvent), actionValues);

        //        switch (action.Type)
        //        {
        //            case DataActionTypes.Insert:
        //                Event EV = new Event();
        //                EV.id = changedEvent.id;
        //                EV.start_date = changedEvent.start_date;
        //                EV.end_date = changedEvent.end_date;
        //                EV.text = changedEvent.text;
        //                cX.Events.Add(EV);
        //                cX.SaveChanges();

        //                break;
        //        }
        //    }
        //    catch
        //    {
        //        action.Type = DataActionTypes.Error;
        //    }
        //    return (ContentResult)new AjaxSaveResponse(action);
        //}
    }
}