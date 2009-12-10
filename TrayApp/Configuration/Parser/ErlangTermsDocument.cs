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
using Windar.TrayApp.Configuration.Parser.Tokens;
using Windar.TrayApp.Configuration.Values;

namespace Windar.TrayApp.Configuration.Parser
{
    public class ErlangTermsDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public List<ParserToken> Tokens { get; private set; }

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

        public NamedValue FindNamedValue(string name)
        {
            NamedValue result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                var tuple = (TupleToken) token;
                var svaTuple = NamedValue.CreateFrom(tuple);
                if (svaTuple == null) continue;
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named string: name = " + name
                        + ", value = " + svaTuple.Value);
                }
                result = svaTuple;
                break;
            }
            return result;
        }

        public NamedString FindNamedString(string name)
        {
            NamedString result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                var tuple = (TupleToken) token;
                var svaTuple = NamedString.CreateFrom(tuple);
                if (svaTuple == null) continue;
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named string: name = " + name 
                        + ", value = " + svaTuple.Value);
                }
                result = svaTuple;
                break;
            }
            return result;
        }

        public NamedBoolean FindNamedBoolean(string name)
        {
            NamedBoolean result = null;
            foreach (var token in Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                var tuple = (TupleToken) token;
                var svaTuple = NamedBoolean.CreateFrom(tuple);
                if (svaTuple == null) continue;
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Found named string: name = " + name
                        + ", value = " + svaTuple.Value);
                }
                result = svaTuple;
                break;
            }
            return result;
        }
    }
}
