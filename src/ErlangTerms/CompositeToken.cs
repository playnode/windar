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

using System.Collections.Generic;
using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class CompositeToken : ParserToken
    {
        public List<ParserToken> Tokens { get; set; }

        public CompositeToken()
        {
            Tokens = new List<ParserToken>();
        }

        public List<IValueToken> GetValueTokens()
        {
            var result = new List<IValueToken>();
            foreach (var token in Tokens)
            {
                if (token is IValueToken)
                {
                    result.Add((IValueToken) token);
                }
            }
            return result;
        }

        public List<StringToken> GetStringTokens()
        {
            var result = new List<StringToken>();
            foreach (var token in GetValueTokens())
            {
                if (token is StringToken)
                {
                    result.Add((StringToken) token);
                }
            }
            return result;
        }

        public List<TupleToken> GetTupleTokens()
        {
            var result = new List<TupleToken>();
            foreach (var token in GetValueTokens())
            {
                if (token is TupleToken)
                {
                    result.Add((TupleToken) token);
                }
            }
            return result;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var token in Tokens) result.Append(token);
            return result.ToString();
        }
    }
}
