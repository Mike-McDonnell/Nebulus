using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nebulus.Controllers
{
    public class SlideDeckController : ApiController
    {

        public IHttpActionResult GetSlideInfo()
        {

            var slides = Nebulus.NebulusContext.Create().ScreenSaverItems.Where(slidedeck => slidedeck.Active == true).FirstOrDefault();

            return Ok(new { lastUpdated = slides.Modified, Id = slides.Id });
        }

        public IHttpActionResult GetSlideInfo(int Id)
        {
            var slides = Nebulus.NebulusContext.Create().ScreenSaverItems.Find(Id);

            using (var slidefile = new System.IO.FileStream(slides.SlidesPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                byte[] data = new byte[slidefile.Length];

                slidefile.Read(data, 0, data.Length);

                return Ok(new { lastUpdated = slides.Modified, SlideData = data, Id = slides.Id, SlideShowPresenter = slides.Presenter });
            }
        }
    }
}
