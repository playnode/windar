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

using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class WhitespaceToken : ParserToken
    {
        public string Text { get; set; }

        public WhitespaceToken()
        {
            
        }

        public WhitespaceToken(string text)
        {
            Text = text;
        }

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
                    case '"':
                        {
                            result.Append("\\\"");
                            break;
                        }
                    default:
                        result.Append(c);
                        break;
                }
            }
            result.Append("\" (").Append(Text.Length).Append(' ');
            if (Text.Length == 1) result.Append("char)");
            else result.Append("chars)");
            return result.ToString();
        }
    }
}
