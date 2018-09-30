using System.Reflection;
using System.Text;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class WhitespaceParser : Parser<WhitespaceToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public WhitespaceParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            Comment,
            WhiteSpace,
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

        internal static WhitespaceToken GetWhitespace(ParserInputStream stream)
        {
            return new WhitespaceParser(stream).NextToken();
        }

        public override WhitespaceToken NextToken()
        {
            int c;
            StringBuilder buffer = new StringBuilder();
            while ((c = InputStream.NextChar()) != -1)
            {
                switch (_state)
                {
                    case State.Initial:
                        {
                            switch ((char) c)
                            {
                                case '%':
                                    {
                                        ChangeState(State.Comment);
                                        break;
                                    }
                                case ' ':
                                case '\t':
                                case '\n':
                                case '\r':
                                    {
                                        InputStream.PushBack(c);
                                        ChangeState(State.WhiteSpace);
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
                    case State.Comment:
                        {
                            switch ((char) c)
                            {
                                case '\r':
                                    {
                                        // Discard.
                                        break;
                                    }
                                case '\n':
                                    {
                                        return new CommentToken(buffer.ToString());
                                    }
                                default:
                                    {
                                        buffer.Append((char) c);
                                        break;
                                    }
                            }
                            break;
                        }
                    case State.WhiteSpace:
                        {
                            switch ((char) c)
                            {
                                case '\r':
                                    {
                                        // Discard.
                                        break;
                                    }
                                case ' ':
                                case '\t':
                                case '\n':
                                    {
                                        buffer.Append((char) c);
                                        break;
                                    }
                                default:
                                    {
                                        InputStream.PushBack(c);
                                        return new WhitespaceToken(buffer.ToString());
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            string msg = GetUnexpectedStateErrorMessage(_state.ToString());
                            throw new ParserException(msg);
                        }
                }
            }

            // End of file.
            return null;
        }
    }
}
