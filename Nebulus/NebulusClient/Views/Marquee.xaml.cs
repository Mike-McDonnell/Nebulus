using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NebulusClient
{
    /// <summary>
    /// Interaction logic for Marquee.xaml
    /// </summary>
    public partial class Marquee : Window
    {
        private System.Windows.Threading.DispatcherTimer CloseOptionTimer = new System.Windows.Threading.DispatcherTimer();
        private int TextLengthTimeOut;
        public Marquee()
        {
            InitializeComponent();

            this.Height = 120;

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

            this.Width = desktopWorkingArea.Width;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

            this.Width = desktopWorkingArea.Width;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        public void StartCloseOptionTimer(int TimeOutSeconds)
        {
            this.TextLengthTimeOut = TimeOutSeconds;

            CloseOptionTimer.Tick += (o, e) =>
            {
                this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
                this.CloseOptionTimer.Stop();
            };

            CloseOptionTimer.Interval = new TimeSpan(0, 0, TextLengthTimeOut);
            CloseOptionTimer.Start();
        }

        internal async void StartSpeech(string speech)
        {
            while (this.IsLoaded)
            {
                if (speech != string.Empty)
                {
                    NebulusClient.App_Code.SpeechHelper.Speak(speech);
                }

                await Task.Delay(TextLengthTimeOut * 1000);
            }
        }
    }
}
