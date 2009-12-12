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
    class NamedInteger : NamedValue
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public new int Value
        {
            get
            {
                return Int32.Parse(((IntegerToken) base.Value).Text);
            }
            set
            {
                base.Value = new IntegerToken(value.ToString());
            }
        }

        public NamedInteger(string name, int value) : base(name)
        {
            Tokens.Add(new IntegerToken(value.ToString()));
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedInteger instance.</param>
        /// <returns>An instance of NamedInteger based on the give tuple.</returns>
        public static new NamedInteger CreateFrom(TupleToken tuple)
        {
            if (Log.IsDebugEnabled) Log.Debug("Trying to create a NamedInteger from tuple = " + tuple);

            NamedInteger result = null;
            string name = null;
            var foundName = false;
            foreach (var tupleToken in tuple.Tokens)
            {
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

                // We're expecting an integer to be the next value token.
                // Otherwise, quit and return false.
                if (!(tupleToken is IntegerToken)) break;

                // Create the NamedInteger instance and return.
                var value = Int32.Parse(((IntegerToken) tupleToken).Text);
                result = new NamedInteger(name, value) { Tokens = tuple.Tokens };
                break;
            }
            return result;
        }
    }
}
