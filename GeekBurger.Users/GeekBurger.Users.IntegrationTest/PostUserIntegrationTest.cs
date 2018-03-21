
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

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
             host?.StopAsync()?.Wait();
        }

        [TestMethod]
        public void SendFile()
        {
            var faceBytes = File.ReadAllBytes("..\\..\\..\\resources\\avatar.png");

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
            
            HttpContent content = new ByteArrayContent(faceBytes);
            
            var result = client.PostAsync("users", content).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
