using System.Reflection;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class TupleParser : Parser<TupleToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public TupleParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            TupleOpen,
            SegmentBegin,
            SegmentEnd,
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

        internal static TupleToken GetTuple(ParserInputStream stream)
        {
            return new TupleParser(stream).NextToken();
        }

        public override TupleToken NextToken()
        {
            TupleToken result = new TupleToken();
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                switch (_state)
                {
                    case State.Initial:
                        {
                            switch ((char) c)
                            {
                                case '{':
                                    {
                                        ChangeState(State.TupleOpen);
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
                    case State.TupleOpen:
                        // NOTE: Same as SegmentBegin, except '}' is allowed here.
                        {
                            // Atom expression.
                            if (AtomParser.IsValidAtomFirstChar(c))
                            {
                                InputStream.PushBack(c);
                                result.Tokens.Add(AtomParser.GetAtom(InputStream));
                                ChangeState(State.SegmentEnd);
                                break;
                            }

                            // Numeric expression.
                            if (NumericExpressionParser.IsValidExpressionFirstChar(c))
                            {
                                InputStream.PushBack(c);
                                result.Tokens.Add(NumericExpressionParser.GetExpression(InputStream));
                                ChangeState(State.SegmentEnd);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '}':
                                    {
                                        return result;
                                    }
                                case '\'':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(AtomParser.GetAtom(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '"':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(StringParser.GetString(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '{':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(GetTuple(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '[':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(ListParser.GetList(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '%':
                                case ' ':
                                case '\t':
                                case '\n':
                                case '\r':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(WhitespaceParser.GetWhitespace(InputStream));
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
                    case State.SegmentBegin:
                        // NOTE: Same as TupleOpen, except '}' is NOT allowed here.
                        {
                            // Atom expression.
                            if (AtomParser.IsValidAtomFirstChar(c))
                            {
                                InputStream.PushBack(c);
                                result.Tokens.Add(AtomParser.GetAtom(InputStream));
                                ChangeState(State.SegmentEnd);
                                break;
                            }

                            // Numeric expression.
                            if (NumericExpressionParser.IsValidExpressionFirstChar(c))
                            {
                                InputStream.PushBack(c);
                                result.Tokens.Add(NumericExpressionParser.GetExpression(InputStream));
                                ChangeState(State.SegmentEnd);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '\'':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(AtomParser.GetAtom(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '"':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(StringParser.GetString(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '{':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(GetTuple(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '[':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(ListParser.GetList(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '%':
                                case ' ':
                                case '\t':
                                case '\n':
                                case '\r':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(WhitespaceParser.GetWhitespace(InputStream));
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
                    case State.SegmentEnd:
                        {
                            switch ((char) c)
                            {
                                case ',':
                                    {
                                        result.Tokens.Add(new CommaToken());
                                        ChangeState(State.SegmentBegin);
                                        break;
                                    }
                                case '}':
                                    {
                                        return result;
                                    }
                                case '%':
                                case ' ':
                                case '\t':
                                case '\n':
                                case '\r':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(WhitespaceParser.GetWhitespace(InputStream));
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
                    default:
                        {
                            string msg = GetUnexpectedStateErrorMessage(_state.ToString());
                            throw new ParserException(msg);
                        }
                }
            }

            const string endmsg = "Unexpected end while parsing Tuple. Partial token exception property.";
            if (Log.IsErrorEnabled) Log.Error(endmsg);
            throw new ParserException(endmsg, result);
        }
    }
}
