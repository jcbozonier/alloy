using System;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.FakeSpecObjects
{
    public static class ScenarioExtensions
    {
        
        public static IMessagingService Assume_it_can_find_any_address(this IMessagingService plugin)
        {
            plugin.Stub(x => x.CanFind(null))
                .IgnoreArguments()
                .Return(true);
            return plugin;
        }

        public static IPluginFinder Assume_a_single_messaging_service_is_found(this IPluginFinder finder)
        {
            finder.Stub(x => x.GetAllPlugins())
                .Return(new[] { typeof(IMessagingService) });

            return finder;
        }

        public static IMessagingService Assume_a_message_will_be_sent_and_deliver_the_arguments_to(this IMessagingService plugin, Action<IIdentity, string> callback)
        {
            plugin.Stub(x => x.SendMessage(null, null))
                .Callback<IIdentity, string>((x, y) =>
                {
                    callback(x, y);
                    return true;
                });

            return plugin;
        }

        /// <summary>
        /// This method doesn't care what credentials are provided as long as **something** is.
        /// </summary>
        /// <param name="gui"></param>
        public static IInteractionContext Assume_some_credential_is_provided(this IInteractionContext gui)
        {
            gui.Stub(x => x.GetCredentials(null))
                .Return(new ScenarioRepository().CreateFakeCredentials());
            return gui;
        }

        public static IInteractionContext Assume_valid_credentials_are_provided_for_the_correct_service(this IInteractionContext gui)
        {
            gui.Stub(x => x.GetCredentials(null))
                .IgnoreArguments()
                .Return(null)
                .WhenCalled(x =>
                                {
                                    var info = (ServiceInformation) x.Arguments[0];
                                    x.ReturnValue = new Credentials()
                                                        {
                                                            ServiceInformation = info,
                                                            UserName = "Test user",
                                                            Password = "Test pw"
                                                        };
                                });
            return gui;
        }

        public static IPluginFinder Assume_that_two_different_plugins_are_found(this IPluginFinder finder)
        {
            finder.Stub(x => x.GetAllPlugins())
                .Return(new[] { typeof(FakeTwitterPlugin), typeof(FakeGTalkPlugin) });
            return finder;
        }

        public static void Assume_code_was_pasted_and_return(this ICodePaste codePaster, string codeSampleUrl)
        {
            codePaster.Stub(x => x.PasteCode(null))
                .Constraints(Is.Anything())
                .Return(codeSampleUrl);
        }

        public static void Assume_these_plugins_were_found(this IPluginFinder finder, params Type[] types)
        {
            finder
                .Stub(x => x.GetAllPlugins())
                .Return(types);
        }
    }

    public class FakeTwitterPlugin : FakePlugin
    {
        public string MessageSent;

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }

    public class FakeGTalkPlugin : FakePlugin
    {
        public string MessageSent;

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }
}

