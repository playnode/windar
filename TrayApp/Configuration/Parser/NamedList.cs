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
using System.Reflection;
using System.Text;
using log4net;

namespace Windar.TrayApp.Configuration.Parser
{
    class NamedList : ListToken
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

        #region Constructors

        public NamedList(string name, ListToken list)
        {
            Tokens.Add(new AtomToken(name));
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken(" "));
            Tokens.Add(new ListToken(list.Tokens));
        }

        public NamedList(string name, List<ParserToken> list)
        {
            Tokens.Add(new AtomToken(name));
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken(" "));
            Tokens.Add(new ListToken(list));
        }

        /// <summary>
        /// Extending class to add value token.
        /// </summary>
        /// <param name="name">Name for the value.</param>
        protected NamedList(string name)
        {
            Tokens.Add(new AtomToken(name));
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken(" "));
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
            if (Log.IsDebugEnabled) Log.Debug("Trying to create a NamedList from tuple = " + tuple);

            NamedList result = null;
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

                // We're expecting a list to be the next value token.
                // Otherwise, quit and return false.
                if (!(tupleToken is ListToken)) break;

                // Create the NamedList instance and return.
                var value = ((ListToken) tupleToken).Tokens;
                result = new NamedList(name, value) { Tokens = tuple.Tokens };
                if (Log.IsDebugEnabled) Log.Debug("Result = " + result);
                break;
            }
            return result;
        }

        #endregion

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append('{');
            foreach (var token in Tokens)
                result.Append(token.ToString());
            result.Append('}');
            return result.ToString();
        }

        private TupleToken GetTupleNamed(string name)
        {
            TupleToken result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples in the list token.
                if (!(token is ListToken)) continue;
                foreach (var listToken in ((ListToken) token).Tokens)
                {
                    // Only interested in tuple tokens here.
                    if (!(listToken is TupleToken)) continue;

                    // Find tuple with name.
                    var foundName = false;
                    foreach (var tupleToken in ((TupleToken) listToken).Tokens)
                    {
                        // Only interested in value tokens here.
                        if (!(tupleToken is IValueToken)) continue;
                        if (!foundName)
                        {
                            if (!(tupleToken is AtomToken)) continue;
                            if (((AtomToken) tupleToken).Text != name) break;
                            foundName = true;
                            continue;
                        }
                        result = (TupleToken) listToken;
                        break;
                    }

                    // Break loop if result found.
                    if (result != null) break;
                }

                // Break loop if result found.
                if (result != null) break;
            }
            return result;
        }

        public int GetNamedInteger(string name)
        {
            return NamedInteger.CreateFrom(GetTupleNamed(name)).Value;
        }

        public string GetNamedString(string name)
        {
            return NamedString.CreateFrom(GetTupleNamed(name)).Value;
        }

        public void SetNamedValue(string name, int value)
        {
            var named = NamedInteger.CreateFrom(GetTupleNamed(name));
            if (named == null)
            {
                named = new NamedInteger(name, value);
                if (CountValues() > 0) List.Tokens.Insert(List.Tokens.Count - 1, new CommaToken());
                List.Tokens.Add(new WhitespaceToken("\n"));
                List.Tokens.Add(new WindarAddedComment());
                List.Tokens.Add(named);
                List.Tokens.Add(new WhitespaceToken("\n"));
            }
            named.Value = value;
        }

        public void SetNamedValue(string name, string value)
        {
            var named = NamedString.CreateFrom(GetTupleNamed(name));
            if (named == null)
            {
                named = new NamedString(name, value);
                if (CountValues() > 0) List.Tokens.Insert(List.Tokens.Count - 1, new CommaToken());
                List.Tokens.Add(new WhitespaceToken("\n"));
                List.Tokens.Add(new WindarAddedComment());
                List.Tokens.Add(named);
                List.Tokens.Add(new WhitespaceToken("\n"));
            }
            named.Value = value;
        }

        private int CountValues()
        {
            var result = 0;
            foreach (var token in Tokens)
                if (token is IValueToken) result++;
            return result;
        }

        public void AddListItem(string item)
        {
            //TODO
        }

        public void RemoveListItem(string item)
        {
            //TODO
        }
    }
}
