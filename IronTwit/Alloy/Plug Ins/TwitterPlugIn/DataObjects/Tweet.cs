using System;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace IronTwitterPlugIn.DataObjects
{
    public class Tweet : IMessage
    {
        /// <summary>
        /// for de/serialization only (alias to Text)
        /// </summary>
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        public string Text { get; set; }

        /// <summary>
        /// for de/serialization only (alias to Sender)
        /// </summary>
        public TwitterUser user
        {
            get { return (TwitterUser)Address; }
            set
            {
                if (!value.UserName.StartsWith("@"))
                    value.UserName = "@" + value.UserName;
                Address = value;
            }
        }
        public long id { get; set; }

        public string created_at
        {
            get
            {
                return TimeStamp.ToLongDateString();
            }
            set
            {
                TimeStamp = ParseDateTime(value);
            }
        }

        public IIdentity Address { get; set; }
        public DateTime TimeStamp
        {
            get; set;
        }

        public static DateTime ParseDateTime(string date)
        {
            string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }
    }

    
}