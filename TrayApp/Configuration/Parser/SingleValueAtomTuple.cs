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
using Windar.TrayApp.Configuration.Parser.Tokens;

namespace Windar.TrayApp.Configuration.Parser
{
    public class SingleValueAtomTuple : TupleToken
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
                }
            }
        }

        public IValueToken Value
        {
            get
            {
                IValueToken result = null;
                foreach (var token in Tokens)
                {
                    // Ignore white-space.
                    if (!(token is IValueToken)) continue;

                    // Return value found.
                    if (Log.IsDebugEnabled) Log.Debug("Found and returning value = " + (IValueToken) token);
                    result = (IValueToken) token;
                    break;
                }
                return result;
            }
            set
            {
                var foundName = false;
                for (var i = 0; i < Tokens.Count; i++)
                {
                    // Ignore white-space.
                    if (!(Tokens[i] is IValueToken)) continue;

                    // Ignore the atom name.
                    if (!foundName)
                    {
                        foundName = true;
                        continue;
                    }

                    if (Log.IsDebugEnabled)
                    {
                        Log.Debug("Found and replacing value = " + (IValueToken) Tokens[i]);
                        Log.Debug("Replacing with value = " + value);
                    }

                    // Replace the value token.
                    Tokens[i] = (ParserToken) value;

                    if (Log.IsDebugEnabled)
                    {
                        Log.Debug("New value = " + (IValueToken) Tokens[i]);
                    }
                    break;
                }
            }
        }

        private SingleValueAtomTuple()
        {
            // Private constructor used in CreateFrom method.
        }

        public SingleValueAtomTuple(string name, IValueToken value)
        {
            Tokens.Add(new AtomToken { Text = name });
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken { Text = " " });
            Tokens.Add((ParserToken) value);
        }

        public SingleValueAtomTuple(string name, string value)
        {
            Tokens.Add(new AtomToken { Text = name });
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken { Text = " " });
            Tokens.Add(new StringToken { Text = value });
        }

        public SingleValueAtomTuple(string name, bool value)
        {
            Tokens.Add(new AtomToken { Text = name });
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken { Text = " " });
            Tokens.Add(new AtomToken { Text = value ? "true" : "false" });
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a SingleValueAtomTuple instance.</param>
        /// <returns>An instance of SingleValueAtomTuple based on the give tuple.</returns>
        public static SingleValueAtomTuple CreateFrom(TupleToken tuple)
        {
            SingleValueAtomTuple result = null;
            foreach (var tupleToken in tuple.Tokens)
            {
                // Seek out the first value token, ignoring spaces.
                if (!(tupleToken is IValueToken)) continue;

                // We're expecting the atom to be the first value token.
                if (!(tupleToken is AtomToken)) break;
                if (((AtomToken) tupleToken).Text.Equals("name"))
                {
                    //result = new SingleValueAtomTuple { Tokens = new List<ParserToken>() };
                    //foreach (var token in tuple.Tokens) result.Tokens.Add(token);
                    result = new SingleValueAtomTuple { Tokens = tuple.Tokens };
                }
                break;
            }
            return result;
        }
    }
}
