using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nebulus.Security;

namespace Nebulus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var test = Request.LogonUserIdentity;

            ViewBag.Title = "Home Page";

            return View();
        }
        [ADAdminAuthorization]
        [HttpGet]
        public ActionResult Configure()
        {
            ViewBag.Title = "Configure Page";

            if (AppConfiguration.Settings.PrintServiceSettings.PrintServerNames != null)
            {
                AppConfiguration.Settings.PrintServiceSettings.PrintServerNamesList = AppConfiguration.Settings.PrintServiceSettings.PrintServerNames.Split('|');
            }
            else
            {
                AppConfiguration.Settings.PrintServiceSettings.PrintServerNamesList = new List<string>(); 
            }

            return View(AppConfiguration.Settings);
        }
        [ADAdminAuthorization]
        [HttpPost]
        public ActionResult Configure(Nebulus.Models.ConfigureModel NewConfig)
        {
            AppConfiguration.Settings = NewConfig;

            if (NewConfig.FeatureSet > 2)
            {
                var printerconfig = AppConfiguration.NebulusDBContext.PrintServiceConfiguration.FirstOrDefault();

                //AppConfiguration.NebulusDBContext.PrintServiceConfiguration.Remove(printerconfig);

                printerconfig.PrintServerNames = String.Join("|", NewConfig.PrintServiceSettings.PrintServerNamesList);
                printerconfig.printServerServiceAccount = NewConfig.PrintServiceSettings.printServerServiceAccount;
                printerconfig.printServerServiceAccountPassword = NewConfig.PrintServiceSettings.printServerServiceAccountPassword;

                //AppConfiguration.NebulusDBContext.PrintServiceConfiguration.Add(printerconfig);

                AppConfiguration.NebulusDBContext.Entry(printerconfig).State = System.Data.Entity.EntityState.Modified;
            }

            AppConfiguration.NebulusDBContext.SaveChanges();

            AppConfiguration.SaveSettings();

            return View(AppConfiguration.Settings);
        }
    }
}
