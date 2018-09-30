using System.Reflection;
using System.Text;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    public class AtomParser : Parser<AtomToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public AtomParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            AtomName,
            OpenQuote,
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

        internal static AtomToken GetAtom(ParserInputStream stream)
        {
            return new AtomParser(stream).NextToken();
        }

        public override AtomToken NextToken()
        {
            StringBuilder buffer = new StringBuilder();
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                switch (_state)
                {
                    case State.Initial:
                        {
                            if (IsValidAtomFirstChar(c))
                            {
                                InputStream.PushBack(c);
                                ChangeState(State.AtomName);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '\'':
                                    {
                                        buffer.Append((char) c);
                                        ChangeState(State.OpenQuote);
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
                    case State.AtomName:
                        {
                            if (IsValidAtomChar(c))
                            {
                                buffer.Append((char) c);
                                break;
                            }

                            switch ((char) c)
                            {
                                case ',':
                                case '}':
                                case '%':
                                case ' ':
                                case '\t':
                                case '\n':
                                case '\r':
                                    {
                                        InputStream.PushBack(c);
                                        return GetAtom(buffer.ToString());
                                    }
                                default:
                                    {
                                        string msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
                                        if (Log.IsErrorEnabled) Log.Error(msg);
                                        throw new ParserException(msg);
                                    }
                            }
                        }
                    case State.OpenQuote:
                        {
                            if (IsValidQuotedAtomChar(c))
                            {
                                buffer.Append((char) c);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '\'':
                                    {
                                        buffer.Append((char) c);
                                        return GetAtom(buffer.ToString());
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

            const string endmsg = "Unexpected end while parsing NumericExpression.";
            if (Log.IsErrorEnabled) Log.Error(endmsg);
            throw new ParserException(endmsg);
        }

        private static AtomToken GetAtom(string str)
        {
            AtomToken result;
            switch (str)
            {
                case "true":
                    result = new BooleanToken(true);
                    break;
                case "false":
                    result = new BooleanToken(false);
                    break;
                default:
                    result = new AtomToken(str);
                    break;
            }
            return result;
        }

        internal static bool IsValidAtomFirstChar(int c)
        {
            return c > 96 && c < 123; // Lower-case letters.
        }

        private static bool IsValidAtomChar(int c)
        {
            return (c > 96 && c < 123)  // Lower-case letters.
                || (c > 63 && c < 91)   // Upper-case letters and '@' char.
                || (((char) c) == '_'); // Underscore.
        }

        private static bool IsValidQuotedAtomChar(int c)
        {
            //TODO: Check what characters are disallowed in quoted atom.
            return ((char) c) != '\'';
        }
    }
}
