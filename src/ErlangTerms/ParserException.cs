using System;

namespace Windar.ErlangTermsParser
{
    public class ParserException : Exception
    {
        ParserToken _incompleteToken;

        public ParserToken IncompleteToken
        {
            get { return _incompleteToken; }
            set { _incompleteToken = value; }
        }

        public ParserException() { }
        public ParserException(string msg) : base(msg) { }
        public ParserException(string msg, Exception ex) : base(msg, ex) { }
        public ParserException(string msg, ParserToken partialToken) : base(msg) { IncompleteToken = partialToken; }
    }
}
