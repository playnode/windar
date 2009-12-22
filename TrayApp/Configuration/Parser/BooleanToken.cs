/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
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

using System;

namespace Windar.TrayApp.Configuration.Parser
{
    class BooleanToken : AtomToken
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
