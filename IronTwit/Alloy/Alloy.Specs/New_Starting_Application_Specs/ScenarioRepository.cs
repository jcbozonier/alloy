﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IServiceProvider=Unite.Messaging.Services.IServiceProvider;

namespace Unite.Specs.New_Starting_Application_Specs
{
    public class ScenarioRepository
    {
        public readonly IInteractionContext FakeContext;
        public readonly IPluginFinder FakePluginFinder;
        public readonly ISettingsProvider FakeSettings;
        public readonly IMessagingService FakeMessagePlugin;

        public ScenarioRepository()
        {
            FakeContext = MockRepository.GenerateMock<IInteractionContext>();
            FakePluginFinder = MockRepository.GenerateMock<IPluginFinder>();
            FakeSettings = MockRepository.GenerateMock<ISettingsProvider>();
            FakeMessagePlugin = MockRepository.GenerateMock<IMessagingService>();
        }

        public ScenarioRepository(IMessagingService plugin) : this()
        {
            FakeMessagePlugin = plugin;
        }

        public void InitializeIoC()
        {
            ContainerBootstrapper.BootstrapStructureMap(FakeContext, FakePluginFinder, FakeSettings, FakeMessagePlugin);
        }

        public MainView GetMainView()
        {
            InitializeIoC();
            return ObjectFactory.GetInstance<MainView>();
        }

        public MainView GetMainViewDontIoC()
        {
            return ObjectFactory.GetInstance<MainView>();
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>
                       {
                           GetMessage(),
                           GetMessage()
                       };
        }

        public List<IMessage> GetMessages(DateTime timeStamp)
        {
            return new List<IMessage>
                       {
                           GetMessage(timeStamp),
                           GetMessage(timeStamp)
                       };
        }

        public IMessage GetMessage()
        {
            return new Message()
                       {
                           Address = new Address(),
                           Text = "Fake message",
                           TimeStamp = DateTime.MinValue
                       };
        }

        public IMessage GetMessage(DateTime timeStamp)
        {
            return new Message()
            {
                Address = new Address(),
                Text = "Fake message",
                TimeStamp = timeStamp
            };
        }

        public Credentials CreateFakeCredentials()
        {
            return new Credentials
                       {
                           ServiceInformation = new ServiceInformation()
                                                    {
                                                        ServiceID = Guid.NewGuid(),
                                                        ServiceName = "Fake"
                                                    }
                       };
        }
    }

    public class Address : IIdentity
    {
        public string UserName
        {
            get; set;
        }

        public ServiceInformation ServiceInfo
        {
            get; set;
        }
    }

    public class Message : IMessage
    {
        public string Text
        {
            get; set;
        }

        public IIdentity Address
        {
            get; set;
        }

        public DateTime TimeStamp
        {
            get; set;
        }
    }

    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap(
            IInteractionContext gui, 
            IPluginFinder pluginFinder, 
            ISettingsProvider settings, 
            IMessagingService plugin)
        {
            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<MainView>().TheDefaultIsConcreteType<MainView>();
                x.ForRequestedType<IInteractionContext>().TheDefault.IsThis(gui);
                x.ForRequestedType<IMessagingService>().TheDefault.IsThis(plugin);
                x.ForRequestedType<ISettingsProvider>().TheDefault.IsThis(settings);
                x.ForRequestedType<IMessagingServiceManager>().TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<IContactProvider>().TheDefaultIsConcreteType<ContactProvider>();
                x.ForRequestedType<IServiceProvider>().TheDefaultIsConcreteType<ServiceProvider>();
                x.ForRequestedType<IPluginFinder>().TheDefault.IsThis(pluginFinder);
            });
        }
    }
}
