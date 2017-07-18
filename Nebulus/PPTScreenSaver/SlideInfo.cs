using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PPTScreenSaver
{
    class SlideInfo
    {
        private static string SlideShowPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\SlideShowSS";

        public string Id { get; set; }

        public DateTimeOffset lastUpdated {get; set;}

        public string PPTFilePath { get; set; }

        public byte[] SlideData { get; set; }

        public PresenterType SlideShowPresenter { get; set; }

        public static SlideInfo LoadSlideInfo()
        {
            if (!File.Exists(SlideShowPath +"\\" + "SlideInfo.js"))
            {
                if(!Directory.Exists(SlideShowPath))
                {
                    var dirSecurity = new System.Security.AccessControl.DirectorySecurity(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), System.Security.AccessControl.AccessControlSections.Access);
                    dirSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule("Authenticated Users", System.Security.AccessControl.FileSystemRights.Modify, System.Security.AccessControl.AccessControlType.Allow));

                    Directory.CreateDirectory(SlideShowPath, dirSecurity);
                }
                return new SlideInfo();
            }

            using (StreamReader file = File.OpenText(SlideShowPath +"\\" + "SlideInfo.js"))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (SlideInfo)serializer.Deserialize(file, typeof(SlideInfo));
            }
        }

        public virtual void Save()
        {
            using (StreamWriter file = File.CreateText(SlideShowPath + "\\" + "SlideInfo.js"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }

        public virtual async Task<SlideInfo> GetSlideInfo()
        {
            HttpResponseMessage response = null;
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {

                try
                {
                    client.BaseAddress = new Uri(PPTScreenSaver.Properties.Settings.Default.ClientConfigConnectionString);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // New code:
                        response = await client.GetAsync("api/SlideDeck/SlideInfo");
                        if (response.IsSuccessStatusCode)
                        {
                            return (SlideInfo) await response.Content.ReadAsAsync<SlideInfo>();
                        }
                        else
                        {
                            throw new HttpRequestException("Web Service returned " + response.StatusCode, new Exception(response.ReasonPhrase));
                        }
                }
                catch (Exception ex)
                    {
                        AppLogging.Instance.Error("Error: Connecting to Config service ", ex);
                    }

                return new SlideInfo();
            }
        }


        public virtual async Task<bool> UpdateSlideInfo()
        {

            var ServerSlidesInfo = await this.GetSlideInfo();
            if( ServerSlidesInfo.lastUpdated > this.lastUpdated)
            {
                HttpResponseMessage response = null;
                HttpClientHandler handler = new HttpClientHandler();

                handler.UseDefaultCredentials = true;

                using (var client = new HttpClient(handler))
                {

                    try
                    {
                        client.BaseAddress = new Uri(PPTScreenSaver.Properties.Settings.Default.ClientConfigConnectionString);

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // New code:
                        response = await client.GetAsync("api/SlideDeck/Slides/?id=" + ServerSlidesInfo.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var path = SlideShowPath +"\\" + "Slides.pptx";
                            using(var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                ServerSlidesInfo = (SlideInfo)await response.Content.ReadAsAsync<SlideInfo>();
                                await stream.WriteAsync(ServerSlidesInfo.SlideData, 0, ServerSlidesInfo.SlideData.Length);

                                this.Id = ServerSlidesInfo.Id;
                                this.lastUpdated = ServerSlidesInfo.lastUpdated;
                                this.PPTFilePath = path;
                            }
                            this.Save();
                        }
                        else
                        {
                            throw new HttpRequestException("Web Service returned " + response.StatusCode, new Exception(response.ReasonPhrase));
                        }
                    }
                    catch (Exception ex)
                    {
                        AppLogging.Instance.Error("Error: Connecting to Config service ", ex);
                    }

                }

                try
                {
                    PPTInteropSlides.CreateSlides(this.PPTFilePath);
                }
                catch(Exception ex)
                {
                    AppLogging.Instance.Error("Error: Unable to save PowerPoint Slides as images ", ex);
                }

                return true;
            }

            return false;
        }
    }

}
