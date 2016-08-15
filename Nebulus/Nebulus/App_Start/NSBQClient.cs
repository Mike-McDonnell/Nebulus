using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nebulus
{
    class NSBQ
    {
        public static TopicClient NSBQClient;
        public static NotificationHubClient NNHClient;
        internal static void ConfigureServiceHUB()
        {
            if (AppConfiguration.Settings.ServiceBUSConenctionString != string.Empty)
            {
                try
                {
                    ServiceBusConnectionStringBuilder connBuilder = new ServiceBusConnectionStringBuilder(AppConfiguration.Settings.ServiceBUSConenctionString);

                    MessagingFactory messageFactory = MessagingFactory.CreateFromConnectionString(connBuilder.ToString());
                    NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connBuilder.ToString());

                    string QueueName = AppConfiguration.Settings.ServiceBUSQueueName;

                    if (!namespaceManager.TopicExists(QueueName))
                    {
                        namespaceManager.CreateTopic(QueueName);
                    }

                    NSBQClient = messageFactory.CreateTopicClient(QueueName);
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
                }
            }
            if (AppConfiguration.Settings.NotificationHubServerConenctionString != string.Empty)
            {
                try
                {
                    ServiceBusConnectionStringBuilder connBuilder = new ServiceBusConnectionStringBuilder(AppConfiguration.Settings.NotificationHubServerConenctionString);
                    MessagingFactory messageFactory = MessagingFactory.CreateFromConnectionString(connBuilder.ToString());
                    NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connBuilder.ToString());

                    string NotificationHubName = AppConfiguration.Settings.NotificationHubName;

                    //if (namespaceManager.NotificationHubExists(NotificationHubName))
                    //{
                        
                        NNHClient = NotificationHubClient.CreateClientFromConnectionString(connBuilder.ToString(), AppConfiguration.Settings.NotificationHubName);
                    //}
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error("Error: Connecting to NotificationHub ", ex);
                }
            }
        }
    }
}
