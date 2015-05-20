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


        // GET: Message
        public ActionResult Index()
        {
            var messageItems = MModel.MessageItems.ToList();
            return View(messageItems);
        }

        public ActionResult Create()
        {
            var MessageItem = new 
            return View();
        }
    }
}