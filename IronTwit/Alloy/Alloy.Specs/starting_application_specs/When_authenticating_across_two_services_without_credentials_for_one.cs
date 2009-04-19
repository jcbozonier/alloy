using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.starting_application_specs
{
    [TestFixture]
    public class When_initiatializing_two_services_that_require_authentication : authenticating_two_services_context
    {
        [Test]
        public void Both_services_should_ask_to_authenticate()
        {
            TwitterPluginAuthenticated.ShouldBeTrue();
            GTalkPluginAuthenticated.ShouldBeTrue();
        }

        protected override void BecauseOf()
        {
            View.Init();
        }

        protected override void Context()
        {
            
        }
    }

    public abstract class authenticating_two_services_context
    {
        protected ScenarioRepository ScenarioRepo;
        protected MainView ViewModel;
        protected FakeTwitterPlugin FakeTwitter;
        protected FakeGTalkPlugin FakeGTalk;
        protected MainView View;

        protected bool TwitterPluginAuthenticated;
        protected bool GTalkPluginAuthenticated;

        [Test]
        public void Setup()
        {
            var fakesRepo = ScenarioRepository.CreateUnstubbedInstance();
            
            fakesRepo.FakePluginFinder
                .Stub(x => x.GetAllPlugins())
                .Return(new[] { typeof(FakeTwitterPlugin), typeof(FakeGTalkPlugin) });

            fakesRepo.FakeUIContext.Stub(x => x.GetCredentials(null))
                .Callback<IServiceInformation>(Foo)
                .Return(new Credentials());

            FakeTwitter = new FakeTwitterPlugin();
            FakeGTalk = new FakeGTalkPlugin();

            fakesRepo.InitializeIoC();
            ObjectFactory.Inject(FakeTwitter);
            ObjectFactory.Inject(FakeGTalk);

            View = fakesRepo.GetMainViewDontIoC();

            Context();
            BecauseOf();
        }

        private bool Foo(IServiceInformation serviceInfo)
        {
            if(serviceInfo.ServiceName == "FakeTwitterPlugin")
                TwitterPluginAuthenticated = true;
            if (serviceInfo.ServiceName == "FakeGTalkPlugin")
                GTalkPluginAuthenticated = true;

            return true;
        }

        protected abstract void BecauseOf();

        protected abstract void Context();
    }

    public class FakeTwitterPlugin : FakePlugin
    {
        public string MessageSent;
        public override Unite.Messaging.Entities.ServiceInformation GetInformation()
        {
            return new ServiceInformation() {ServiceName = "FakeTwitterPlugin"};
        }

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }

    public class FakeGTalkPlugin : FakePlugin
    {
        public string MessageSent;

        public override Unite.Messaging.Entities.ServiceInformation GetInformation()
        {
            return new ServiceInformation() { ServiceName = "FakeGTalkPlugin" };
        }

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }
}
