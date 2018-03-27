
using GeekBurger.Users.Contract;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GeekBurger.Users.IntegrationTest
{
    [TestClass]
    public class PostUserIntegrationTest
    {
        IWebHost host;

        [TestInitialize]
        public void Initialize()
        {
            host = WebHost.CreateDefaultBuilder()
                           .UseStartup<Startup>()
                           .UseKestrel(options =>
                                options.Listen(IPAddress.Loopback, 5000)
                           )
                           .Build();
            
            host.RunAsync();
        }

        [TestCleanup]
        public void CleanMess()
        {

            System.Threading.Thread.Sleep(5000);
             host?.StopAsync()?.Wait();
        }

        [TestMethod]
        public void SendFile()
        {
            var faceBytes = File.ReadAllBytes("C:\\TMWX9rEB_400x400.jpg");

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
            
            HttpContent content = new ByteArrayContent(faceBytes);
            
            var result = client.PostAsync("users", content).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void SendRestrictionsAndThenFile()
        {
            var faceBytes = File.ReadAllBytes("C:\\download.jpg");

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            FoodRestrictionsList list = new FoodRestrictionsList();
            list.Restrictions = new string []{ "leite", "amendoim" };
            list.Others = new string[] { "peixe" };

            var contentRestrictions = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");

            var resultRestrictions = client.PostAsync("users/a42c8a19-350d-4400-9803-227689be081b/foodrestrictions", contentRestrictions).Result;

            HttpContent content = new ByteArrayContent(faceBytes);

            var result = client.PostAsync("users", content).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void VerifyBadRequest()
        {
            int maxSizePlusOne = 4 * 1024 * 1024 + 1;

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            HttpContent content = new ByteArrayContent(new byte[maxSizePlusOne]);

            var result = client.PostAsync("users", content).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
