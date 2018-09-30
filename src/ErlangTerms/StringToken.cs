using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class StringToken : ParserToken, IValueToken
    {
        string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public StringToken()
        {
            
        }

        public StringToken(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append('"').Append(Text).Append('"').ToString();
        }
    }
}
