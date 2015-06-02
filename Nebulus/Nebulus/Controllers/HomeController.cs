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

            if(AppConfiguration.Settings.PrintServiceSettings.PrintServerNames != null)
            {
                AppConfiguration.Settings.PrintServiceSettings.PrintServerNamesList = AppConfiguration.Settings.PrintServiceSettings.PrintServerNames.Split('|');
            }
            else
            {
                AppConfiguration.Settings.PrintServiceSettings = new Models.PrintServiceSettingsModel();
            }

            return View(AppConfiguration.Settings);
        }
        [HttpPost]
        public ActionResult Configure(Nebulus.Models.ConfigureModel NewConfig)
        {
            AppConfiguration.Settings = NewConfig;

            var oldPrinterList = AppConfiguration.NebulusDBContext.PrintServiceConfiguration.First();

            AppConfiguration.NebulusDBContext.PrintServiceConfiguration.Remove(oldPrinterList);

            oldPrinterList.PrintServerNames = String.Join("|", NewConfig.PrintServiceSettings.PrintServerNamesList);
            oldPrinterList.printServerServiceAccount = NewConfig.PrintServiceSettings.printServerServiceAccount;
            oldPrinterList.printServerServiceAccountPassword = NewConfig.PrintServiceSettings.printServerServiceAccountPassword;

            AppConfiguration.NebulusDBContext.PrintServiceConfiguration.Add(oldPrinterList);

            AppConfiguration.NebulusDBContext.SaveChanges();

            AppConfiguration.SaveSettings();

            return View(AppConfiguration.Settings);
        }
    }
}
