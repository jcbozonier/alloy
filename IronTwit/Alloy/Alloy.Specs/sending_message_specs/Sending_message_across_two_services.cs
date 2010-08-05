using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.Specs.TestObjects;
using IServiceProvider = Unite.Messaging.Services.IServiceProvider;

namespace Unite.Specs.sending_message_specs
{
    [TestFixture]
    public class When_sending_message_that_is_valid_for_multiple_services
    {
        [Test]
        public void It_should_send_message_using_each_service()
        {
            var messagingPlugIns = new[] {new TestMessagingPlugIn(), new TestMessagingPlugIn()};
            var messagingPlugInProvider = new TestServiceProvider();
            messagingPlugInProvider.Add(messagingPlugIns);

            var unifiedMessenger = new UnifiedMessenger(messagingPlugInProvider);


            unifiedMessenger.SendMessage("recipient", "message");

            Assert.That(messagingPlugIns.Select(x => x.SentMessages.SingleOrDefault().Text).ToArray(), Has.All.EqualTo("message"), "It should send the message via every plugin.");
            Assert.That(messagingPlugIns.Select(x=>x.SentMessages.SingleOrDefault().Address.UserName).ToArray(), Has.All.EqualTo("recipient"), "It should send the message to the correct recipient.");
        }
    }

    public class TestMessagingPlugIn : IMessagingService
    {
        public readonly List<IMessage> SentMessages = new List<IMessage>();

        public bool CanAccept(Credentials credentials)
        {
            return true;
        }

        public List<IMessage> GetMessages()
        {
            return Enumerable.Empty<IMessage>().ToList();
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            SentMessages.Add(new Message(){Address = recipient, Text = message, TimeStamp = DateTime.MinValue});
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public bool CanFind(string address)
        {
            return true;
        }

        public ServiceInformation GetInformation()
        {
            return new ServiceInformation();
        }

        public void StartReceiving()
        {
        }

        public void StopReceiving()
        {
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
        public event EventHandler<ContactEventArgs> ContactsReceived;
        public void IfCanAcceptSet(Credentials credentials)
        {
            
        }
    }

    public class TestServiceProvider : IServiceProvider
    {
        private IMessagingService[] _Services = Enumerable.Empty<IMessagingService>().ToArray();

        public void Add(params IMessagingService[] services)
        {
            _Services = services;
        }

        public IEnumerable<IMessagingService> GetAllServices()
        {
            return _Services;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;

        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public void ForEachPlugIn(Action<IMessagingService> takeAction)
        {
            foreach (var service in _Services)
                takeAction(service);
        }
    }
}