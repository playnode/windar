/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010, 2011 Steven Robertson <steve@playnode.com>
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
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    /// <summary>
    /// NOTE: This is a simple parser for extracting erlang terms 
    /// to allow for the Playdar configuration terms to be parsed and updated
    /// before being written back to file. This is to preserve manual edits.
    /// </summary>
    public class ErlangTermsParser : Parser<ParserToken>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public ErlangTermsParser(ParserInputStream stream) : base(stream) { }

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
                            return WhitespaceParser.GetWhitespace(InputStream);
                        }
                    case '{':
                        {
                            InputStream.PushBack(c);
                            return TupleParser.GetTuple(InputStream);
                        }
                    case '.':
                        {
                            return new TermEndToken();
                        }
                    default:
                        {
                            string msg = GetEdgeUnknownErrorMessage(c, "DocumentRoot");
                            if (Log.IsErrorEnabled) Log.Error(msg);
                            throw new ParserException(msg);
                        }
                }
            }
            return null;
        }
    }
}
