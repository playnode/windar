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

namespace Windar.TrayApp.Configuration.Parser
{
    class StringParser : Parser<StringToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public StringParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
            StringOpen,
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

        internal static StringToken GetString(ParserInputStream stream)
        {
            return new StringParser(stream).NextToken();
        }

        public override StringToken NextToken()
        {
            var buffer = new StringBuilder();
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("StringParser: c = " + ParserInputStream.ToNameString(c));
                }
                switch (_state)
                {
                    case State.Initial:
                        {
                            switch ((char) c)
                            {
                                case '"':
                                    {
                                        ChangeState(State.StringOpen);
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
                    case State.StringOpen:
                        {
                            if (IsValidStringValueCharacter(c))
                            {
                                buffer.Append((char) c);
                                break;
                            }

                            switch ((char) c)
                            {
                                case '"':
                                    {
                                        return new StringToken(buffer.ToString());
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

            const string endmsg = "Unexpected end while parsing String.";
            if (Log.IsErrorEnabled) Log.Error(endmsg);
            throw new ParserException(endmsg);
        }

        internal static bool IsValidStringValueCharacter(int c)
        {
            //TODO: Check what characters are disallowed in string value.
            return ((char) c) != '"';
        }
    }
}
