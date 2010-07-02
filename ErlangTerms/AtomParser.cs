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
            var buffer = new StringBuilder();
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
                                        if (Log.IsErrorEnabled) Log.Error(msg);
                                        throw new ParserException(msg);
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
