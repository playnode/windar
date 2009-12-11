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
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace Windar.TrayApp.Configuration.Parser
{
    public class ErlangTermsDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        internal List<ParserToken> Tokens { get; private set; }

        private FileInfo _file;

        public void Load(FileInfo file)
        {
            _file = file;
            Tokens = new List<ParserToken>();

            var stream = new ParserInputStream(_file.OpenText());
            var parser = new ErlangTermsParser(stream);
            ParserToken token;
            while ((token = parser.NextToken()) != null)
            {
                Tokens.Add(token);
                if (!Log.IsInfoEnabled) continue;
                if (token is CommentToken)
                {
                    var str = token.ToString();
                    Log.Info("Token, Comment: " + str.Substring(0, str.Length - 1));
                }
                else if (token is TermEndToken)
                {
                    Log.Info("Token, TermEnd.");
                }
                else if (token is NumericExpression)
                {
                    var str = token.ToString();
                    if (str.IndexOf('\n') > 0) Log.Info("Token, NumericExpression (multi-line)\n" + str);
                    else Log.Info("Token, NumericExpression: " + str);
                }
                else if (token is ListToken)
                {
                    var str = token.ToString();
                    if (str.IndexOf('\n') > 0) Log.Info("Token, List (multi-line)\n" + str);
                    else Log.Info("Token, List: " + str);
                }
                else if (token is StringToken)
                {
                    var str = token.ToString();
                    if (str.IndexOf('\n') > 0) Log.Info("Token, String (multi-line)\n" + str);
                    else Log.Info("Token, String: \"" + str + '"');
                }
                else if (token is TupleToken)
                {
                    var str = token.ToString();
                    if (str.IndexOf('\n') > 0) Log.Info("Token, Tuple (multi-line)\n" + str);
                    else Log.Info("Token, Tuple: " + str);
                }
                else if (token is WhitespaceToken)
                {
                    Log.Info("Token, Whitespace: " + ((WhitespaceToken) token).ToEscapedString());
                }
                else
                {
                    Log.Info("Token, Unrecognised!");
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var token in Tokens) result.Append(token.ToString());
            return result.ToString();
        }

        public void Save()
        {
            //TODO: Using _file
        }

        #region Methods to find named values.

        internal NamedValue FindNamedValue(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedValue result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named value from tuple.
                var tuple = (TupleToken) token;
                var named = NamedValue.CreateFrom(tuple);
                if (named == null) continue;

                // Found named tuple?
                if (named.Name != name)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.Debug("Didn't match, name = " + named.Name);
                    }
                    continue;
                }
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named value: name = " + name
                        + ", value = " + named.Value);
                }
                result = named;
                break;
            }
            return result;
        }

        internal NamedString FindNamedString(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedString result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named string from tuple.
                var tuple = (TupleToken) token;
                var named = NamedString.CreateFrom(tuple);
                if (named == null) continue;

                // Found named tuple?
                if (named.Name != name)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.Debug("Didn't match, name = " + named.Name);
                    }
                    continue;
                }
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named string: name = " + name 
                        + ", value = " + named.Value);
                }
                result = named;
                break;
            }
            return result;
        }

        internal NamedBoolean FindNamedBoolean(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedBoolean result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named boolean from tuple.
                var tuple = (TupleToken) token;
                var named = NamedBoolean.CreateFrom(tuple);
                if (named == null) continue;

                // Found named tuple?
                if (named.Name != name)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.Debug("Didn't match, name = " + named.Name);
                    }
                    continue;
                }
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named boolean: name = " + name
                        + ", value = " + named.Value);
                }
                result = named;
                break;
            }
            return result;
        }

        internal NamedList FindNamedList(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedList result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                if (Log.IsDebugEnabled) Log.Debug("TupleToken = " + token);

                // Try to create a named value from tuple.
                var tuple = (TupleToken) token;
                var named = NamedList.CreateFrom(tuple);
                if (named == null)
                {
                    if (Log.IsDebugEnabled) Log.Debug("NamedList Is Nothing");
                    continue;
                }

                // Found named tuple?
                if (named.Name != name)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.Debug("Didn't match, name = " + named.Name);
                    }
                    continue;
                }
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named list: name = " + name
                        + ", list = " + named.List);
                }
                result = named;
                break;
            }
            return result;
        }

        #endregion
    }
}
