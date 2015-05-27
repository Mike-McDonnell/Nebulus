using System;
using System.Collections.Generic;
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
            return new JsonResult();
        }

    }
}