﻿using Microsoft.ServiceBus.Messaging;
using Nebulus;
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

        public static List<MessageBopUp> MessageList = new List<MessageBopUp>();
        private System.Windows.Threading.DispatcherTimer ListTimer = new System.Windows.Threading.DispatcherTimer();

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            AppLogging.Instance.Info("Starting Nebulus Client");

            ClientConfiguration = await ClientStart.GetClientConfiguration();
            Nebulus.NSBQ.ConfigureServiceHUB();

            AppLogging.Instance.Info("Client Configured to connect to: " + ClientConfiguration.ServiceBUSConenctionString);

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = true;
            options.MaxConcurrentCalls = 1;

            try
            {
                Nebulus.NSBQ.NSBQClient.OnMessage((o) =>
                {
                    try
                    {
                        var message = o.GetBody<Nebulus.Models.MessageItem>();

                        if (message.PassesSecurityFilter)
                        {

                            if (message.Expiration > DateTimeOffset.Now)
                            {
                                if (message.MessageType == Nebulus.Models.MessageType.Marquee)
                                {
                                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                                    {
                                        CloseOpenMarquee();
                                        MessageList.Add(PopUpCreator.StartMarquee(message));
                                    }));

                                }
                                if (message.MessageType == Nebulus.Models.MessageType.Popup)
                                {
                                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                                    {
                                        MessageList.Add(PopUpCreator.StartPopUp(message));
                                    }));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                               {
                                   AppLogging.Instance.Error("Error: Processing Message popup ", ex);
                               }));
                    }

                }, options);
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Receving Message popup  ", ex);
            }

            ListTimer.Tick += CheckPopUpExperation;
            ListTimer.Interval = new TimeSpan(0, 0, 5);
            ListTimer.Start();
        }

        private void CloseOpenMarquee()
        {
            var cMarquee = MessageList.Where(popup => popup.MessageItem.MessageType == Nebulus.Models.MessageType.Marquee).FirstOrDefault();
            if(cMarquee != null)
            {
                cMarquee.PopUpWindows.Close();
                MessageList.Remove(cMarquee);
            }
        }

        private void CheckPopUpExperation(object sender, EventArgs e)
        {
            try
            {
                var expiredPopups = MessageList.Where(popup => popup.MessageItem.Expiration < DateTimeOffset.Now);
                var finishedPopUp = MessageList.Where(popup => popup.StartedTime.AddHours(popup.MessageItem.duration) < DateTime.Now);

                foreach (var popup in expiredPopups)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                          {
                              try
                              {
                                  if (popup.PopUpWindows.IsLoaded)
                                  {
                                      popup.PopUpWindows.Close();
                                  }

                              }
                              catch { }
                          }));
                }

                foreach (var popup in finishedPopUp)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                          {
                              try
                              {
                                  popup.PopUpWindows.Close();
                                  
                              }
                              catch { }
                          }));
                }

                foreach (var popup in MessageList.Where(p => !p.PopUpWindows.IsLoaded))
                {
                    popup.MessageItem = null;
                    popup.PopUpWindows = null;
                    
                    MessageList.Remove(popup);
                }
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: closing popup  ", ex);
            }
        } 
    }
}
