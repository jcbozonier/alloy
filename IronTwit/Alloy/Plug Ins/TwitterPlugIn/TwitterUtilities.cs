﻿using System.Net;
using System.Security.Authentication;
using System.Threading;
using IronTwitterPlugIn.DataObjects;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace IronTwitterPlugIn
{
    [StructureMap.Pluggable("Complex")]
    public class TwitterUtilities : IMessagingService
    {
        private readonly int MaxMessageLength = 140;
        private ITwitterDataAccess _DataAccess;

        public static readonly Guid SERVICE_ID = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B565D7}");
        public static readonly string SERVICE_NAME = "Twitter";

        private Credentials _UserCredentials;

        public Guid ServiceId { get { return SERVICE_ID; } }
        public string ServiceName { get { return SERVICE_NAME; } }

        private readonly ServiceInformation _ServiceInformation;
            /// <summary>
        /// This constructor should only be used if Twitter changes their default max
        /// message length. Until that time, this is only for testing.
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="maxMessageLength"></param>
        public TwitterUtilities(ITwitterDataAccess dataAccess, int maxMessageLength)
            : this(dataAccess)
        {
                MaxMessageLength = maxMessageLength;
        }
        
        [DefaultConstructor]
        public TwitterUtilities() : this(null)
        {
            
        }

        [DefaultConstructor]
        public TwitterUtilities(ITwitterDataAccess dataAccess)
        {
            // .NET Twitter fix for HTTP Error 417 with Twitter
            ServicePointManager.Expect100Continue = false;

            _DataAccess = dataAccess; //testing path
            
            _ServiceInformation = new ServiceInformation()
            {
                ServiceID = SERVICE_ID,
                ServiceName = SERVICE_NAME
            };

            if (dataAccess == null) //default path
                _DataAccess = new TwitterDataAccess();

            _ReceivedTweets = new Dictionary<int, bool>();
        }

        public void SendMessage(IIdentity theRecipient, string message)
        {
            _RequestCredentials();

            var recipient = theRecipient.UserName;

            // Get the maximum length of a message subtracting the to field
            // go to the maxLength - 1 index of the message and search backwards for a space or end of string.
            // tag this index as the end index
            // pull message from start index to end index into message variable
            // tag on the recipient
            // send message
            // repeat until the endIndex == messageLength-1.

            var maxLengthOfMessageContent = (!string.IsNullOrEmpty(recipient))
                                                ? MaxMessageLength - (recipient.Length + 1) //For space below
                                                : MaxMessageLength;

            var recipientMessagePortion = (!String.IsNullOrEmpty(recipient))
                                              ? recipient + " "
                                              : "";

            var startMessageIndex = 0;
            var endMessageIndex = Math.Min(maxLengthOfMessageContent - 1, message.Length-1);

            var messagesToSend = new List<string>();

            while (startMessageIndex < message.Length - 1)
            {
                if(startMessageIndex >= message.Length ||
                    startMessageIndex > endMessageIndex)
                    break;

                if( message[endMessageIndex] == ' ' || 
                    endMessageIndex >= message.Length-1)
                {
                    // Then this is the message portion we want.
                    messagesToSend.Add(
                        recipientMessagePortion + message.Substring(startMessageIndex, endMessageIndex - startMessageIndex + 1));

                    startMessageIndex = endMessageIndex + 1;
                    endMessageIndex = Math.Min(startMessageIndex + maxLengthOfMessageContent - 1, message.Length-1);
                }
                else
                {
                    endMessageIndex--;
                }
                
            }

            messagesToSend.ForEach((messageToSend) => _DataAccess.SendMessage(_UserCredentials, messageToSend));
        }

        public void SetCredentials(Credentials credentials)
        {
            _UserCredentials = credentials;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        public bool CanFind(string address)
        {
            if (string.IsNullOrEmpty(address))
                return true;
            if (address.Trim() == String.Empty)
                return true;
            if (address.StartsWith("@"))
                return true;
            return false;
        }

        public ServiceInformation GetInformation()
        {
            return _ServiceInformation;
        }

        public bool CanAccept(Credentials credentials)
        {
            return credentials.ServiceInformation.ServiceID.Equals(GetInformation().ServiceID);
        }

        private bool _StopReceiving;

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
        public void StartReceiving()
        {
            _StopReceiving = false;

            var receivingThread = new Thread(() =>
                                                 {
                                                     while (!_StopReceiving)
                                                     {
                                                         
                                                         var messages = GetMessages();
                                                         if (MessagesReceived != null)
                                                             MessagesReceived(this, new MessagesReceivedEventArgs(messages));
                                                         
                                                         // 100 api calls over an hour.
                                                         Thread.Sleep(60*60*1000/99);
                                                     }
                                                 });
            receivingThread.Start();
        }

        public void StopReceiving()
        {
            _StopReceiving = true;
        }

        public List<IMessage> GetMessages()
        {
            var tweets = new List<Tweet>();

            _RequestCredentials();

            try
            {
                var resultString = _DataAccess.GetMessages(_UserCredentials);

                var str = new StringReader(resultString);
                var converter = new JsonSerializer
                                    {
                                        MissingMemberHandling = MissingMemberHandling.Ignore
                                    };

                // Convert the sender property to proper twitter form.
                tweets = (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));

                _MessagesHaveBeenReceivedBefore = _MessagesHaveBeenReceivedBefore || tweets.Count > 0;
            }
            catch (WebException err)
            {
                //Swallow errors because Twitter crashes more than a fighter pilot with parkinson's.
                if(err.Message.Contains("(400)") ||
                   err.Message.Contains("(502)"))
                    return new List<IMessage>();

                // Those credentials suck apparently.
                _UserCredentials = null;
                // Let everyone know how much they suck.
                if(AuthorizationFailed != null && _MessagesHaveBeenReceivedBefore)
                    AuthorizationFailed(this, new CredentialEventArgs(){ServiceInfo = _ServiceInformation});
            }

            tweets.RemoveAll(tweet => _ReceivedTweets.ContainsKey(tweet.id));
            tweets.ForEach(tweet=>
                               {
                                   if(!_ReceivedTweets.ContainsKey(tweet.id))
                                   {
                                       _ReceivedTweets.Add(tweet.id, true);
                                   }
                               });

            return new List<IMessage>(tweets.ToArray());
        }

        private Dictionary<int,bool> _ReceivedTweets;
        private bool _MessagesHaveBeenReceivedBefore;

        private void _RequestCredentials()
        {
            if (_UserCredentials == null && CredentialsRequested != null)
                CredentialsRequested(this, new CredentialEventArgs()
                {
                    ServiceInfo = _ServiceInformation
                });
        }
    }
}
