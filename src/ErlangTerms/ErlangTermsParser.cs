using System.Reflection;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    /// <summary>
    /// NOTE: This is a simple parser for extracting erlang terms 
    /// to allow for the Playdar configuration terms to be parsed and updated
    /// before being written back to file. This is to preserve manual edits.
    /// </summary>
    public class ErlangTermsParser : Parser<ParserToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public ErlangTermsParser(ParserInputStream stream) : base(stream) { }

        public override ParserToken NextToken()
        {
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                switch ((char) c)
                {
                    case '%':
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        {
                            InputStream.PushBack(c);
                            return WhitespaceParser.GetWhitespace(InputStream);
                        }
                    case '{':
                        {
                            InputStream.PushBack(c);
                            return TupleParser.GetTuple(InputStream);
                        }
                    case '.':
                        {
                            return new TermEndToken();
                        }
                    default:
                        {
                            string msg = GetEdgeUnknownErrorMessage(c, "DocumentRoot");
                            if (Log.IsErrorEnabled) Log.Error(msg);
                            throw new ParserException(msg);
                        }
                }
            }
            return null;
        }
    }
}
