using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPTScreenSaver
{
    class PPTApp : ApplicationContext
    {
        private SlideInfo slideInfo;

        public PPTApp()
        {
            BlanketScreen();
            slideInfo = SlideInfo.LoadSlideInfo();

            if(slideInfo.PPTFilePath != null)
            {
                StartShow();
            }

            Loaded();
        }

        private void StartShow()
        {
            if (slideInfo.SlideShowPresenter == PresenterType.PowerPoint)
            {
                PowerPointUtlity.CloseSlides();
                PowerPointUtlity.ShowSlides(slideInfo.PPTFilePath);
            }
            
            if(slideInfo.SlideShowPresenter == PresenterType.Photos)
            {
                PowerPointUtlity.ShowSlidesAsPhotos(slideInfo.PPTFilePath);
            }
        }

        private async void Loaded()
        {
            if(await slideInfo.UpdateSlideInfo())
            {
                StartShow();
            }
        }

        private void BlanketScreen()
        {
            Form window = new Form();
            window.FormBorderStyle = FormBorderStyle.None;
            window.BackColor = System.Drawing.Color.Black;
            window.Left = System.Windows.Forms.SystemInformation.VirtualScreen.Left;
            window.Top = System.Windows.Forms.SystemInformation.VirtualScreen.Top;
            window.Height = Screen.PrimaryScreen.Bounds.Height;
            window.Width = System.Windows.Forms.SystemInformation.VirtualScreen.Width;
            window.ShowInTaskbar = false;
            window.Activated += (e, o) => 
            {
                PowerPointUtlity.BringToForeGround();
            };
            window.Show();
        }
    }
}
