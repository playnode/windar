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

using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    public class NamedList : ListToken
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public string Name
        {
            get
            {
                string result = null;
                foreach (ParserToken token in Tokens)
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
                foreach (ParserToken token in Tokens)
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
                bool foundName = false;
                foreach (ParserToken token in Tokens)
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
            bool foundName = false;
            foreach (ParserToken tupleToken in tuple.Tokens)
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
                List<ParserToken> value = ((ListToken) tupleToken).Tokens;
                result = new NamedList(name, value);
                result.Tokens = tuple.Tokens;
                if (Log.IsDebugEnabled) Log.Debug("Result = " + result);
                break;
            }
            return result;
        }

        #endregion

        public List<string> GetStringsList()
        {
            List<string> result = new List<string>();
            foreach (ParserToken token in List.Tokens)
            {
                if (!(token is StringToken)) continue;
                result.Add(((StringToken) token).Text);
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append('{');
            foreach (ParserToken token in Tokens)
                result.Append(token.ToString());
            result.Append('}');
            return result.ToString();
        }

        #region Named items.

        private TupleToken GetTupleNamed(string name)
        {
            TupleToken result = null;
            foreach (ParserToken token in Tokens)
            {
                // Only interested in tuples in the list token.
                if (!(token is ListToken)) continue;
                foreach (ParserToken listToken in ((ListToken) token).Tokens)
                {
                    // Only interested in tuple tokens here.
                    if (!(listToken is TupleToken)) continue;

                    // Find tuple with name.
                    bool foundName = false;
                    foreach (ParserToken tupleToken in ((TupleToken) listToken).Tokens)
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
            int result = -1;
            TupleToken tuple = GetTupleNamed(name);
            if (tuple != null)
            {
                NamedInteger named = NamedInteger.CreateFrom(tuple);
                if (named != null) result = named.Value;
            }
            return result;
        }

        public string GetNamedString(string name)
        {
            string result = null;
            TupleToken tuple = GetTupleNamed(name);
            if (tuple != null)
            {
                NamedString named = NamedString.CreateFrom(tuple);
                if (named != null) result = named.Value;
            }
            return result;
        }

        public void SetNamedValue(string name, int value)
        {
            NamedInteger named = null;
            TupleToken tuple = GetTupleNamed(name);
            if (tuple != null) named = NamedInteger.CreateFrom(tuple);
            if (named == null)
            {
                named = new NamedInteger(name, value);
                if (List.CountValues() > 0) List.Tokens.Insert(List.Tokens.Count - 1, new CommaToken());
                List.Tokens.Add(new WhitespaceToken("\n"));
                List.Tokens.Add(new AddedComment());
                List.Tokens.Add(named);
                List.Tokens.Add(new WhitespaceToken("\n"));
            }
            named.Value = value;
        }

        public void SetNamedValue(string name, string value)
        {
            NamedString named = null;
            TupleToken tuple = GetTupleNamed(name);
            if (tuple != null) named = NamedString.CreateFrom(tuple);
            if (named == null)
            {
                named = new NamedString(name, value);
                if (List.CountValues() > 0) List.Tokens.Insert(List.Tokens.Count - 1, new CommaToken());
                List.Tokens.Add(new WhitespaceToken("\n"));
                List.Tokens.Add(new AddedComment());
                List.Tokens.Add(named);
                List.Tokens.Add(new WhitespaceToken("\n"));
            }
            named.Value = value;
        }

        #endregion

        #region List items.

        public void AddStringsListItem(string item)
        {
            if (GetStringsList().Contains(item))
            {
                if (Log.IsWarnEnabled)
                {
                    Log.Warn("Trying to add item which is already in the list. Item = " + item);
                }
                return;
            }
            int n = List.CountValues();
            if (n == 0)
            {
                // Check if there is already some leading newline or comment line.
                int c = 0;
                foreach (ParserToken token in List.Tokens)
                {
                    if (!(token is WhitespaceToken)) break;
                    c++;
                    if (token is CommentToken) break;
                    if (((WhitespaceToken) token).Text == "\n") break;
                }
                if (c == 0) List.Tokens.Insert(List.Tokens.Count, new WhitespaceToken("\n"));
            }
            if (n > 0) List.Tokens.Insert(List.Tokens.Count - 1, new CommaToken());
            List.Tokens.Add(new WhitespaceToken("\n"));
            List.Tokens.Add(new AddedComment());
            List.Tokens.Add(new StringToken(item));
            List.Tokens.Add(new WhitespaceToken("\n"));
        }

        public void RemoveStringsListItem(string item)
        {
            if (!GetStringsList().Contains(item))
            {
                if (Log.IsWarnEnabled)
                {
                    Log.Warn("Trying to remove item which is not in list. Item = " + item);
                }
                return;
            }
            Stack<ParserToken> previousTokens = new Stack<ParserToken>();
            foreach (ParserToken token in List.Tokens)
            {
                if (Log.IsDebugEnabled) Log.Debug("Token = " + token);
                if (token is StringToken && ((StringToken) token).Text == item)
                {
                    // Remove the resolver script from list.
                    List.Tokens.Remove(token);

                    if (previousTokens.Peek() is WhitespaceToken
                        && !(previousTokens.Peek() is CommentToken))
                    {
                        // Remove preceeding whitespace.
                        while (previousTokens.Peek() is WhitespaceToken
                            && !(previousTokens.Peek() is CommentToken))
                        {
                            List.Tokens.Remove(previousTokens.Pop());
                        }

                        // Remove comma token.
                        if (previousTokens.Peek() is CommaToken)
                        {
                            List.Tokens.Remove(previousTokens.Pop());
                        }
                    }
                    else
                    {
                        // Remove previous ErlangTerms comment (if applicable).
                        if (previousTokens.Count > 0
                            && previousTokens.Peek() is CommentToken
                            && ((CommentToken) previousTokens.Peek()).Text.StartsWith(AddedComment.AddedCommentBegin))
                        {
                            List.Tokens.Remove(previousTokens.Pop());

                            // Remove previous newline.
                            if (previousTokens.Count > 0
                                && previousTokens.Peek() is WhitespaceToken
                                && ((WhitespaceToken) previousTokens.Peek()).Text == "\n\n")
                            {
                                List.Tokens.Remove(previousTokens.Pop());

                                // Remove comma token, if found.
                                if (previousTokens.Count > 0
                                    && previousTokens.Peek() is CommaToken)
                                {
                                    List.Tokens.Remove(previousTokens.Pop());
                                }
                            }
                        }
                    }

                    // Find position of first non-whitespace token.
                    int pos = -1;
                    foreach (ParserToken tok in List.Tokens)
                    {
                        pos++;
                        if (!(tok is WhitespaceToken)) break;
                        continue;
                    }

                    // Remove previous comma token.
                    if (List.Tokens[pos] is CommaToken) List.Tokens.Remove(List.Tokens[pos]);

                    // Remove last newline if no more items in list.
                    if (List.CountValues() == 0)
                    {
                        if (List.Tokens[pos] is WhitespaceToken
                            && ((WhitespaceToken) List.Tokens[pos]).Text == "\n")
                            List.Tokens.Remove(List.Tokens[pos]);
                    }

                    break;
                }
                previousTokens.Push(token);
                continue;
            }
        }

        #endregion
    }
}
