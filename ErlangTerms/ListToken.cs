/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System.Collections.Generic;
using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class ListToken : CompositeToken, IValueToken
    {
        public ListToken()
        {
            Tokens = new List<ParserToken>();
        }

        public ListToken(List<ParserToken> list)
        {
            Tokens = list;
        }

        public int CountValues()
        {
            var result = 0;
            foreach (var token in Tokens)
                if (token is IValueToken) result++;
            return result;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append('[');
            foreach (var token in Tokens)
                result.Append(token.ToString());
            result.Append(']');
            return result.ToString();
        }
    }
}
