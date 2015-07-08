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
        public Marquee()
        {
            InitializeComponent();

            this.Height = 120;

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

            this.Width = desktopWorkingArea.Width;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            CloseOptionTimer.Tick += (o, e) =>
            {
                this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
                this.CloseOptionTimer.Stop();
            };

            CloseOptionTimer.Interval = new TimeSpan(0, 0, 30);
            CloseOptionTimer.Start();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

            this.Width = desktopWorkingArea.Width;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        internal void StartSpeech(string speech)
        {
            if (speech != string.Empty)
            {
                NebulusClient.App_Code.SpeechHelper.Speak(speech);
            }
        }
    }
}
