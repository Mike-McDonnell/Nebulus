using Microsoft.ServiceBus.Messaging;
using Nebulus.Models;
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
            //Generatre Service Config

            var ServiceConfig = new
            {
                DatabaseConnectionString = Nebulus.AppConfiguration.Settings.DatabaseConnectionString
            };

            return Ok(ServiceConfig);
        }

        [HttpPost]
        public IHttpActionResult PostMessage(MessageItem messageItem)
        {
            try
            {
                BrokeredMessage sendMessage = new BrokeredMessage(messageItem);
                if (messageItem.TargetGroup != null && messageItem.TargetGroup != string.Empty)
                {
                    sendMessage.Properties.Add("Tags", messageItem.TargetGroup);
                }
                else
                {
                    sendMessage.Properties.Add("Tags", "BROADCAST");
                }

                NSBQ.NSBQClient.Send(sendMessage);
                AppLogging.Instance.Info("Message sent");
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to ServiceBus ", ex);
                return InternalServerError(ex);
            }
            return Ok();
        }
    }
}
