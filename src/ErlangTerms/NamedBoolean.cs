﻿/*
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

using System;
using System.Reflection;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    public class NamedBoolean : NamedValue
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public new bool Value
        {
            get
            {
                //return Convert.ToBoolean(((AtomToken) base.Value).Text);
                return new BooleanToken((AtomToken) base.Value).Value;
            }
            set
            {
                base.Value = new BooleanToken(value);
            }
        }

        public NamedBoolean(string name, bool value) : base(name)
        {
            Tokens.Add(new BooleanToken(value));
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedBoolean instance.</param>
        /// <returns>An instance of NamedBoolean based on the give tuple.</returns>
        public static new NamedBoolean CreateFrom(TupleToken tuple)
        {
            if (Log.IsDebugEnabled) Log.Debug("Trying to create a NamedBoolean from tuple = " + tuple);

            NamedBoolean result = null;
            string name = null;
            var foundName = false;
            foreach (var tupleToken in tuple.Tokens)
            {
                if (Log.IsDebugEnabled) Log.Debug("name = " + name);

                // Seek out the first value token, ignoring spaces.
                if (!(tupleToken is IValueToken)) continue;

                if (!foundName)
                {
                    // We're expecting the atom to be the first value token.
                    // Otherwise, quit and return false.
                    if (!(tupleToken is AtomToken)) break;

                    // Store the name and look for the value.
                    name = ((AtomToken) tupleToken).Text;
                    if (Log.IsDebugEnabled) Log.Debug("Found name = " + name);
                    foundName = true;
                    continue;
                }

                // We're expecting an atom to be the next value token.
                // Otherwise, quit and return false.
                if (!(tupleToken is AtomToken)) break;

                // Create the NamedBoolean instance and return.
                var value = Convert.ToBoolean(((AtomToken) tupleToken).Text);
                result = new NamedBoolean(name, value) { Tokens = tuple.Tokens };
                if (Log.IsDebugEnabled) Log.Debug("Result = " + result);
                break;
            }
            return result;
        }
    }
}