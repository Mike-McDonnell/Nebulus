using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nebulus;
using Nebulus.Models;
using Nebulus.Security;

namespace Nebulus.Controllers
{
    [BroadCastAuthorizationAttribute]
    public class TemplateController : Controller
    {
        private NebulusContext db = new NebulusContext();

        // GET: Template
        public ActionResult Index()
        {
            return View(db.MessageItems.Where(item => item.Status == MessageStatus.Template).ToList());
        }

        // GET: Template/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageItem messageItem = db.MessageItems.Find(id);
            if (messageItem == null)
            {
                return HttpNotFound();
            }
            return View(messageItem);
        }

        // POST: Template/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MessageItem messageItem = db.MessageItems.Find(id);
            db.MessageItems.Remove(messageItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
