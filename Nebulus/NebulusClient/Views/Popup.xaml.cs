using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        public double widthZoomFactor;
        public double heightZoomFactor;

        public Popup()
        {
            InitializeComponent();
            
            this.browser.Navigated += (o,e) => {
                HideScriptErrors(this.browser, true);
            };
        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(wb);

            objComWebBrowser.GetType().InvokeMember(

            "Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });

            var zoolwPercent = Convert.ToInt32(100 * widthZoomFactor);
            var zoolhPercent = Convert.ToInt32(100 * heightZoomFactor);

            objComWebBrowser.GetType().InvokeMember(

            "ExecWB", BindingFlags.InvokeMethod, null, objComWebBrowser, new object[] { 63, 2, zoolwPercent, zoolwPercent });
        }

    }
}
