﻿/*
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

using System;
using System.Text;

namespace Windar.TrayApp.Configuration.Parser
{
    public class WhitespaceToken : ParserToken
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public string ToEscapedString()
        {
            var result = new StringBuilder();
            result.Append("\"");
            foreach (var c in Text)
            {
                switch (c)
                {
                    case ' ':
                        {
                            result.Append(' ');
                            break;
                        }
                    case '\t':
                        {
                            result.Append("\\t");
                            break;
                        }
                    case '\r':
                        {
                            result.Append("\\r");
                            break;
                        }
                    case '\n':
                        {
                            result.Append("\\n");
                            break;
                        }
                    default:
                        throw new ApplicationException("Unexpected case in switch statement.");
                }
            }
            result.Append("\" (").Append(Text.Length).Append(' ');
            if (Text.Length == 1) result.Append("char)");
            else result.Append("chars)");
            return result.ToString();
        }
    }
}