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
                var fiber = new AsyncFiber(Dispatcher.CurrentDispatcher);

                x.ForRequestedType<Views.MessagingWindow>().TheDefaultIsConcreteType<Views.MessagingWindow>();
                x.ForRequestedType<IUnifiedMessagingService>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<UnifiedMessenger>();
                x.ForRequestedType<IContactService>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<UnifiedMessenger>();
                x.ForRequestedType<IServiceProvider>().TheDefaultIsConcreteType<MessagingPlugInRepository>();
                x.ForRequestedType<IPluginFinder>().TheDefaultIsConcreteType<MessagingPluginFinder>();
                x.ForRequestedType<ICodePaste>().TheDefaultIsConcreteType<CodePasteToUrlService>();
                x.ForRequestedType<ICredentialCache>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<MessagingAccountCredentialRepository>();
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<SecurityDialogService>();
                x.ForRequestedType<IMessageFormatter>().TheDefaultIsConcreteType<AutoFormatCodePastesAsUrls>();
                x.ForRequestedType<IFiber>().TheDefault.IsThis(fiber);
            });
        }
    }
}
