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

                var SubscriptionName = Environment.MachineName; 

                if(NebulusClient.Properties.Settings.Default.SubscriptionNameLevel == 1)
                {
                    SubscriptionName = Environment.UserName;
                }

                if (NebulusClient.Properties.Settings.Default.SubscriptionNameLevel == 2)
                {
                    SubscriptionName = Environment.MachineName + "\\" + Environment.UserName;
                }

                if(NebulusClient.Properties.Settings.Default.SubscriptionNameLevel == 3)
                {
                    try
                    {
                        SubscriptionName = Environment.GetEnvironmentVariable("CLIENTNAME");
                    }
                    catch
                    { }
                }


                if (namespaceManager.SubscriptionExists(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, SubscriptionName))
                {
                    namespaceManager.DeleteSubscription(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, SubscriptionName);
                }

                namespaceManager.CreateSubscription(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, SubscriptionName);

                NSBQClient = messageFactory.CreateSubscriptionClient(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, Environment.MachineName, ReceiveMode.ReceiveAndDelete);

                AddRules(NSBQClient);

                
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
            }
        }

        private static void AddRules(SubscriptionClient Client)
        {
            if (NebulusClient.App.ClientConfiguration.GroupTAGsEnabled)
            {
                foreach(var group in System.Security.Principal.WindowsIdentity.GetCurrent().Groups)
                {
                    Client.AddRule(new RuleDescription(group.Value));
                }
            }
            if (NebulusClient.App.ClientConfiguration.UserTAGsEnabled)
            {
                Client.AddRule("UserName", new SqlFilter("UserName = '" + System.Security.Principal.WindowsIdentity.GetCurrent().User + "'"));
            }
            if (NebulusClient.App.ClientConfiguration.ComputerTAGsEnabled)
            {
                Client.AddRule("ComputerName", new SqlFilter("ComputerName = '" + Environment.MachineName + "'"));
                System.Windows.MessageBox.Show(Environment.MachineName);
            }
            if (NebulusClient.App.ClientConfiguration.SubNetTAGsEnabled)
            {
                try
                {
                    var card = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
                    Client.AddRule("Subnet", new SqlFilter(card.GetIPProperties().GatewayAddresses.FirstOrDefault().Address.ToString()));
                }
                catch
                { }
            }
        }
    }
}
