using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nebulus.Controllers
{
    public class ServiceController : ApiController
    {
        [ActionName("ServiceConfig")]
        public IHttpActionResult GetServiceConfig()
        {
            //Generatre Client Config

            var ServiceConfig = new
            {
                ServiceBUSConenctionString = Nebulus.AppConfiguration.Settings.ServiceBUSConenctionString,
                ServiceBUSQueueName = Nebulus.AppConfiguration.Settings.ServiceBUSQueueName,
                DatabaseConnectionString = Nebulus.AppConfiguration.Settings.DatabaseConnectionString
            };

            return Ok(ServiceConfig);
        }
    }
}
