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

using System.Reflection;
using log4net;

namespace Windar.TrayApp.Configuration.Parser
{
    internal class NamedList : ListToken
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public string Name
        {
            get
            {
                string result = null;
                foreach (var token in Tokens)
                {
                    if (!(token is AtomToken)) continue;
                    if (Log.IsDebugEnabled) Log.Debug("Found and returning atom name = " + ((AtomToken) token).Text);
                    result = ((AtomToken) token).Text;
                    break;
                }
                return result;
            }
            set
            {
                foreach (var token in Tokens)
                {
                    if (!(token is AtomToken)) continue;
                    if (Log.IsDebugEnabled) Log.Debug("Found and setting atom name = " + ((AtomToken) token).Text);
                    ((AtomToken) token).Text = value;
                    break;
                }
            }
        }

        public ListToken List
        {
            get
            {
                ListToken result = null;
                var foundName = false;
                foreach (var token in Tokens)
                {
                    // Ignore white-space.
                    if (!(token is IValueToken)) continue;

                    // Ignore the atom name.
                    if (!foundName)
                    {
                        foundName = true;
                        continue;
                    }

                    // Return value found.
                    if (Log.IsDebugEnabled) Log.Debug("Found and returning value = " + token);
                    result = (ListToken) token;
                    break;
                }
                return result;
            }
        }

        // ReSharper disable SuggestBaseTypeForParameter
        public NamedList(string name, ListToken list)
        // ReSharper restore SuggestBaseTypeForParameter
        {
            Tokens.Add(new AtomToken { Text = name });
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken { Text = " " });
            Tokens.Add(list);
        }

        /// <summary>
        /// Extending class to add value token.
        /// </summary>
        /// <param name="name">Name for the value.</param>
        protected NamedList(string name)
        {
            Tokens.Add(new AtomToken { Text = name });
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken { Text = " " });
        }

        private NamedList()
        {
            // Private constructor used in CreateFrom method.
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedList instance.</param>
        /// <returns>An instance of NamedList based on the give tuple.</returns>
        public static NamedList CreateFrom(TupleToken tuple)
        {
            NamedList result = null;
            foreach (var tupleToken in tuple.Tokens)
            {
                // Seek out the first value token, ignoring spaces.
                if (!(tupleToken is IValueToken)) continue;

                // We're expecting the atom to be the first value token.
                // Otherwise, quite and return false.
                if (!(tupleToken is ListToken)) break;

                // Create the NamedList instance and return.
                result = new NamedList { Tokens = tuple.Tokens };
                break;
            }
            return result;
        }
   }
}
