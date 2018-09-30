using System;
using System.Reflection;
using System.Text;
using log4net;

namespace Windar.ErlangTermsParser
{
    /// <summary>
    /// Simple number values are only supported at the moment. Potentially
    /// there would be support for numerical expressions, later.
    /// </summary>
    public class NumericExpressionParser : Parser<NumericExpression>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public NumericExpressionParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            Integer,
            Float,
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

        internal static NumericExpression GetExpression(ParserInputStream stream)
        {
            return new NumericExpressionParser(stream).NextToken();
        }

        public override NumericExpression NextToken()
        {
            StringBuilder buffer = new StringBuilder();
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                switch (_state)
                {
                    case State.Initial:
                        {
                            if (IsValidDigit(c))
                            {
                                InputStream.PushBack(c);
                                ChangeState(State.Integer);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '(':
                                    {
                                        throw new NotImplementedException();
                                    }
                                default:
                                    {
                                        string msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
                                        if (Log.IsErrorEnabled) Log.Error(msg);
                                        throw new ParserException(msg);
                                    }
                            }
                        }
                    case State.Integer:
                        {
                            if (IsValidDigit(c))
                            {
                                buffer.Append((char) c);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '.':
                                    {
                                        buffer.Append((char) c);
                                        ChangeState(State.Float);
                                        break;
                                    }
                                default:
                                    {
                                        InputStream.PushBack(c);
                                        return new IntegerToken(buffer.ToString());
                                    }
                            }
                            break;
                        }
                    case State.Float:
                        {
                            if (IsValidDigit(c))
                            {
                                buffer.Append((char) c);
                                break;
                            }

                            switch ((char) c)
                            {
                                default:
                                    {
                                        InputStream.PushBack(c);
                                        return new FloatToken(buffer.ToString());
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

        private static bool IsValidDigit(int c)
        {
            return c > 47 && c < 58; // Digits 0-9.
        }

        internal static bool IsValidExpressionFirstChar(int c)
        {
            return IsValidDigit(c) || (c == '(');
        }
    }
}
