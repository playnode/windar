/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Reflection;
using log4net;
using Windar.TrayApp.Configuration.Parser.Tokens;

namespace Windar.TrayApp.Configuration.Parser
{
    class ListParser : Parser<List>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public ListParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            ListOpen,
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

        internal static List GetList(ParserInputStream stream)
        {
            return new ListParser(stream).NextToken();
        }

        public override List NextToken()
        {
            var result = new List();
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                switch (_state)
                {
                    case State.Initial:
                        {
                            switch ((char) c)
                            {
                                case '[':
                                    {
                                        ChangeState(State.ListOpen);
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
                    case State.ListOpen:
                        // NOTE: Same as SegmentBegin, except ']' is allowed here.
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
                                case ']':
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
                                        result.Tokens.Add(TupleParser.GetTuple(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '[':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(GetList(InputStream));
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
                        // NOTE: Same as ListOpen, except ']' is NOT allowed here.
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
                                        result.Tokens.Add(TupleParser.GetTuple(InputStream));
                                        ChangeState(State.SegmentEnd);
                                        break;
                                    }
                                case '[':
                                    {
                                        InputStream.PushBack(c);
                                        result.Tokens.Add(GetList(InputStream));
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
                                        result.Tokens.Add(new Comma());
                                        ChangeState(State.SegmentBegin);
                                        break;
                                    }
                                case ']':
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

            const string endmsg = "Unexpected end while parsing List. Partial token exception property.";
            if (Log.IsErrorEnabled) Log.Error(endmsg);
            throw new ParserException(endmsg, result);
        }
    }
}
