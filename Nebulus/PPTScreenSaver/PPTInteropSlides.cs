using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace PPTScreenSaver
{
    class PPTInteropSlides
    {
        private static PowerPoint.Application PowerPointApp;
        private static PowerPoint.Presentation Presentation;
        internal static void CreateSlides(string PPTpath)
        {
            try
            {
                PowerPointApp = new PowerPoint.Application();
                var presentation = PowerPointApp.Presentations;

                Presentation = presentation.Open2007(PPTpath, MsoTriState.msoCTrue, MsoTriState.msoCTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);

                var imageSlidesDirectory = System.IO.Directory.GetParent(PPTpath) + "\\ImageSlides";

                if(System.IO.Directory.Exists(imageSlidesDirectory))
                {
                    System.IO.Directory.Delete(imageSlidesDirectory, true); 
                }

                System.IO.Directory.CreateDirectory(imageSlidesDirectory);

                Presentation.Export(imageSlidesDirectory, "JPG", 0, 0);

                
            }
            catch (Exception ex) { throw ex; }
            finally { Close(); }
        }

        public static void Close()
        {
            try
            {
                if (Presentation != null)
                {
                    Presentation.Close();
                }

                if (PowerPointApp != null)
                {
                    PowerPointApp.Quit();
                }
            }
            catch { }
        }
    }
}
