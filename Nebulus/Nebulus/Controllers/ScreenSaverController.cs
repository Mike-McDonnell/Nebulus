using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nebulus;
using Nebulus.Models;
using Microsoft.AspNet.Identity;

namespace Nebulus.Controllers
{
    public class ScreenSaverController : Controller
    {
        private NebulusContext db = new NebulusContext();

        // GET: ScreenSaver
        public async Task<ActionResult> Index()
        {
            return View(await db.ScreenSaverItems.ToListAsync());
        }

        // GET: ScreenSaver/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScreenSaver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Created,Modified,Name,Slides,CreatorId,Active,Presenter")] ScreenSaverItem screenSaverItem)
        {
            if (ModelState.IsValid)
            {
                screenSaverItem.Created = DateTimeOffset.Now;
                screenSaverItem.Modified = screenSaverItem.Created;
                screenSaverItem.CreatorId = User.Identity.GetUserId();
               
                db.ScreenSaverItems.Add(screenSaverItem);
                await db.SaveChangesAsync();

                SavePowerPointSlidesToServer(screenSaverItem);

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(screenSaverItem);
        }

        // GET: ScreenSaver/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScreenSaverItem screenSaverItem = await db.ScreenSaverItems.FindAsync(id);
            if (screenSaverItem == null)
            {
                return HttpNotFound();
            }
            return View(screenSaverItem);
        }

        // POST: ScreenSaver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Active,Slides,Presenter")] ScreenSaverItem screenSaverItem)
        {
            screenSaverItem.Modified = DateTimeOffset.Now;

            if (ModelState.IsValid)
            {
                SavePowerPointSlidesToServer(screenSaverItem);

                db.Entry(screenSaverItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ModelState["Slides"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;

                    db.Entry(screenSaverItem).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch { }
                finally { db.Configuration.ValidateOnSaveEnabled = true; }
            }

            return View(screenSaverItem);
        }

        // GET: ScreenSaver/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScreenSaverItem screenSaverItem = await db.ScreenSaverItems.FindAsync(id);
            if (screenSaverItem == null)
            {
                return HttpNotFound();
            }
            return View(screenSaverItem);
        }

        // POST: ScreenSaver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ScreenSaverItem screenSaverItem = await db.ScreenSaverItems.FindAsync(id);
            db.ScreenSaverItems.Remove(screenSaverItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<FileResult> GetSlide(int id)
        {
            ScreenSaverItem screenSaverItem = await db.ScreenSaverItems.FindAsync(id);

            using (var slidefile = new System.IO.FileStream(screenSaverItem.SlidesPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                byte[] data = new byte[slidefile.Length];

                await slidefile.ReadAsync(data, 0, data.Length);

                return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, screenSaverItem.Name + ".pptx");
            }
        }

        private void SavePowerPointSlidesToServer(ScreenSaverItem screenSaverItem)
        {
            var path = System.IO.Path.Combine(Server.MapPath(Nebulus.AppConfiguration.Settings.FileStorageLocation));
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            var filepath = path + screenSaverItem.Id + System.IO.Path.GetExtension(screenSaverItem.Slides.FileName);

            screenSaverItem.Slides.SaveAs(filepath);

            screenSaverItem.SlidesPath = filepath;
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
