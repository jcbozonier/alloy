using System;

namespace Unite.UI.Utilities
{
    public class InlineUri : Uri
    {
        public int StartIndex { get; private set; }
        public int Length { get; private set; }

        public InlineUri(string uriString, int startIndex, int length) : base(uriString)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }
}