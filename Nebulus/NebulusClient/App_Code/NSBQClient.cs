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
        public static SubscriptionClient NSBQClient;
        internal static void ConfigureServiceHUB()
        {
            try
            {
                ServiceBusConnectionStringBuilder connBuilder = new ServiceBusConnectionStringBuilder(NebulusClient.App.ClientConfiguration.ServiceBUSConenctionString);
                MessagingFactory messageFactory = MessagingFactory.CreateFromConnectionString(connBuilder.ToString());
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connBuilder.ToString());

                if (!namespaceManager.SubscriptionExists(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, Environment.MachineName ))
                {
                    namespaceManager.CreateSubscription(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, Environment.MachineName);
                }

                NSBQClient = messageFactory.CreateSubscriptionClient(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, Environment.MachineName, ReceiveMode.ReceiveAndDelete);
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
            }
        }
    }
}
