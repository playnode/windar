using System.Collections.Generic;
using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class TupleToken : CompositeToken, IValueToken
    {
        public TupleToken()
        {
            Tokens = new List<ParserToken>();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append('{');
            foreach (ParserToken token in Tokens) 
                result.Append(token.ToString());
            result.Append('}');
            return result.ToString();
        }
    }
}
