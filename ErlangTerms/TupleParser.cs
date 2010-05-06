/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System.Reflection;
using log4net;

namespace Playnode.ErlangTerms.Parser
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
            var result = new TupleToken();
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
                                        if (Log.IsErrorEnabled) Log.Error(msg);
                                        throw new ParserException(msg);
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            var msg = GetUnexpectedStateErrorMessage(_state.ToString());
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
