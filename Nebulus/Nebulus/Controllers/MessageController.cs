using Microsoft.ServiceBus.Messaging;
using Nebulus.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nebulus.Security;

namespace Nebulus.Controllers
{
    [BroadCastAuthorizationAttribute]
    public class MessageController : Controller
    {
        public MessageController()
        {
  
        }

        public ActionResult Index()
        {
            var messageItems = Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Where(item => item.Expiration > DateTimeOffset.Now && item.Status != MessageStatus.Template).ToList();
            return View(messageItems.OrderBy(item => item.ScheduleStart));
        }
        [HttpGet]
        public ActionResult Create(string id)
        {
            var messageItem = new MessageItem();

            messageItem.Status = MessageStatus.New;
            if(id != null)
            {
                CloneFromTemplate(id, messageItem);
            }

            messageItem.ScheduleStart = DateTimeOffset.Now;
            messageItem.Expiration = DateTimeOffset.Now.AddHours(2);
            messageItem.duration = 2;

            return View(messageItem);
        }

        private static void CloneFromTemplate(string id, MessageItem messageItem)
        {
            var templateMessage = Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(id);
            messageItem.MessageBody = templateMessage.MessageBody;
            messageItem.MessageHeight = templateMessage.MessageHeight;
            messageItem.MessageLeft = templateMessage.MessageLeft;
            messageItem.MessageLocation = templateMessage.MessageLocation;
            messageItem.MessagePriority = templateMessage.MessagePriority;
            messageItem.MessageTop = templateMessage.MessageTop;
            messageItem.MessageType = templateMessage.MessageType;
            messageItem.MessageWidth = templateMessage.MessageWidth;
            messageItem.ScheduleInterval = templateMessage.ScheduleInterval;
            messageItem.TargetGroup = templateMessage.TargetGroup;
            messageItem.duration = templateMessage.duration;

            messageItem.Status = MessageStatus.Clone;
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> Create(MessageItem messageItem, string submit, string[] tags)
        {
            if (ModelState.IsValid)
            {
                if (messageItem.MessageBody.Contains("&shy;"))
                {
                    messageItem.MessageBody = messageItem.MessageBody.Replace("&shy;", string.Empty);
                }

                messageItem.Creator = User.Identity.Name;
                messageItem.MessageItemId = Guid.NewGuid().ToString();
                messageItem.TargetGroup = tags != null ? string.Join("|", tags) : string.Empty;

                if (submit == "Template")
                {
                    messageItem.Status = MessageStatus.Template;
                }
                else if (submit == "Create")
                {
                    if (messageItem.ScheduleStart < DateTimeOffset.Now.AddMinutes(1))
                    {
                        if (await SendMessage(messageItem))
                        {
                            messageItem.SentTime = DateTimeOffset.Now;
                            messageItem.Status = MessageStatus.Sent;
                        }
                    }
                }

                Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Add(messageItem);

                try
                {
                    Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }

                return RedirectToAction("Index");
            }
            return View(messageItem);
        }

        [HttpGet]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> Send(string id)
        {
            if(id == null)
            {
                return View("Error");
            }

            var messageItem = Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(id);

            if(messageItem == null)
            {
                return View("Error");
            }

            if(await SendMessage(messageItem))
            {
                messageItem.SentTime = DateTimeOffset.Now;
                messageItem.Status = MessageStatus.Sent;
                Nebulus.AppConfiguration.NebulusDBContext.Entry(messageItem).State = System.Data.Entity.EntityState.Modified;
                Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(MessageItem messageItem, string[] tags, string[] tagsGroup)
        {
            if (ModelState.IsValid)
            {
                messageItem.TargetGroup = tags != null ? string.Join("|", tags) : string.Empty;

                ((IObjectContextAdapter)Nebulus.AppConfiguration.NebulusDBContext).ObjectContext.Detach(Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(messageItem.MessageItemId));
                Nebulus.AppConfiguration.NebulusDBContext.Entry(messageItem).State = System.Data.Entity.EntityState.Modified;
                Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(messageItem);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(id));
        }

        [HttpPost]
        public ActionResult Delete(MessageItem messageItem)
        {
            Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Remove(Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(messageItem.MessageItemId));
            Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult CalandarEventsData(string startDate)
        {
            DateTimeOffset StartDate = DateTimeOffset.Parse(startDate).Subtract(new TimeSpan(7,0,0,0));
            DateTimeOffset EndDate = DateTimeOffset.Parse(startDate).AddMonths(1).AddDays(7);

            var mEvents = from ev in Nebulus.AppConfiguration.NebulusDBContext.MessageItems where ((ev.ScheduleInterval != ScheduleIntervalType.Never && ev.Expiration > DateTimeOffset.Now) || ev.Expiration > DateTimeOffset.Now && ev.ScheduleStart >= StartDate && ev.ScheduleStart <= EndDate) && ev.Status != MessageStatus.Template select ev;

            var fmEvents = FormatRecurringEvents(mEvents, DateTimeOffset.Parse(startDate));

            var rows = fmEvents; 
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        private List<Object> FormatRecurringEvents(IQueryable<MessageItem> mEvents, DateTimeOffset startDate)
        {
            List<object> fmEvents = new List<object>();

            foreach (var eitem in mEvents)
            {
                var backgroundColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.CornflowerBlue);
                var StartTime = eitem.ScheduleStart;

                if (eitem.ScheduleInterval == ScheduleIntervalType.Daily)
                {

                    if (startDate.Month > eitem.ScheduleStart.Month)
                    {
                        StartTime = new DateTimeOffset(eitem.ScheduleStart.Year, startDate.Month, startDate.Day, eitem.ScheduleStart.Hour, eitem.ScheduleStart.Minute, eitem.ScheduleStart.Second, eitem.ScheduleStart.Offset);
                    }

                    backgroundColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Purple);

                    for (var day = StartTime.DayOfYear; day <= new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)).DayOfYear; day++)
                    {
                        if (eitem.Expiration.DayOfYear < day)
                        {
                            break;
                        }

                        fmEvents.Add(new
                        {
                            id = eitem.MessageItemId,
                            title = eitem.MessageTitle,
                            start = StartTime.AddDays(day - StartTime.DayOfYear),
                            //start = new DateTimeOffset(StartTime.Year, StartTime.Month, day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Offset),
                            end = eitem.duration,
                            color = backgroundColor,
                            allDay = false
                        });
                    }
                }

                if (eitem.ScheduleInterval == ScheduleIntervalType.Weekly)
                {
                    if (startDate.DayOfYear >= (eitem.ScheduleStart.DayOfYear + 7))
                    {
                        StartTime = startDate.StartOfWeek(eitem.ScheduleStart.DayOfWeek);
                    }
                    backgroundColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Orange);

                    for (var day = StartTime.DayOfYear; day <= new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)).DayOfYear; day += 7)
                    {
                        if(eitem.Expiration.DayOfYear < day)
                        {
                            break;
                        }

                        fmEvents.Add(new
                        {
                            id = eitem.MessageItemId,
                            title = eitem.MessageTitle,
                            start = StartTime.AddDays(day - StartTime.DayOfYear),
                            //start = new DateTimeOffset(StartTime.Year, StartTime.Month, new DateTime().AddDays(day - 1).Day, StartTime.Hour, StartTime.Minute, StartTime.Second, StartTime.Offset),
                            end = eitem.duration,
                            color = backgroundColor,
                            allDay = false
                        });
                    }

                }

                if (eitem.ScheduleInterval == ScheduleIntervalType.Monthly)
                {
                    backgroundColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Maroon);

                    if (startDate.Month > eitem.ScheduleStart.Month)
                    {
                        StartTime = new DateTimeOffset(startDate.Year, startDate.Month, eitem.ScheduleStart.Day, eitem.ScheduleStart.Hour, eitem.ScheduleStart.Minute, eitem.ScheduleStart.Second, eitem.ScheduleStart.Offset);
                    }

                    if (eitem.Expiration < StartTime)
                    {
                        continue;
                    }

                    fmEvents.Add(new
                    {
                        id = eitem.MessageItemId,
                        title = eitem.MessageTitle,
                        start = StartTime,
                        end = eitem.duration,
                        color = backgroundColor,
                        allDay = false
                    });
                }

                if (eitem.ScheduleInterval == ScheduleIntervalType.Hourly)
                {
                    backgroundColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Lime);

                    for (var day = eitem.ScheduleStart.Day; day <= DateTime.DaysInMonth(startDate.Year, startDate.Month); day++)
                    {
                        fmEvents.Add(new
                        {
                            id = eitem.MessageItemId,
                            title = eitem.MessageTitle,
                            start = new DateTimeOffset(eitem.ScheduleStart.Year, eitem.ScheduleStart.Month, day, eitem.ScheduleStart.Hour, eitem.ScheduleStart.Minute, eitem.ScheduleStart.Second, eitem.ScheduleStart.Offset),
                            end = eitem.duration,
                            color = backgroundColor,
                            allDay = false
                        });
                    }
                }
                
                if(eitem.ScheduleInterval == ScheduleIntervalType.Never)
                {
                    fmEvents.Add(new
                    {
                        id = eitem.MessageItemId,
                        title = eitem.MessageTitle,
                        start = eitem.ScheduleStart,
                        end = eitem.duration,
                        color = backgroundColor,
                        allDay = false
                    });
                }
             
            }

            return fmEvents;
        }

        private static async System.Threading.Tasks.Task<bool> SendMessage(MessageItem messageItem)
        {
            bool sent = false;
            try
            {
                Microsoft.ServiceBus.Notifications.Notification notification = new Microsoft.ServiceBus.Notifications.WindowsNotification(Newtonsoft.Json.Linq.JObject.FromObject(messageItem).ToString());
                notification.ContentType = "application/octet-stream";
                notification.Headers.Add("X-WNS-Type", "wns/raw");

                if (messageItem.TargetGroup != null && messageItem.TargetGroup != string.Empty)
                {
                    messageItem.TargetGroup.Replace(",", " || ");
                }

                await NSBQ.NNHClient.SendNotificationAsync(notification);

                AppLogging.Instance.Info("Message sent to NotificationHub, " + messageItem.MessageItemId);
                sent = true;
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to NotificationHub ", ex);
            }


            try
            {
                BrokeredMessage sendMessage = new BrokeredMessage(messageItem);
                if (messageItem.TargetGroup != null && messageItem.TargetGroup != string.Empty)
                {
                    sendMessage.Properties.Add("Tags", messageItem.TargetGroup);
                }
                else
                {
                    sendMessage.Properties.Add("Tags", "BROADCAST");
                }

                NSBQ.NSBQClient.Send(sendMessage);
                AppLogging.Instance.Info("Message sent");
                sent = true;
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
            }

            return sent;
        }
    }


    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset StartOfWeek(this DateTimeOffset dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}