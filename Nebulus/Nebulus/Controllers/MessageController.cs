using Microsoft.ServiceBus.Messaging;
using Nebulus.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nebulus.Controllers
{
    public class MessageController : Controller
    {
        private Dpm dpm;

        public MessageController()
        {
            dpm = new Dpm() { MModel = Nebulus.AppConfiguration.NebulusDBContext };
        }

        public ActionResult Index()
        {
            DateTimeOffset ExpireDate = DateTimeOffset.Now.AddDays(45);
            var messageItems = Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Where(item => item.Expiration <= ExpireDate).ToList();
            return View(messageItems);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var messageItem = new MessageItem();
            messageItem.ScheduleStart = DateTimeOffset.Now;
            messageItem.Expiration = DateTimeOffset.Now.AddHours(2);
            messageItem.duration = 2;

            return View(messageItem);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(MessageItem messageItem, string[] tags)
        {
            if (ModelState.IsValid)
            {

                messageItem.MessageItemId = Guid.NewGuid().ToString();
                messageItem.TargetGroup = tags != null ? string.Join("|", tags) : string.Empty;
                Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Add(messageItem);
                Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();

                try
                {
                    BrokeredMessage sendMessage = new BrokeredMessage(messageItem);
                    if (messageItem.TargetGroup != null && messageItem.TargetGroup != string.Empty)
                    {
                        sendMessage.Properties.Add("Tags", messageItem.TargetGroup);
                    }
                
                    NSBQ.NSBQClient.Send(sendMessage);
                    AppLogging.Instance.Info("Message sent");
                }
                catch(Exception ex) {
                    AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
                }
                return RedirectToAction("Index");
            }
            return View(messageItem);
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
        public ActionResult Backend()
        {
            return dpm.CallBack(this);
        }
    }
}