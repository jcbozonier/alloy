using IronTwitterPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace TwitterPlugIn.Specs
{
    [TestFixture]
    public class When_sending_a_tweet_that_is_too_large_for_a_single_message
    {
        [Test]
        public void It_should_split_the_tweet_into_multiple_acceptably_sized_messages()
        {
            NumberOfMessagesSent.ShouldEqual(2);
        }

        [Test]
        public void It_should_include_the_user_name_in_each_message()
        {
            FirstMessageSent.Contains("@darkxanthos").ShouldBeTrue();
        }

        [Test]
        public void It_should_split_the_messages_where_there_are_spaces()
        {
            FirstMessageSent.Contains("Firstmessage");
            SecondMessageSent.Contains("Secondmessage");
        }

        [TestFixtureSetUp]
        public void Context()
        {
            MockTweetService = MockRepository.GenerateMock<ITwitterDataAccess>();
            MockTweetService.Stub(x => x.SendMessage(null, null))
                .Callback<Credentials, string>(CountASentMessage)
                .Return("");

            TwitterDAL = new TwitterUtilities(MockTweetService, 30);
            MyIdentity = new Identity("@darkxanthos", TwitterDAL.GetInformation());

            BecauseOf();
        }

        public void BecauseOf()
        {
            TwitterDAL.SendMessage(MyIdentity, "Firstmessage Secondmessage");
        }

        private ITwitterDataAccess MockTweetService;
        private int NumberOfMessagesSent;
        private TwitterUtilities TwitterDAL;
        private Identity MyIdentity;
        private string FirstMessageSent;
        private string SecondMessageSent;

        private bool CountASentMessage(Credentials credentials, string messageToSend)
        {
            NumberOfMessagesSent++;
            if (NumberOfMessagesSent == 1)
                FirstMessageSent = messageToSend;
            if (NumberOfMessagesSent == 2)
                SecondMessageSent = messageToSend;
            return true;
        }
    }
}
