using Unite.Messaging;
using Unite.Messaging.Entities;

namespace IronTwitterPlugIn
{
    public interface ITwitterDataAccess
    {
        /// <summary>
        /// Returns a status in JSON.
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string SendMessage(Credentials credentials, string message);
        string GetMessages(Credentials credentials);
        string GetContacts(Credentials credentials);
    }
}
