using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Services;

namespace Unite.Messaging.Extras
{
    public interface IMessageFormatter
    {
        string ApplyFormatting(string message);
    }

    public class AutoFormatCodePastesAsUrls : IMessageFormatter
    {
        private ICodePaste _CodePasteService;

        public AutoFormatCodePastesAsUrls(ICodePaste codePasteService)
        {
            _CodePasteService = codePasteService;
        }

        public string ApplyFormatting(string message)
        {
            var result = message;
            if (message.Contains("\n") &&
                        (message.Contains("\t") || message.Contains("\n   ")))
                result = _CodePasteService.PasteCode(result);
            return result;
        }
    }
}
