using System.Collections.Generic;
using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class ListToken : CompositeToken, IValueToken
    {
        public ListToken()
        {
            Tokens = new List<ParserToken>();
        }

        public ListToken(List<ParserToken> list)
        {
            Tokens = list;
        }

        public int CountValues()
        {
            int result = 0;
            foreach (ParserToken token in Tokens)
                if (token is IValueToken) result++;
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append('[');
            foreach (ParserToken token in Tokens)
                result.Append(token.ToString());
            result.Append(']');
            return result.ToString();
        }
    }
}
