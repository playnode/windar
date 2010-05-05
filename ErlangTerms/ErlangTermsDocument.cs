/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * ErlangTerms - Tokeniser for some basic Erlang terms.
 *
 * ErlangTerms is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * ErlangTerms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    public class ErlangTermsDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        protected CompositeToken Document { get; private set; }

        private FileInfo _file;

        public void Load(FileInfo file)
        {
            _file = file;
            Document = new CompositeToken();
            var reader = file.OpenText();
            var stream = new ParserInputStream(reader);
            var parser = new ErlangTermsParser(stream);
            ParserToken token;
            try
            {
                while ((token = parser.NextToken()) != null)
                {
                    Document.Tokens.Add(token);
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
            catch (Exception ex)
            {
                throw new Exception("Exception when loading erlang terms document.", ex);
            }
            finally
            {
                // Close the file.
                reader.Close();
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var token in Document.Tokens) result.Append(token.ToString());
            return result.ToString();
        }

        public void Save()
        {
            // Make a backup of configution files before saving.
            // These can be restored manually if necessary.
            var filename = _file.FullName;
            var bak = filename + ".bak";
            var bakFile = new FileInfo(bak);
            if (bakFile.Exists) bakFile.Delete();
            _file.MoveTo(bak);
            _file = new FileInfo(filename);

            // Write the new updated file.
            var fs = _file.OpenWrite();
            var info = new UTF8Encoding(true).GetBytes(Document.ToString());
            fs.Write(info, 0, info.Length);
            fs.Close();
        }

        #region Methods to find named values.

        public NamedValue FindNamedValue(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedValue result = null;
            foreach (var token in Document.Tokens)
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

        public NamedString FindNamedString(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedString result = null;
            foreach (var token in Document.Tokens)
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

        public NamedBoolean FindNamedBoolean(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedBoolean result = null;
            foreach (var token in Document.Tokens)
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

        public NamedInteger FindNamedInteger(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedInteger result = null;
            foreach (var token in Document.Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named boolean from tuple.
                var tuple = (TupleToken) token;
                var named = NamedInteger.CreateFrom(tuple);
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
                    Log.Info("Found named integer: name = " + name
                        + ", value = " + named.Value);
                }
                result = named;
                break;
            }
            return result;
        }

        public NamedList FindNamedList(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedList result = null;
            foreach (var token in Document.Tokens)
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
