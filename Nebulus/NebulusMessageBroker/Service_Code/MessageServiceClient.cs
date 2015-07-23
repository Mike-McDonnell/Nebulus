using Nebulus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NebulusMessageBroker.Service_Code
{
    public class MessageServiceClient
    {
        private string ServiceConnectionUri;
        private HttpClient WebServiceCleint;

        public MessageServiceClient(string ServiceConnectionUri)
        {
            // TODO: Complete member initialization
            this.ServiceConnectionUri = ServiceConnectionUri;

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            this.WebServiceCleint = new HttpClient(handler);
        }

        internal async static Task<MessageServiceClient> CreateMessageServiceClient(string ServiceConnectionUri)
        {
            var messageServiceClient = new MessageServiceClient(ServiceConnectionUri);

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await client.GetAsync(ServiceConnectionUri);
                if (response.IsSuccessStatusCode)
                {
                    return messageServiceClient;
                }
                else
                {
                    throw new Exception("Unable To connect to WebService Uri, " + ServiceConnectionUri + ", " + response.RequestMessage);
                }
            }
        }

        internal void SendMessage(MessageItem mItem)
        {
            try
            {
                WebServiceCleint.PostAsJsonAsync(this.ServiceConnectionUri + "/api/service/PostMessage", mItem);
                AppLogging.Instance.Debug("Sent Message Id:" + mItem.MessageItemId + " Title:" + mItem.MessageTitle);
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error sending Message to web service, " + mItem.MessageItemId, ex);
            }
        }
    }
}
