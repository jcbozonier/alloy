﻿using System;
using System.Collections.Generic;
using IronTwit.Models;
using IronTwit.Models.Twitter;
using IronTwit.Utilities;
using IronTwit.ViewModels;
using NUnit.Framework;
using SpecUnit;
using StructureMap;

namespace Specs.Application_starting.Receiving_messages
{
    [TestFixture]
    public class When_refresh_is_requested : context
    {
        [Test]
        public void It_should_get_items()
        {
            Model.Messages.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void It_should_be_able_to_refresh()
        {
            Model.ReceiveMessage.CanExecute(null).ShouldBeTrue();
        }

        protected override void Because()
        {
            Model.ReceiveMessage.Execute(null);
        }

        protected override void Context()
        {
            Model.ApplicationStarting();
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;

        protected bool Application_Asked_For_User_Name_And_Password
        {
            get
            {
                return !String.IsNullOrEmpty(Model.UserName) && !String.IsNullOrEmpty(Model.Password);
            }
        }

        protected bool Message_was_sent
        {
            get;
            set;
        }


        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrapper.BootstrapStructureMap();

            Utilities = new TestTwitterUtilities();

            ObjectFactory.EjectAllInstancesOf<ITwitterUtilities>();
            ObjectFactory.Inject<ITwitterUtilities>(Utilities);

            Model = ObjectFactory.GetInstance<MainView>();
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestTwitterUtilities : ITwitterUtilities
    {
        public string Username;
        public string Password;
        public string Message;

        public List<IMessage> GetUserMessages(string username, string password)
        {
            Username = username;
            Password = password;

            return new List<IMessage>
                       {
                           new Tweet()
                               {
                                   Text="Message 1",
                                   Sender = new TwitterUser()
                                              {
                                                  AccountName = "darkxanthos"
                                              }
                               }
                       };
        }

        public void SendMessage(string username, string password, string message, string recipient)
        {
            Username = username;
            Password = password;
            Message = message;
        }
    }

    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials()
        {
            return new Credentials()
            {
                UserName = "username",
                Password = "password"
            };
        }

        public bool AuthenticationFailedRetryQuery()
        {
            return false;
        }
    }

    public static class ContainerBootstrapper
    {

        public static void BootstrapStructureMap()
        {

            // Initialize the static ObjectFactory container

            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<TestingInteractionContext>();
                x.ForRequestedType<ITwitterUtilities>().TheDefaultIsConcreteType<TestTwitterUtilities>();
            });

        }

    }
}
