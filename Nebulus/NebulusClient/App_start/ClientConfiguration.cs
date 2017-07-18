using Nebulus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NebulusClient
{
    public class ClientStart
    {
        public static async Task<ClientConfig> GetClientConfiguration()
        {
            ClientConfig clientConfig = new ClientConfig();
            HttpResponseMessage response = null;

            while (response == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;

                using (var client = new HttpClient(handler))
                {

                    try
                    {
                        client.BaseAddress = new Uri(NebulusClient.Properties.Settings.Default.ClientConfigConnectionString);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // New code:
                        response = await client.GetAsync("api/client/config");
                        if (response.IsSuccessStatusCode)
                        {
                            clientConfig = await response.Content.ReadAsAsync<ClientConfig>();
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
                        System.Threading.Thread.Sleep(new Random().Next(30, 60) * 1000);
                    }

                }
            }
            return clientConfig;
        }
    }
}
