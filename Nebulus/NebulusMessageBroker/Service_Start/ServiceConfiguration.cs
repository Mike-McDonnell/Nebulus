using Nebulus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NebulusMessageBroker.Service_Start
{
    class ServiceConfiguration
    {
        public static async Task<ServiceConfig> GetServiceConfiguration()
        {
            ServiceConfig serviceConfig = new ServiceConfig();

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {
                try
                {
                    client.BaseAddress = new Uri(NebulusMessageBroker.Properties.Settings.Default.ServiceConfigConnectionString);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // New code:
                    HttpResponseMessage response = await client.GetAsync("api/service/ServiceConfig");
                    if (response.IsSuccessStatusCode)
                    {
                        serviceConfig = await response.Content.ReadAsAsync<ServiceConfig>();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new HttpRequestException("Web Service returned " + response.StatusCode + ", Client is unable to authentication with configuration web service. Web service may be configured with 'Windows Authintication' and the client is a non domain user", new Exception(response.ReasonPhrase));
                        }
                        else
                            throw new HttpRequestException("Web Service returned " + response.StatusCode, new Exception(response.ReasonPhrase));
                    }
                }
                catch (Exception ex)
                {
                    AppLogging.Instance.Error("Error: Connecting to Config service ", ex);
                }
            }
            return serviceConfig;
        }
    }
}
