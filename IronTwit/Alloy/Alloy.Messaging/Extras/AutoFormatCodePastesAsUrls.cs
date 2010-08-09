using System;
using Unite.Messaging.Services;

namespace Unite.Messaging.Extras
{
    public class AutoFormatCodePastesAsUrls : IMessageFormatter
    {
        private readonly ICodePaste _CodePasteService;
        private IUnifiedMessagingController _SendMessageObserver;

        public AutoFormatCodePastesAsUrls(ICodePaste codePasteService)
        {
            _CodePasteService = codePasteService;
        }

        public void MessageToSend(string recipient, string message)
        {
            var result = message;
            if (message.Contains("\n") &&
                        (message.Contains("\t") || message.Contains("\n   ")))
                result = _CodePasteService.PasteCode(result);

            _SendMessageObserver.MessageToSend(recipient, result);
        }

        public void OnMessageToSendNotify(IUnifiedMessagingController unifiedMessagingController)
        {
            _SendMessageObserver = unifiedMessagingController;
        }
    }
}
