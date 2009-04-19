using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwitterPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace TwitterPlugIn.Specs
{
    [TestFixture]
    public class When_asking_if_a_plugin_can_accept_a_credential_for_another_service
    {
        private TwitterUtilities Twitter;
        private ITwitterDataAccess TwitterDal;
        private bool CredentialsAccepted;

        [Test]
        public void The_plugin_should_reply_that_it_cant()
        {
            CredentialsAccepted.ShouldBeFalse();
        }

        [TestFixtureSetUp]
        public void Context()
        {
            TwitterDal = MockRepository.GenerateMock<ITwitterDataAccess>();
            Twitter = new TwitterUtilities(TwitterDal);

            BecauseOf();
        }

        private void BecauseOf()
        {
            // Just creating some arbitrary credentials here that
            // obviously don't come from Twitter plug in and thus
            // can't possibly be acceptable.
            CredentialsAccepted = Twitter.CanAccept(new Credentials()
                                  {
                                      UserName = "",
                                      Password = "",
                                      ServiceInformation = new ServiceInformation()
                                                               {
                                                                   ServiceID = Guid.NewGuid(),
                                                                   ServiceName = "Name",
                                                               }
                                  });
        }
    }
}
