using System;
using System.Collections.Generic;
using Rhino.Mocks;
using StructureMap;
using StructureMap.Attributes;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IServiceProvider=Unite.Messaging.Services.IServiceProvider;

namespace Unite.Specs.FakeSpecObjects
{
    public class ScenarioRepository
    {
        public readonly IInteractionContext FakeUIContext;
        public readonly IPluginFinder FakePluginFinder;
        public readonly ISettingsProvider FakeSettings;
        public readonly IMessagingService FakeMessagePlugin;
        public readonly ICodePaste FakeCodeFormatter;

        public ScenarioRepository()
        {
            FakeUIContext = MockRepository.GenerateMock<IInteractionContext>();
            FakeSettings = MockRepository.GenerateMock<ISettingsProvider>();
            FakeCodeFormatter = MockRepository.GenerateMock<ICodePaste>();
            FakeMessagePlugin = MockRepository.GenerateMock<IMessagingService>();
            FakePluginFinder = MockRepository.GenerateMock<IPluginFinder>();
        }

        private void _ApplyDefaultStubs()
        {
            FakePluginFinder
                .Stub(x => x.GetAllPlugins())
                .Return(new []{typeof (IMessagingService)});
            FakeMessagePlugin
                .Stub(x => x.CanFind(null))
                .IgnoreArguments()
                .Return(true);
        }

        public ScenarioRepository(bool applyDefaultStubs):this()
        {
            if(applyDefaultStubs)
                _ApplyDefaultStubs();
        }

        public ScenarioRepository(IMessagingService plugin) : this()
        {
            FakeMessagePlugin = plugin;
        }

        public void InitializeIoC()
        {
            ContainerBootstrapper.BootstrapStructureMap(FakeUIContext, FakePluginFinder, FakeSettings, FakeMessagePlugin, FakeCodeFormatter);
        }

        public MainView GetMainView()
        {
            InitializeIoC();
            ObjectFactory.Inject(FakeMessagePlugin);
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

        public static ScenarioRepository CreateUnstubbedInstance()
        {
            return new ScenarioRepository(false);
        }

        public static ScenarioRepository CreateStubbedInstance()
        {
            return new ScenarioRepository(true);
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
            BootstrapStructureMap(gui, pluginFinder, settings, plugin, null);
        }

        public static void BootstrapStructureMap(IInteractionContext gui, IPluginFinder pluginFinder, ISettingsProvider settings, IMessagingService plugin, ICodePaste formatter)
        {
            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<MainView>().TheDefaultIsConcreteType<MainView>();
                x.ForRequestedType<IInteractionContext>().TheDefault.IsThis(gui);
                x.ForRequestedType<IMessagingService>().TheDefault.IsThis(plugin);
                x.ForRequestedType<ISettingsProvider>().TheDefault.IsThis(settings);
                x.ForRequestedType<IMessagingServiceManager>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<IContactService>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<ContactManager>().TheDefaultIsConcreteType<ContactManager>();
                x.ForRequestedType<IServiceProvider>().TheDefaultIsConcreteType<ServiceProvider>();
                x.ForRequestedType<IPluginFinder>().TheDefault.IsThis(pluginFinder);
                x.ForRequestedType<ICodePaste>().TheDefault.IsThis(formatter);
                x.ForRequestedType<IMessageFormatter>().TheDefaultIsConcreteType<MessageFormatter>();
                x.ForRequestedType<IJobRunner>().TheDefaultIsConcreteType<SynchronousJobRunner>();

            });
        }
    }
}