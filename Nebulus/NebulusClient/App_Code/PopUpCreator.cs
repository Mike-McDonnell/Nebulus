using Nebulus;
using NebulusClient.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NebulusClient
{
    public class PopUpCreator
    {
        public static MessageBopUp StartMarquee(Nebulus.Models.MessageItem message)
        {
            var marquee = new Marquee();
            try
            {
                marquee.Show();
                marquee.browser.NavigateToString("<!doctype html><html><head><title></title></head><body oncontextmenu='return false;'><marquee>" + message.MessageBody + "</marquee></body></html>");
                marquee.StartSpeech(SpeechHelper.GetSpeechString(message.MessageBody));
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Creating Marquee popup ", ex);
            }

            return new MessageBopUp() { PopUpWindows = marquee, MessageItem = message, StartedTime = DateTime.Now };
        }

        public static MessageBopUp StartPopUp(Nebulus.Models.MessageItem message)
        {
            var popup = new Popup();
            int baseWidth = 1920; int baseHegiht = 1080;

            //f = c/b

            var WidthFactor = (System.Windows.SystemParameters.PrimaryScreenWidth / baseWidth);
            var HegihtFactor = (System.Windows.SystemParameters.PrimaryScreenHeight / baseHegiht);

            try
            {
                if (message.MessageLocation == Nebulus.Models.MessageLocation.Custom)
                {
                    popup.Width = Convert.ToDouble(message.MessageWidth) * WidthFactor; popup.Height = Convert.ToDouble(message.MessageHeight) * HegihtFactor;
                    popup.Top = Convert.ToDouble(message.MessageTop) * HegihtFactor; popup.Left = Convert.ToDouble(message.MessageLeft) * WidthFactor;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Bottom)
                {
                    popup.Width = 800 * WidthFactor; popup.Height = 600 * HegihtFactor;
                    popup.Top = System.Windows.SystemParameters.PrimaryScreenHeight * .45; popup.Left = System.Windows.SystemParameters.PrimaryScreenWidth * .28;

                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Center)
                {
                    popup.Width = 1000 * WidthFactor; popup.Height = 800 * HegihtFactor;
                    popup.Top = System.Windows.SystemParameters.PrimaryScreenHeight * .20; popup.Left = System.Windows.SystemParameters.PrimaryScreenWidth * .23;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.FullScreen)
                {
                    popup.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen; popup.WindowState = System.Windows.WindowState.Maximized;
                    //popup.Width = System.Windows.SystemParameters.PrimaryScreenWidth; popup.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Left)
                {
                    popup.Top = System.Windows.SystemParameters.PrimaryScreenHeight * .1; popup.Left = System.Windows.SystemParameters.PrimaryScreenWidth * .01;
                    popup.Width = 500 * WidthFactor; popup.Height = 900 * HegihtFactor;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Right)
                {
                    popup.Top = System.Windows.SystemParameters.PrimaryScreenHeight * .1; popup.Left = System.Windows.SystemParameters.PrimaryScreenWidth * .73;
                    popup.Width = 500 * WidthFactor; popup.Height = 900 * HegihtFactor;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Top)
                {
                    popup.Top = System.Windows.SystemParameters.PrimaryScreenHeight * .1; popup.Left = System.Windows.SystemParameters.PrimaryScreenWidth * .28;
                    popup.Width = 800 * WidthFactor; popup.Height = 600 * HegihtFactor;
                }

                popup.Loaded += (o, evt) =>
                {

                    popup.browser.NavigateToString("<!doctype html><html><head><title></title></head><body oncontextmenu='return false;' style='transform: scale(" + WidthFactor + "," + HegihtFactor + "); -ms-transform: scale(" + WidthFactor + "," + HegihtFactor + "); -webkit-transform: scale(" + WidthFactor + "," + HegihtFactor + ");'>" + message.MessageBody + "</body></html>");

                };

                popup.widthZoomFactor = WidthFactor; popup.heightZoomFactor = HegihtFactor;

                popup.Show();
                popup.StartSpeech(SpeechHelper.GetSpeechString(message.MessageBody));
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Creating Message popup  ", ex);
            }
            return new MessageBopUp() { PopUpWindows = popup, MessageItem = message, StartedTime = DateTime.Now };

        }
    }
}
