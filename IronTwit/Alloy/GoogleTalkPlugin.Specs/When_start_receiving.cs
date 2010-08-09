using GoogleTalkPlugIn;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Unite.Messaging.Entities;

namespace GoogleTalkPlugin.Specs
{
    [TestFixture]
    public class When_authenticating_and_user_provides_no_credentials
    {
        [Test]
        public void It_should_never_login_to_the_data_access_layer()
        {
            var fakeDataAccess = new TestGoogleTalkDataAccess();
            
            var plugin = new GoogleTalkMessagingService(fakeDataAccess);
            
            plugin.StartReceiving();

            Assert.That(fakeDataAccess.LoginWasRequested, Is.False);
        }


    }
}
