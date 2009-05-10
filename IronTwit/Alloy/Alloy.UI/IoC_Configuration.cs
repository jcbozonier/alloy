using System.Threading;
using System.Windows.Threading;
using StructureMap.Attributes;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using StructureMap;
using Unite.Messaging;

namespace Unite.UI
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                var asyncJobRunner = new AsyncJobRunner(Dispatcher.CurrentDispatcher);

                x.ForRequestedType<Views.MainView>().TheDefaultIsConcreteType<Views.MainView>();
                x.ForRequestedType<IMessagingServiceManager>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<IContactService>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<ContactManager>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<ContactManager>();
                x.ForRequestedType<IServiceProvider>().TheDefaultIsConcreteType<ServiceProvider>();
                x.ForRequestedType<IPluginFinder>().TheDefaultIsConcreteType<PluginFinder>();
                x.ForRequestedType<ICodePaste>().TheDefaultIsConcreteType<CodePaste>();
                x.ForRequestedType<ICredentialCache>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<CredentialCache>();
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<GuiInteractionContext>();
                x.ForRequestedType<IMessageFormatter>().TheDefaultIsConcreteType<MessageFormatter>();
                x.ForRequestedType<IJobRunner>().TheDefault.IsThis(asyncJobRunner);
            });
        }
    }
}
