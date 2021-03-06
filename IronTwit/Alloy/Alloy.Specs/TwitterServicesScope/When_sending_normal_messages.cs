﻿using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using IronTwitterPlugIn;
using NUnit.Framework;
using SpecUnit;


namespace Unite.Specs.TwitterServicesScope
{
    [TestFixture]
    public class When_sending_a_large_message_with_no_recipient : context
    {
        [Test]
        public void It_should_not_be_broken_up_into_multiple_messages()
        {
            DataAccess.MessagesSent.ShouldEqual(1);
        }

        [Test]
        public void It_clips_each_message_to_be_at_or_below_the_max_message_length()
        {
            DataAccess.SentMessages.ForEach(message => message.Length.ShouldBeLessThan(MaxMessageLength + 1));
        }

        protected string Recipient = null;

        protected override void Because()
        {
            Utilities.SendMessage(
                new TestSender()
                    {
                        UserName = Recipient
                    },
                "This is a test message from");
        }

        protected override void Context()
        {
        }
    }

    public class TestSender : IIdentity
    {
        public string UserName { get; set; }

        public ServiceInformation ServiceInfo
        {
            get; set;
        }
    }

    [TestFixture]
    public class When_sending_a_large_message_with_recipient_first_time : context
    {
        [Test]
        public void It_should_request_user_credentials()
        {
            CredentialsRequested.ShouldBeTrue();
        }

        [Test]
        public void It_should_not_be_broken_up_into_multiple_messages()
        {
            DataAccess.MessagesSent.ShouldEqual(1);
        }

        [Test]
        public void It_includes_the_recipient_in_the_message()
        {
            DataAccess.SentMessages.ForEach(message => message.Contains(Recipient).ShouldBeTrue());
        }

        [Test]
        public void It_clips_each_message_to_be_at_or_below_the_max_message_length()
        {
            DataAccess.SentMessages.ForEach(message => message.Length.ShouldBeLessThan(MaxMessageLength + 1));
        }

        protected string Recipient = "@ChadBoyer";

        protected override void Because()
        {
            Utilities.SendMessage(
                new FakeRecipient(){UserName = Recipient},
                "This is a test message from");
        }

        protected override void Context()
        {
        }
    }

    public class FakeRecipient : IIdentity
    {
        public string UserName { get; set; }
        public ServiceInformation ServiceInfo { get; set; }
    }

    [TestFixture]
    public abstract class context
    {
        protected TwitterUtilities Utilities;
        protected TestTwitterDataAccess DataAccess;
        protected readonly int MaxMessageLength = 140;
        protected bool CredentialsRequested;

        [TestFixtureSetUp]
        public void Setup()
        {
            DataAccess = new TestTwitterDataAccess();
            Utilities = new TwitterUtilities(DataAccess, MaxMessageLength);
            Utilities.CredentialsRequested += Utilities_CredentialsRequested;

            Context();
            Because();
        }

        void Utilities_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            CredentialsRequested = true;
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestTwitterDataAccess : ITwitterDataAccess
    {
        public int MessagesSent;
        public List<string> SentMessages;

        public TestTwitterDataAccess()
        {
            SentMessages = new List<string>();
        }

        public string SendMessage(Credentials credentials, string message)
        {
            MessagesSent++;
            SentMessages.Add(message);
            return "result message";
        }

        public string GetMessages(Credentials credentials)
        {
            return "result message";
        }

        public string GetContacts(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }
    }
}