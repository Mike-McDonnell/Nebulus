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

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;


            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://143.83.140.141:8080");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/client/config");
                if (response.IsSuccessStatusCode)
                {
                    clientConfig = await response.Content.ReadAsAsync<ClientConfig>();
                }
            }
            return clientConfig;
        }
    }
}
