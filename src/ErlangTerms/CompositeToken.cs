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

using System.Collections.Generic;
using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class CompositeToken : ParserToken
    {
        List<ParserToken> _tokens;

        public List<ParserToken> Tokens
        {
            get { return _tokens; }
            set { _tokens = value; }
        }

        public CompositeToken()
        {
            Tokens = new List<ParserToken>();
        }

        public List<IValueToken> GetValueTokens()
        {
            List<IValueToken> result = new List<IValueToken>();
            foreach (ParserToken token in Tokens)
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
            List<StringToken> result = new List<StringToken>();
            foreach (IValueToken token in GetValueTokens())
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
            List<TupleToken> result = new List<TupleToken>();
            foreach (IValueToken token in GetValueTokens())
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
            StringBuilder result = new StringBuilder();
            foreach (ParserToken token in Tokens) result.Append(token);
            return result.ToString();
        }
    }
}
