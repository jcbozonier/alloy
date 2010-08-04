using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Unite.UI.Utilities
{
    public static class InlineUris
    {
        //http://www.regexguru.com/2008/11/detecting-urls-in-a-block-of-text/
        private const string URI_REGEX =
            @"\b(?:(?:https?|ftp|file)://|www\.|ftp\.)"
          + @"(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*"
          + @"(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])";// (free-spacing, case insensitive)

        private static Regex _regex = new Regex(URI_REGEX, RegexOptions.IgnoreCase);

        public static List<InlineUri> Get(string text)
        {
            var uris = new List<InlineUri>();

            if (string.IsNullOrEmpty(text))
                return uris;

            Match match;
            int index = 0;
            while (true)
            {
                match = _regex.Match(text, index);
                if (match == Match.Empty)
                    break;

                var uri = text.Substring(match.Index, match.Length);

                // #TODO: Add test... Without the below line the following tweet crashes the app:
                // "RT @bhalchander,@adnanmahmud: looking for folks to help us test our beta site www.jolkona.org pls RT"
                if(!uri.Contains("http://"))
                    uri = "http://" + uri;

                uris.Add(new InlineUri(uri, match.Index, match.Length));
                index = match.Index + match.Length;
            };

            return uris;
        }
    }
}
