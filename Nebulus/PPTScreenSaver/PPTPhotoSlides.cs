using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPTScreenSaver
{
    class PPTPhotoSlides
    {
        private static string[] ImageList;

        internal static async void Show(string PPTpath)
        {
            var imageSlidesDirectory = System.IO.Directory.GetParent(PPTpath) + "\\ImageSlides";

            ImageList = System.IO.Directory.GetFiles(imageSlidesDirectory, "*.JPG", System.IO.SearchOption.AllDirectories);

            Form slidewindows = new Form();
            slidewindows.FormBorderStyle = FormBorderStyle.None;
            slidewindows.BackColor = System.Drawing.Color.Black;
            slidewindows.Left = System.Windows.Forms.SystemInformation.VirtualScreen.Left;
            slidewindows.Top = System.Windows.Forms.SystemInformation.VirtualScreen.Top;
            slidewindows.Height = Screen.PrimaryScreen.Bounds.Height;
            slidewindows.Width = Screen.PrimaryScreen.Bounds.Width;
            slidewindows.ShowInTaskbar = false;
            slidewindows.TopMost = true;
            slidewindows.Show();

            while (true)
            {
                foreach (var imageUri in ImageList)
                {
                    try
                    {

                        slidewindows.BeginInvoke(new Action(() =>
                        {
                            slidewindows.BackgroundImage = System.Drawing.Image.FromFile(imageUri);
                            slidewindows.BackgroundImageLayout = ImageLayout.Zoom;
                            slidewindows.Refresh();

                        }));
                    }
                    catch { }

                    await Task.Delay(15 * 1000);
                }
            }
        }
    }
}
