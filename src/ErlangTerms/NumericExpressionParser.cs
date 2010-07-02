/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Reflection;
using System.Text;
using log4net;

namespace Playnode.ErlangTerms.Parser
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
            var buffer = new StringBuilder();
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                            var msg = GetUnexpectedStateErrorMessage(_state.ToString());
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
