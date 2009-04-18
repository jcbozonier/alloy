﻿namespace Unite.Messaging.Services
{
    public class CodePaste : ICodePaste
    {
        public string PasteCode(string codeToPaste)
        {
            var paster = new OpenPasteAPI.OpenPaste
                             {
                                 Code = codeToPaste,
                                 AuthorName = "Alloy Messaging Client",

                             };
            paster.Save();
            
            return paster.Url;
        }
    }
}
