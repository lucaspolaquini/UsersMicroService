using GeekBurger.Users.Contract;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Services
{
    public class ServiceBusService : IServiceBus
    {
        private readonly IConfiguration configuration;
        private readonly Lazy<IServiceBusNamespace> busNamespace;

        public ServiceBusService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.busNamespace = new Lazy<IServiceBusNamespace>(() => this.configuration.GetServiceBusNamespace());
           
        }

        public void EnsureTopicIsCreated(string topic)
        {
            if (!busNamespace.Value.Topics.List()
                .Any(t => t.Name
                    .Equals(topic, StringComparison.InvariantCultureIgnoreCase)))
                busNamespace.Value.Topics.Define(topic)
                    .WithSizeInMB(1024).Create();

        }

        public async Task PostMessage(string topic, UserRetrievedMessage message)
        {
            EnsureTopicIsCreated(topic);
            
            var byteArray = message.Serialize();

            var topicClient = new TopicClient(configuration.GetSection("serviceBus")["connectionString"], topic);

            Message oMessage = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = byteArray
            };

            await topicClient.SendAsync(oMessage);
        }

    }
}
