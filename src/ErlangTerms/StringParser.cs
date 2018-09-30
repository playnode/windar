using System.Reflection;
using System.Text;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class StringParser : Parser<StringToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public StringParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            StringOpen,
        }

        private State _state = State.Initial;

        private void ChangeState(State to)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(GetStateChangeMessage(_state.ToString(), to.ToString()));
            }
            _state = to;
        }

        #endregion

        internal static StringToken GetString(ParserInputStream stream)
        {
            return new StringParser(stream).NextToken();
        }

        public override StringToken NextToken()
        {
            StringBuilder buffer = new StringBuilder();
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("StringParser: c = " + ParserInputStream.ToNameString(c));
                }
                switch (_state)
                {
                    case State.Initial:
                        {
                            switch ((char) c)
                            {
                                case '"':
                                    {
                                        ChangeState(State.StringOpen);
                                        break;
                                    }
                                default:
                                    {
                                        string msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
                                        if (Log.IsErrorEnabled) Log.Error(msg);
                                        throw new ParserException(msg);
                                    }
                            }
                            break;
                        }
                    case State.StringOpen:
                        {
                            if (IsValidStringValueCharacter(c))
                            {
                                buffer.Append((char) c);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '"':
                                    {
                                        return new StringToken(buffer.ToString());
                                    }
                                default:
                                    {
                                        string msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
                                        if (Log.IsErrorEnabled) Log.Error(msg);
                                        throw new ParserException(msg);
                                    }
                            }
                        }
                    default:
                        {
                            string msg = GetUnexpectedStateErrorMessage(_state.ToString());
                            throw new ParserException(msg);
                        }
                }
            }

            const string endmsg = "Unexpected end while parsing String.";
            if (Log.IsErrorEnabled) Log.Error(endmsg);
            throw new ParserException(endmsg);
        }

        internal static bool IsValidStringValueCharacter(int c)
        {
            //TODO: Check what characters are disallowed in string value.
            return ((char) c) != '"';
        }
    }
}
