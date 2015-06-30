using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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

                namespaceManager.CreateSubscription(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, SubscriptionName, new SqlFilter(BuildRules()));

                NSBQClient = messageFactory.CreateSubscriptionClient(NebulusClient.App.ClientConfiguration.ServiceBUSQueueName, Environment.MachineName, ReceiveMode.ReceiveAndDelete);
                
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
            }
        }

        private static string BuildRules()
        {
            var SQLQuery = "Tags LIKE '%BROADCAST%' OR";

            try
            {
                if (NebulusClient.App.ClientConfiguration.GroupTAGsEnabled)
                {
                    var pC = new PrincipalContext(ContextType.Domain);

                    UserPrincipal user = UserPrincipal.FindByIdentity(pC, IdentityType.Sid, System.Security.Principal.WindowsIdentity.GetCurrent().User.Value);

                    if (user != null)
                    {
                        foreach (var group in user.GetGroups().Where(p => ((GroupPrincipal)p).IsSecurityGroup == false).Select(g => g.SamAccountName).ToList<string>())
                        {
                            SQLQuery += " Tags LIKE '%" + group + "%' OR";
                        }
                    }                  
                }
                if (NebulusClient.App.ClientConfiguration.UserTAGsEnabled)
                {
                    SQLQuery += " Tags LIKE '%" + Environment.UserName + "%' OR";
                }
                if (NebulusClient.App.ClientConfiguration.ComputerTAGsEnabled)
                {
                    SQLQuery += " Tags LIKE '%" + Environment.MachineName.ToUpper() + "%' OR";
                }
                if (NebulusClient.App.ClientConfiguration.SubNetTAGsEnabled)
                {
                    try
                    {
                        var card = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up).FirstOrDefault();
                        SQLQuery += " Tags LIKE '%" + card.GetIPProperties().GatewayAddresses.FirstOrDefault().Address.ToString() + "%'";
                    }
                    catch
                    { }
                }

                if (SQLQuery.EndsWith("OR"))
                {
                    SQLQuery = SQLQuery.Substring(0, SQLQuery.Length - 2).Trim();
                }
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Creating SQLQuery ", ex);
            }
            return SQLQuery;
        }
    }
}
