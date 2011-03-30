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

using System;

namespace Playnode.ErlangTerms.Parser
{
    public class BooleanToken : AtomToken
    {
        public bool Value
        {
            get { return Convert.ToBoolean(Text); }
            set { Text = value.ToString().ToLower(); }
        }

        public BooleanToken(bool value)
        {
            Value = value;
        }

        public BooleanToken(string str)
        {
            switch (str.ToLower())
            {
                case "true":
                    Value = true;
                    break;
                case "false":
                    Value = false;
                    break;
                default:
                    throw new ArgumentException("String is not \"true\" or \"false\".");
            }
        }

        public BooleanToken(AtomToken token)
        {
            Value = Convert.ToBoolean(token.Text);
        }

        public override string ToString()
        {
            return Value ? "true" : "false";
        }
    }
}
