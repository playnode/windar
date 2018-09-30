using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class WhitespaceToken : ParserToken
    {
        string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public WhitespaceToken()
        {
            
        }

        public WhitespaceToken(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }

        public string ToEscapedString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("\"");
            foreach (char c in Text)
            {
                switch (c)
                {
                    case ' ':
                        {
                            result.Append(' ');
                            break;
                        }
                    case '\t':
                        {
                            result.Append("\\t");
                            break;
                        }
                    case '\r':
                        {
                            result.Append("\\r");
                            break;
                        }
                    case '\n':
                        {
                            result.Append("\\n");
                            break;
                        }
                    case '"':
                        {
                            result.Append("\\\"");
                            break;
                        }
                    default:
                        result.Append(c);
                        break;
                }
            }
            result.Append("\" (").Append(Text.Length).Append(' ');
            if (Text.Length == 1) result.Append("char)");
            else result.Append("chars)");
            return result.ToString();
        }
    }
}
