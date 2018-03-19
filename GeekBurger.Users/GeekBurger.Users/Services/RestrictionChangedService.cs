using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Users.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using GeekBurger.Users.Contract;
using Newtonsoft.Json;

namespace GeekBurger.Users.Services
{
    public class RestrictionChangedService : IRestrictionChangedService
    {
        private string Topic = "UserRetrieved";
        private IConfiguration _configuration;
        private List<Message> _messages;
        private Task _lastTask;
        private IServiceBusNamespace _namespace;

        public RestrictionChangedService(IConfiguration configuration)
        {
            _configuration = configuration;
            _messages = new List<Message>();
            _namespace = _configuration.GetServiceBusNamespace();
            EnsureTopicIsCreated();
        }

        public void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List()
                .Any(topic => topic.Name
                    .Equals(Topic, StringComparison.InvariantCultureIgnoreCase)))
                _namespace.Topics.Define(Topic)
                    .WithSizeInMB(1024).Create();

        }

        public void AddToMessageList(IEnumerable<EntityEntry<Restriction>> changes)
        {
            try
            {
                var restrictions = changes.Where(entity => entity.State != EntityState.Detached && entity.State != EntityState.Unchanged);

                UserRetrievedMessage message = new UserRetrievedMessage();
                message.UserId = restrictions.FirstOrDefault().Entity.UserId;
                message.AreRestrictionsSet = true;
                message.Restrictions = new FoodRestrictionsList();
                if (restrictions.Any(r => r.Entity.Other == false))
                    message.Restrictions.Restrictions = restrictions.Where(r => r.Entity.Other == false).Select(r => r.Entity.Name).ToArray();
                if (restrictions.Any(r => r.Entity.Other == true))
                    message.Restrictions.Others = restrictions.Where(r => r.Entity.Other == true).Select(r => r.Entity.Name).ToArray();

                var serialized = JsonConvert.SerializeObject(message);
                var byteArray = System.Text.Encoding.UTF8.GetBytes(serialized);

                Message oMessage = new Message
                {
                    Body = byteArray,
                    MessageId = Guid.NewGuid().ToString(),
                    Label = message.UserId.ToString()
                };

                _messages.Add(oMessage);
            }
            catch
            {
                //TODO: Logging
            }
        }

        public async void SendMessagesAsync()
        {
            if (_lastTask != null && !_lastTask.IsCompleted)
                return;

            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var topicClient = new TopicClient(config.ConnectionString, Topic);

            _lastTask = SendAsync(topicClient);

            await _lastTask;

            var closeTask = topicClient.CloseAsync();
            await closeTask;
            HandleException(closeTask);
        }

        public async Task SendAsync(TopicClient topicClient)
        {
            int tries = 0;
            Message message;
            while (true)
            {
                if (_messages.Count <= 0)
                    break;

                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }

                var sendTask = topicClient.SendAsync(message);
                await sendTask;
                var success = HandleException(sendTask);

                if (!success)
                    Thread.Sleep(10000 * (tries < 60 ? tries++ : tries));
                else
                    _messages.Remove(message);
            }
        }

        public bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0) return true;

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is ServiceBusCommunicationException)
                    Console.WriteLine("Connection Problem with Host. Internet Connection can be down");
            });

            return false;
        }
    }
}
