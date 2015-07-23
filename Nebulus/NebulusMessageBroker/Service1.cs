using Nebulus;
using Nebulus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NebulusMessageBroker
{
    public partial class Service1 : ServiceBase
    {
        private Service_Start.ServiceConfig ServiceConfiguration;
        private NebulusContext DataBaseContext;
        private Service_Code.MessageServiceClient MessageWebServiceClient;
        private List<MessageItem> messageList = new List<MessageItem>();

        public Service1()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            AppLogging.Instance.Info("Starting Messaging Broker Service");
            try
            {
                this.ServiceConfiguration = await Service_Start.ServiceConfiguration.GetServiceConfiguration();
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error(ex);
            }

            AppLogging.Instance.Info("Connecting to Database, " + ServiceConfiguration.DatabaseConnectionString);

            try
            {
                this.DataBaseContext = new NebulusContext(this.ServiceConfiguration.DatabaseConnectionString);
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error Connecting to database", ex);
            }

            AppLogging.Instance.Info("Connecting to Messaging WebService, " + ServiceConfiguration.NebulusMessageWebServiceUri);
            try
            {
                this.MessageWebServiceClient = await Service_Code.MessageServiceClient.CreateMessageServiceClient(NebulusMessageBroker.Properties.Settings.Default.ServiceConfigConnectionString + "/api/service/PostMessage");
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error(ex);
            }

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 30000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

        }

        protected override void OnStop()
        {

        }

        private async void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ServiceConfiguration != null)
            {
                if (this.DataBaseContext != null)
                {
                    try
                    {
                        AppLogging.Instance.Debug("Reconnecting to Database");
                        this.DataBaseContext = new NebulusContext(ServiceConfiguration.DatabaseConnectionString);

                        int windowOffSet = 5;
                        var timeWindowEnd = DateTimeOffset.Now.AddMinutes(windowOffSet);

                        var messages = this.DataBaseContext.MessageItems.Where(message => message.ScheduleStart >= DateTimeOffset.Now && message.ScheduleStart <= timeWindowEnd && message.Expiration >= timeWindowEnd || ((message.ScheduleInterval != ScheduleIntervalType.Never && message.ScheduleStart.Hour == DateTimeOffset.Now.Hour && message.Expiration >= timeWindowEnd)));

                        foreach (var mItem in messages)
                        {
                            if (!messageList.Any(message => message.MessageItemId == mItem.MessageItemId))
                            {
                                messageList.Add(mItem);
                                var mTask = Task.Run(async () =>
                                {
                                    AppLogging.Instance.Debug("Queueing Message, " + mItem.MessageItemId);
                                    await Task.Delay(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, mItem.ScheduleStart.Hour, mItem.ScheduleStart.Minute, 0) - DateTimeOffset.Now);
                                    MessageWebServiceClient.SendMessage(mItem);
                                });

                            }
                        }

                        messageList.RemoveAll(message => message.ScheduleStart < DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(windowOffSet)));
                    }
                    catch (Exception ex)
                    {
                        AppLogging.Instance.Error("Service Error: ", ex);
                    }
                }
                else
                {
                    AppLogging.Instance.Info("Reconnecting to Database, " + ServiceConfiguration.DatabaseConnectionString);

                    try
                    {
                        this.DataBaseContext = new NebulusContext(ServiceConfiguration.DatabaseConnectionString);
                    }
                    catch (Exception ex)
                    {
                        AppLogging.Instance.Error("Error Connecting to database", ex);
                    }
                }
            }
            else
            {
                AppLogging.Instance.Info("Retrying Messaging Service Configuration Connection");
                try
                {
                    this.ServiceConfiguration = await Service_Start.ServiceConfiguration.GetServiceConfiguration();
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error(ex);
                }
            }
        }
    }
}
    
