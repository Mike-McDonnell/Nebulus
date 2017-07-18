using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPTScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                try
                {
                    if (args[0] != "/s")
                    {
                        Application.Exit();
                    }
                    
                    Nebulus.Win32.PinvokeKeyBoard.KeyPressed += PinvokeKeyBoard_KeyPressed;
                    Nebulus.Win32.PinvokeMouse.MouseAction += PinvokeMouse_MouseAction;

                    Nebulus.Win32.PinvokeKeyBoard.startListening();
                    Nebulus.Win32.PinvokeMouse.Start();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Application.Run(new PPTApp());
                }
                catch(Exception ex)
                {
                    AppLogging.Instance.Error("Error: Unexpected issues starting application", ex);
                }
            }
            
        }
        public static void ShutdownApp()
        {
            Nebulus.Win32.PinvokeKeyBoard.stopListening();
            Nebulus.Win32.PinvokeMouse.stop();
            PowerPointUtlity.CloseSlides();
            Application.Exit();
        }

        private static void PinvokeMouse_MouseAction(object sender, EventArgs e)
        {
            ShutdownApp();
        }

        private static void PinvokeKeyBoard_KeyPressed(object sender, EventArgs e)
        {
            ShutdownApp();
        }
    }
}
