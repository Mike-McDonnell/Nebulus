using Nebulus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nebulus
{
    //class Dpm : DayPilotMonth
    //{
    //    public NebulusContext MModel;

    //    protected override void OnInit(InitArgs e)
    //    {
    //        DateTimeOffset ExpireDate = DateTimeOffset.Now.AddDays(45);
    //        DateTimeOffset StartDate = DateTimeOffset.Now.Subtract(new TimeSpan(31, 0, 0, 0));

    //        var basicEvents = from ev in MModel.MessageItems where ev.Expiration <= ExpireDate && ev.ScheduleStart > StartDate && ev.ScheduleInterval == ScheduleIntervalType.Never select ev;
    //        var repetEvents = from ev in MModel.MessageItems where ev.Expiration <= ExpireDate && ev.ScheduleInterval != ScheduleIntervalType.Never select ev;

    //        var eItems = new List<MessageItem>();

    //        eItems.AddRange(basicEvents);

    //        foreach (var eitem in repetEvents)
    //        {
    //            if (eitem.ScheduleInterval == ScheduleIntervalType.Daily)
    //            {
    //                if(DateTimeOffset.Now.Month > eitem.ScheduleStart.Month)
    //                {
    //                    eitem.ScheduleStart = new DateTimeOffset(eitem.ScheduleStart.Year, DateTimeOffset.Now.Month, 1, eitem.ScheduleStart.Hour, eitem.ScheduleStart.Minute, eitem.ScheduleStart.Second, eitem.ScheduleStart.Offset); 
    //                }
    //                if(!eitem.MessageTitle.Contains("R-day"))
    //                    eitem.MessageTitle += " R-Day";
    //            }

    //            if(eitem.ScheduleInterval == ScheduleIntervalType.Weekly)
    //            {
    //                if(DateTimeOffset.Now.DayOfYear >= (eitem.ScheduleStart.DayOfYear + 7))
    //                {
    //                    eitem.ScheduleStart = DateTimeOffset.Now.StartOfWeek(eitem.ScheduleStart.DayOfWeek);
    //                }
    //                if(!eitem.MessageTitle.Contains("R-Week"))
    //                    eitem.MessageTitle += " R-Week";
    //            }

    //            if(eitem.ScheduleInterval == ScheduleIntervalType.Monthly)
    //            {
    //                if(DateTimeOffset.Now.Month > eitem.ScheduleStart.Month)
    //                {
    //                    eitem.ScheduleStart = new DateTimeOffset(eitem.ScheduleStart.Year, DateTimeOffset.Now.Month, eitem.ScheduleStart.Day, eitem.ScheduleStart.Hour, eitem.ScheduleStart.Minute, eitem.ScheduleStart.Second, eitem.ScheduleStart.Offset);
    //                }
    //                if(!eitem.MessageTitle.Contains("R-Month"))
    //                    eitem.MessageTitle += " R-Month";
    //            }

    //            if(eitem.ScheduleInterval == ScheduleIntervalType.Hourly)
    //            {
    //                if(!eitem.MessageTitle.Contains("R-Hour"))
    //                    eitem.MessageTitle += " R-Hour";
    //            }

    //            eItems.Add(eitem);
    //        }

    //        Events = eItems;

    //        DataIdField = "MessageItemId";
    //        DataTextField = "MessageTitle";
    //        DataStartField = "ScheduleStart";
    //        DataEndField = "Expiration";

    //        Update();

    //    }

    //    protected override void OnEventClick(EventClickArgs e)
    //    {
    //        base.OnEventClick(e);

    //        Redirect("~/Message/Edit/" + e.Id);
    //    }

    //    protected override void OnCommand(CommandArgs e)
    //    {
    //        switch (e.Command)
    //        {
    //            case "navigate":
    //                StartDate = (DateTime)e.Data["start"];
    //                DateTimeOffset ExpireDate = StartDate.AddDays(45);
    //                Events = from ev in MModel.MessageItems where ev.Expiration <= ExpireDate && ev.Expiration > StartDate select ev;

    //                DataIdField = "MessageItemId";
    //                DataTextField = "MessageTitle";
    //                DataStartField = "ScheduleStart";
    //                DataEndField = "Expiration";
    //                Update(CallBackUpdateType.Full);
    //                break;

    //            case "refresh":
    //                Update();
    //                break;

    //            case "previous":
    //                StartDate = StartDate.AddDays(-7);
    //                Update(CallBackUpdateType.Full);
    //                break;

    //            case "next":
    //                StartDate = StartDate.AddDays(7);
    //                Update(CallBackUpdateType.Full);
    //                break;

    //            case "today":
    //                StartDate = DateTime.Today;
    //                Update(CallBackUpdateType.Full);
    //                break;

    //        }
    //    }
    //}

    //public static class DateTimeOffsetExtensions
    //{
    //    public static DateTimeOffset StartOfWeek(this DateTimeOffset dt, DayOfWeek startOfWeek)
    //    {
    //        int diff = dt.DayOfWeek - startOfWeek;
    //        if (diff < 0)
    //        {
    //            diff += 7;
    //        }

    //        return dt.AddDays(-1 * diff).Date;
    //    }
    //}
}