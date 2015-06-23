using Nebulus;
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
                marquee.browser.NavigateToString("<marquee>" + message.MessageBody + "</marquee>");   
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

            try
            {
                if (message.MessageLocation == Nebulus.Models.MessageLocation.Custom)
                {
                    popup.Width = Convert.ToDouble(message.MessageWidth); popup.Height = Convert.ToDouble(message.MessageHeight);
                    popup.Top = Convert.ToDouble(message.MessageTop); popup.Left = Convert.ToDouble(message.MessageLeft);
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Bottom)
                {
                    popup.Width = 800; popup.Height = 600;

                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Center)
                {
                    popup.Width = 1000; popup.Height = 800;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.FullScreen)
                {
                    popup.Width = System.Windows.SystemParameters.PrimaryScreenWidth; popup.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Left)
                {
                    popup.Width = 500; popup.Height = 900;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Right)
                {
                    popup.Width = 500; popup.Height = 900;
                }
                else if (message.MessageLocation == Nebulus.Models.MessageLocation.Top)
                {
                    popup.Width = 800; popup.Height = 600;
                }

                popup.Loaded += (o, evt) =>
                {

                    popup.browser.NavigateToString("<!doctype html>" + "<html><head><title></title></head><body>" + message.MessageBody + "</body></html>");

                };

                popup.Show();
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Creating Message popup  ", ex);
            }
            return new MessageBopUp() { PopUpWindows = popup, MessageItem = message, StartedTime = DateTime.Now };

        }
    }
}
