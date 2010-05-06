﻿/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * ErlangTerms - Tokeniser for some basic Erlang terms.
 *
 * ErlangTerms is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * ErlangTerms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System.Collections.Generic;
using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public class TupleToken : CompositeToken, IValueToken
    {
        public TupleToken()
        {
            Tokens = new List<ParserToken>();
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append('{');
            foreach (var token in Tokens) 
                result.Append(token.ToString());
            result.Append('}');
            return result.ToString();
        }
    }
}