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
        [AllowAnonymous]
        [ActionName("config")]
        public IHttpActionResult GetConfig()
        {
            //Generatre Client Config

            var newClientConfig = new ClientConfig();
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
