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

using System.Collections.Generic;

namespace Windar.TrayApp.Configuration.Parser
{
    class CompositeToken : ParserToken
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
    }
}
