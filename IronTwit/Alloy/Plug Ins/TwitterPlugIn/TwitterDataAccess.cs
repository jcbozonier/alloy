using Unite.Messaging;
using Unite.Messaging.Entities;
using Yedda;

namespace IronTwitterPlugIn
{
    public class TwitterDataAccess : ITwitterDataAccess
    {
        public string SendMessage(Credentials credentials, string message)
        {
            var twit = new Twitter();
            twit.TwitterClient = "Alloy";
            twit.TwitterClientUrl = "http://github.com/jcbozonier/irontwit/tree/master";
            twit.TwitterClientVersion = "0.8";

            var result = twit.UpdateAsJSON(credentials.UserName, credentials.Password, message);
            return result;
        }

        public string GetMessages(Credentials credentials)
        {
            var twit = new Twitter();
            twit.TwitterClient = "Alloy";
            twit.TwitterClientUrl = "http://github.com/jcbozonier/irontwit/tree/master";
            twit.TwitterClientVersion = "0.8";

            var result = twit.GetFriendsTimelineAsJSON(credentials.UserName, credentials.Password);
            return result;
        }

        public string GetContacts(Credentials credentials)
        {
            var twit = new Twitter();
            twit.TwitterClient = "Alloy";
            twit.TwitterClientUrl = "http://github.com/jcbozonier/irontwit/tree/master";
            twit.TwitterClientVersion = "0.8";

            var result = twit.GetFriendsAsJSON(credentials.UserName, credentials.Password);
            return result;
        }
    }
}
