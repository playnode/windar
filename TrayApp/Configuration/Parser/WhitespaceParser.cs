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
using System.Text;
using log4net;
using Windar.TrayApp.Configuration.Parser.Tokens;

namespace Windar.TrayApp.Configuration.Parser
{
    class WhitespaceParser : Parser<WhitespaceToken>
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
                                        return new CommentToken { Text = buffer.ToString() };
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
                                        return new WhitespaceToken { Text = buffer.ToString() };
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
