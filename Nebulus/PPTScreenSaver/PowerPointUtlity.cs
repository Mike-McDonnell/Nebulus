using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PPTScreenSaver
{
    class PowerPointUtlity
    {
        [DllImport("USER32.DLL")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("USER32.DLL")]
        static extern int PostMessage(IntPtr hWnd, UInt32 msg, int wParam, int lParam);
        [DllImport("USER32.DLL", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool EndTask(IntPtr hWnd, bool fShutDown, bool fForce);

        private const UInt32 WM_CLOSE = 0x0010;

        private static string PPTtitle = "PowerPoint Slide Show - [Slides.pptx]";

        static System.Diagnostics.Process _PowerPointProcess;

        internal static void ShowSlides(string PPTpath)
        {
            try
            {
                _PowerPointProcess = System.Diagnostics.Process.Start("POWERPNT.EXE", string.Format("/s {0}", PPTpath));
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ShowSlidesAsPhotos(PPTpath);
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Opining PowerPoint ", ex);
            }
        }

        internal static void ShowSlidesAsPhotos(string PPTpath)
        {
            try
            {
                PPTPhotoSlides.Show(PPTpath);
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Opining showing SlideDeck as Photos", ex);
            }
        }

        internal static void CloseSlides()
        {
            try
            {
                if (_PowerPointProcess != null && !_PowerPointProcess.HasExited)
                {
                    if (!_PowerPointProcess.CloseMainWindow())
                    {
                        _PowerPointProcess.Kill();
                    }
                }
                else 
                {
                    Win32CloseWindow();
                }

                PPTInteropSlides.Close();
              
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Closing PowerPoint ", ex);
            }
        }

        private static void Win32CloseWindow()
        {
            IntPtr hWnd = FindWindow(null, PPTtitle);
            if (hWnd != null)
            {
                PostMessage(hWnd, WM_CLOSE, 0, 0);
            }
        }

        private static void Win32endTask()
        {
            IntPtr hWnd = FindWindow(null, PPTtitle);
            if (hWnd != null)
            {
                EndTask(hWnd, true, true);
            }
        }

        internal static void BringToForeGround()
        {
            if(_PowerPointProcess != null)
            {
                try
                {
                    SetForegroundWindow(_PowerPointProcess.MainWindowHandle);
                }
                catch
                { }
            }
        }
    }
}
