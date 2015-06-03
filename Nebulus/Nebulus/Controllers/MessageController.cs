using Microsoft.ServiceBus.Messaging;
using Nebulus.Models;
using System;
using System.Collections.Generic;
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

            return View(messageItem);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(MessageItem messageItem, List<string> tag)
        {
            messageItem.MessageItemId = Guid.NewGuid().ToString();
            messageItem.TargetGroup = string.Join("|", tag);
            Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Add(messageItem);
            Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(Nebulus.AppConfiguration.NebulusDBContext.MessageItems.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(MessageItem messageItem)
        {
            Nebulus.AppConfiguration.NebulusDBContext.Entry(messageItem).State = System.Data.Entity.EntityState.Modified;
            Nebulus.AppConfiguration.NebulusDBContext.SaveChanges();

            return RedirectToAction("Index");
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