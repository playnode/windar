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
using Windar.TrayApp.Configuration.Parser.Basic;
using Windar.TrayApp.Configuration.Parser.Tokens;

namespace Windar.TrayApp.Configuration.Parser
{
    /// <summary>
    /// This is a simple parser for extracting tokens to allow for the
    /// configuration terms to be parsed and updated before being written back
    /// to file. This is done to preserve manual edits.
    /// </summary>
    public class ErlangTermsParser : Parser<ParserToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public ErlangTermsParser(ParserInputStream stream) : base(stream) { }

        #region Character validation functions.

        private static bool IsValidAtomNameFirstChar(int c)
        {
            return c > 96 && c < 123;  // Lower-case letters.
        }

        private static bool IsValidAtomNameChar(int c)
        {
            return (c > 96 && c < 123) // Lower-case letters.
                || (c > 63 && c < 91)  // Upper-case letters and '@' char.
                || (c == '_');         // Underscore.
        }

        private static bool IsValidNumericalExpressionFirstChar(int c)
        {
            return (c > 47 && c < 58)  // Digits 0-9.
                || (c == '('); // Brackets (not currently supported).
        }

        #endregion

        public override ParserToken NextToken()
        {
            int c;
            while ((c = InputStream.NextChar()) != -1)
            {
                switch ((char) c)
                {
                    case '%':
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        {
                            InputStream.PushBack(c);
                            return GetWhitespace();
                        }
                    case '{':
                        {
                            InputStream.PushBack(c);
                            return GetTuple();
                        }
                    case '.':
                        {
                            return new EndTerm();
                        }
                    default:
                        {
                            var msg = GetEdgeUnknownErrorMessage(c, "DocumentRoot");
                            if (Log.IsErrorEnabled) Log.Error(msg);
                            throw new ParserException(msg);
                        }
                }
            }
            return null;
        }

        private Whitespace GetWhitespace()
        {
            var parser = new WhitespaceParser(InputStream);
            return parser.NextToken();
        }

        private Tuple GetTuple()
        {
            var parser = new TupleParser(InputStream);
            return parser.NextToken();
        }
    }
}
