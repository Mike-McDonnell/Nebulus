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
        MessageModel MModel;

        public MessageController()
        {
            MModel = new MessageModel();
        }

        public ActionResult Index()
        {
            var messageItems = MModel.MessageItems.ToList();
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
        public ActionResult Create(MessageItem messageItem)
        {
            messageItem.MessageItemId = Guid.NewGuid().ToString();
            MModel.MessageItems.Add(messageItem);
            MModel.SaveChanges();
            return View(messageItem);
        }
        [HttpGet]
        public ActionResult Edit(MessageItem messageItem)
        {
            if (ModelState.IsValid)
            {
                MModel.Entry(messageItem).State = System.Data.Entity.EntityState.Modified;
                MModel.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(messageItem);
        }
    }
}