using System.Collections.Generic;
using System.Text;

namespace Windar.ErlangTermsParser
{
    public class CompositeToken : ParserToken
    {
        List<ParserToken> _tokens;

        public List<ParserToken> Tokens
        {
            get { return _tokens; }
            set { _tokens = value; }
        }

        public CompositeToken()
        {
            Tokens = new List<ParserToken>();
        }

        public List<IValueToken> GetValueTokens()
        {
            List<IValueToken> result = new List<IValueToken>();
            foreach (ParserToken token in Tokens)
            {
                if (token is IValueToken)
                {
                    result.Add((IValueToken) token);
                }
            }
            return result;
        }

        public List<StringToken> GetStringTokens()
        {
            List<StringToken> result = new List<StringToken>();
            foreach (IValueToken token in GetValueTokens())
            {
                if (token is StringToken)
                {
                    result.Add((StringToken) token);
                }
            }
            return result;
        }

        public List<TupleToken> GetTupleTokens()
        {
            List<TupleToken> result = new List<TupleToken>();
            foreach (IValueToken token in GetValueTokens())
            {
                if (token is TupleToken)
                {
                    result.Add((TupleToken) token);
                }
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (ParserToken token in Tokens) result.Append(token);
            return result.ToString();
        }
    }
}
