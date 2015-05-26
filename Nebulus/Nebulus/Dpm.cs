using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Month;
using Nebulus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nebulus
{
    class Dpm : DayPilotMonth
    {
        public MessageModel MModel;

        protected override void OnInit(InitArgs e)
        {
            DateTimeOffset ExpireDate = DateTimeOffset.Now.AddDays(45);
            Events = from ev in MModel.MessageItems where ev.Expiration <= ExpireDate select ev;

            DataIdField = "MessageItemId";
            DataTextField = "MessageTitle";
            DataStartField = "ScheduleStart";
            DataEndField = "Expiration";

            Update();

        }

        protected override void OnEventClick(EventClickArgs e)
        {
            base.OnEventClick(e);

            Redirect("~/Message/Edit/" + e.Id);
        }

        protected override void OnCommand(CommandArgs e)
        {
            switch (e.Command)
            {
                case "navigate":
                    StartDate = (DateTime)e.Data["start"];
                    DateTimeOffset ExpireDate = StartDate.AddDays(45);
                    Events = from ev in MModel.MessageItems where ev.Expiration <= ExpireDate && ev.Expiration > StartDate select ev;

                    DataIdField = "MessageItemId";
                    DataTextField = "MessageTitle";
                    DataStartField = "ScheduleStart";
                    DataEndField = "Expiration";
                    Update(CallBackUpdateType.Full);
                    break;

                case "refresh":
                    Update();
                    break;

                case "previous":
                    StartDate = StartDate.AddDays(-7);
                    Update(CallBackUpdateType.Full);
                    break;

                case "next":
                    StartDate = StartDate.AddDays(7);
                    Update(CallBackUpdateType.Full);
                    break;

                case "today":
                    StartDate = DateTime.Today;
                    Update(CallBackUpdateType.Full);
                    break;

            }
        }
    }
}