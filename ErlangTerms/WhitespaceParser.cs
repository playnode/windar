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
            var buffer = new StringBuilder();
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
                                        var msg = GetEdgeUnknownErrorMessage(c, _state.ToString());
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
                            var msg = GetUnexpectedStateErrorMessage(_state.ToString());
                            throw new ParserException(msg);
                        }
                }
            }

            // End of file.
            return null;
        }
    }
}
