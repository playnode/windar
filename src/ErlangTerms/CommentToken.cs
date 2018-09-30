using System.Text;

namespace Windar.ErlangTermsParser
{
    public class CommentToken : WhitespaceToken
    {
        public CommentToken()
        {
            
        }

        public CommentToken(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append('%').Append(Text).Append("\n");
            return result.ToString();
        }
    }
}
