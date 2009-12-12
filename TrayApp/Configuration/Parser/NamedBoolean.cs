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

using System;
using System.Reflection;
using log4net;

namespace Windar.TrayApp.Configuration.Parser
{
    class NamedBoolean : NamedValue
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public new bool Value
        {
            get
            {
                return Convert.ToBoolean(((AtomToken) base.Value).Text);
            }
            set
            {
                base.Value = new AtomToken(value ? "true" : "false");
            }
        }

        public NamedBoolean(string name, bool value) : base(name)
        {
            Tokens.Add(new AtomToken(value ? "true" : "false"));
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
