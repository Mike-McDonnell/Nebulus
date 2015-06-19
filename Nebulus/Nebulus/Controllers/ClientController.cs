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
        public IHttpActionResult GetConfig()
        {
            //Generatre Client Config

            var newClientConfig = new ClientConfig();
            newClientConfig.ServiceBUSConenctionString = Nebulus.AppConfiguration.Settings.ServiceBUSConenctionString;
            newClientConfig.ServiceBUSQueueName = Nebulus.AppConfiguration.Settings.ServiceBUSQueueName;
            
            return Ok(newClientConfig);
        }
    }
}
