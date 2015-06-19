using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NebulusClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ClientConfig ClientConfiguration;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ClientConfiguration = await ClientStart.GetClientConfiguration();
            Nebulus.NSBQ.ConfigureServiceHUB();

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = true;
            options.MaxConcurrentCalls = 1;

            Nebulus.NSBQ.NSBQClient.OnMessage((o) =>
            {
               var message = o.GetBody<Nebulus.Models.MessageItem>();
               if(message.MessageType == Nebulus.Models.MessageType.Marquee)
               {
                   Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                   {
                       StartMarquee(message);
                   }));

               }

            });

        }

        private static void StartMarquee(Nebulus.Models.MessageItem message)
        {
            var marquee = new Marquee();
            marquee.Show();
            marquee.browser.NavigateToString(message.MessageBody);

            
        }
    }
}
