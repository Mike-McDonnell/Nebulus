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
            ServiceBusConnectionStringBuilder connBuilder = new ServiceBusConnectionStringBuilder(Nebulus.Properties.Settings.Default.NebulusHUBConnectionString);
            MessagingFactory messageFactory = MessagingFactory.CreateFromConnectionString(connBuilder.ToString());
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connBuilder.ToString());

            string QueueName = "NebulusSprachSBQueue";

            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }
            
            NSBQClient = messageFactory.CreateQueueClient(QueueName);
        }
    }
}
