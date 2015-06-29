using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nebulus.Controllers
{
    public class TagController : Controller
    {
        [Authorize]
        public ActionResult Index() //ConfigurationPage
        {
            return View();
        }

        public JsonResult GetADGroups(string term)
        {
            if (AppConfiguration.Settings.GroupTAGsDateSourceType == Models.TAGsDateSourceType.ActiveDirectory)
            {
                try
                {
                    PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();

                    GroupPrincipal insPrincipal = new GroupPrincipal(AppConfiguration.ADPrincipalContext);

                    if(AppConfiguration.Settings.GroupTAGsDateSource != string.Empty)
                    {
                        insPrincipal = new GroupPrincipal(new PrincipalContext(ContextType.Domain, AppConfiguration.ADPrincipalContext.ConnectedServer, AppConfiguration.Settings.GroupTAGsDateSource));
                    }

                    insPrincipal.SamAccountName = term + "*";

                    insPrincipalSearcher.QueryFilter = insPrincipal;

                    var results = insPrincipalSearcher.FindAll().Take(10).Select(p => p.SamAccountName);

                    return Json(results, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error("Error: AD Group Query GetADGroups", ex);
                }
            }
            if(AppConfiguration.Settings.GroupTAGsDateSourceType == Models.TAGsDateSourceType.DataBase)
            {

            }
            if(AppConfiguration.Settings.GroupTAGsDateSourceType == Models.TAGsDateSourceType.LocalFileSource)
            {

            }

            return new JsonResult();
        }

        public JsonResult GetADUsers(string term)
        {
            if (AppConfiguration.Settings.UserTAGsDateSourceType == Models.TAGsDateSourceType.ActiveDirectory)
            {
                try
                {
                    PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();

                    UserPrincipal insPrincipal = new UserPrincipal(AppConfiguration.ADPrincipalContext);

                    if (AppConfiguration.Settings.UserTAGsDateSource != string.Empty)
                    {
                        insPrincipal = new UserPrincipal(new PrincipalContext(ContextType.Domain, AppConfiguration.ADPrincipalContext.ConnectedServer, AppConfiguration.Settings.UserTAGsDateSource));
                    }

                    insPrincipal.SamAccountName = term + "*";

                    insPrincipalSearcher.QueryFilter = insPrincipal;

                    var results = insPrincipalSearcher.FindAll().Take(10).Select(p => p.SamAccountName);

                    return Json(results, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error("Error: AD Group Query GetADUsers", ex);
                }
            }
            if (AppConfiguration.Settings.UserTAGsDateSourceType == Models.TAGsDateSourceType.DataBase)
            {

            }
            if (AppConfiguration.Settings.UserTAGsDateSourceType == Models.TAGsDateSourceType.LocalFileSource)
            {

            }

            return new JsonResult();
        }

        public JsonResult GetADComputers(string term)
        {
            if (AppConfiguration.Settings.ComputerTAGsDateSourceType == Models.TAGsDateSourceType.ActiveDirectory)
            {
                try
                {
                    PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();

                    ComputerPrincipal insPrincipal = new ComputerPrincipal(AppConfiguration.ADPrincipalContext);

                    if (AppConfiguration.Settings.ComputerTAGsDateSource != string.Empty)
                    {
                        insPrincipal = new ComputerPrincipal(new PrincipalContext(ContextType.Domain, AppConfiguration.ADPrincipalContext.ConnectedServer, AppConfiguration.Settings.ComputerTAGsDateSource));
                    }
                    
                    insPrincipal.Name = term + "*";

                    insPrincipalSearcher.QueryFilter = insPrincipal;

                    var results = insPrincipalSearcher.FindAll().Take(10).Select(p => p.Name.ToUpper());

                    return Json(results, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error("Error: AD Group Query GetADComputer", ex);
                }
            }
            if (AppConfiguration.Settings.ComputerTAGsDateSourceType == Models.TAGsDateSourceType.DataBase)
            {

            }
            if (AppConfiguration.Settings.ComputerTAGsDateSourceType == Models.TAGsDateSourceType.LocalFileSource)
            {

            }

            return new JsonResult();
        }

        public JsonResult GetSubNetList(string term)
        {

            return new JsonResult();
        }
    }
}