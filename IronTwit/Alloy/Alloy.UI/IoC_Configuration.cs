using System.Threading;
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
                x.ForRequestedType<Views.MainView>().TheDefaultIsConcreteType<Views.MainView>();
                x.ForRequestedType<IMessagingServiceManager>().TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<IContactProvider>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<ContactProvider>();
                x.ForRequestedType<IServiceProvider>().TheDefaultIsConcreteType<ServiceProvider>();
                x.ForRequestedType<IPluginFinder>().TheDefaultIsConcreteType<PluginFinder>();
                x.ForRequestedType<ICodePaste>().TheDefaultIsConcreteType<CodePaste>();
                x.ForRequestedType<ICredentialCache>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<CredentialCache>();
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<GuiInteractionContext>();
                x.ForRequestedType<IMessageFormatter>().TheDefaultIsConcreteType<MessageFormatter>();
            });
        }
    }
}
