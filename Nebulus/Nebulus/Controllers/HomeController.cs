using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nebulus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpGet]
        public ActionResult Configure()
        {
            ViewBag.Title = "Configure Page";

            return View(AppConfiguration.Settings);
        }
        [HttpPost]
        public ActionResult Configure(Nebulus.Models.ConfigureModel NewConfig)
        {
            AppConfiguration.Settings = NewConfig;
            AppConfiguration.SaveSettings();

            AppConfiguration.NebulusDBContext.Entry(NewConfig.PrintServiceSettings).State = System.Data.Entity.EntityState.Modified;
            AppConfiguration.SaveSettings();

            return View(AppConfiguration.Settings);
        }
    }
}
