using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleTalkPlugIn;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace GoogleTalkPlugin.Specs
{
    [TestFixture]
    public class AuthenticatedGoogleTalkDataAccess_Tests
    {
        [Test]
        public void When_sending_a_message_and_not_logged_in()
        {
            var googleTalkDataAccessSpy = new TestGoogleTalkDataAccess();
            var sut = new AuthenticatedGoogleTalkDataAccess(googleTalkDataAccessSpy);

            sut.Message("recipient", "message");

            Assert.That(googleTalkDataAccessSpy.SentMessages, Is.Empty, "It should not send any messages.");
        }

        [Test]
        public void When_logging_in_after_sending_a_message_while_not_logged_in()
        {
            var googleTableDataAccessSpy = new TestGoogleTalkDataAccess();
            var sut = new AuthenticatedGoogleTalkDataAccess(googleTableDataAccessSpy);

            var expectedSentMessages = new List<KeyValuePair<string, string>>()
                                           {
                                               new KeyValuePair<string, string>("recipient 1", "message 1"),
                                               new KeyValuePair<string, string>("recipient 2", "message 2")
                                           };

            sut.Message(expectedSentMessages[0].Key, expectedSentMessages[0].Value);
            sut.Message(expectedSentMessages[1].Key, expectedSentMessages[1].Value);

            sut.Login("name", "password");

            Assert.That(googleTableDataAccessSpy.SentMessages, Is.EquivalentTo(expectedSentMessages), "It should send all messages sent before logging in.");
        }
    }
}
