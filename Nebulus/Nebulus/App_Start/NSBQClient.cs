using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nebulus
{
    class NSBQ
    {
        public static TopicClient NSBQClient;
        internal static void ConfigureServiceHUB()
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
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
            }
        }
    }
}
