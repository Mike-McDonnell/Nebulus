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
        public static QueueClient NSBQClient;
        internal static void ConfigureServiceHUB()
        {
            try
            {
                ServiceBusConnectionStringBuilder connBuilder = new ServiceBusConnectionStringBuilder(NebulusClient.App.ClientConfiguration.ServiceBUSConenctionString);
                MessagingFactory messageFactory = MessagingFactory.CreateFromConnectionString(connBuilder.ToString());

                NSBQClient = messageFactory.CreateQueueClient(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, ReceiveMode.ReceiveAndDelete);
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
            }
        }
    }
}
