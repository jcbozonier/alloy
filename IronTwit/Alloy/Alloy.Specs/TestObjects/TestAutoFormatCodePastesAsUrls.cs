using Unite.Messaging.Services;

namespace Unite.Specs.TestObjects
{
    public class TestCodePasteService : ICodePaste
    {
        public string CodePasteText = "yada yada yada";

        public string PasteCode(string codeToPaste)
        {
            return CodePasteText;
        }
    }
}
