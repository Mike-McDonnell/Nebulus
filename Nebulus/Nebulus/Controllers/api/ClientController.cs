using NebulusClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nebulus.Controllers
{
    public class ClientController : ApiController
    {
        [ActionName("config")]
        public IHttpActionResult GetConfig()
        {
            //Generatre Client Config

            var newClientConfig = new ClientConfig();
            newClientConfig.NotificationHubName = Nebulus.AppConfiguration.Settings.NotificationHubName;
            newClientConfig.NotificationHubConenctionString = Nebulus.AppConfiguration.Settings.NotificationHubClientConenctionString;
            newClientConfig.UseNotificationHub = Nebulus.AppConfiguration.Settings.WindowsClientsUseNotificationHub;
            newClientConfig.ServiceBUSConenctionString = Nebulus.AppConfiguration.Settings.ServiceBUSConenctionString;
            newClientConfig.ServiceBUSQueueName = Nebulus.AppConfiguration.Settings.ServiceBUSQueueName;
            newClientConfig.GroupTAGsEnabled = Nebulus.AppConfiguration.Settings.GroupTAGsEnabled;
            newClientConfig.ComputerTAGsEnabled = Nebulus.AppConfiguration.Settings.ComputerTAGsEnabled;
            newClientConfig.UserTAGsEnabled = Nebulus.AppConfiguration.Settings.UserTAGsEnabled;
            newClientConfig.SubNetTAGsEnabled = Nebulus.AppConfiguration.Settings.SubNetTAGsEnabled;
            
            return Ok(newClientConfig);
        }
    }
}
