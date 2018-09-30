using System;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class ErlangTermsDocument
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        CompositeToken _document;

        protected CompositeToken Document
        {
            get { return _document; }
            set { _document = value; }
        }

        FileInfo _file;

        public void Load(FileInfo file)
        {
            _file = file;
            Document = new CompositeToken();
            StreamReader reader = file.OpenText();
            ParserInputStream stream = new ParserInputStream(reader);
            ErlangTermsParser parser = new ErlangTermsParser(stream);
            ParserToken token;
            try
            {
                while ((token = parser.NextToken()) != null)
                {
                    Document.Tokens.Add(token);
                    if (!Log.IsInfoEnabled) continue;
                    if (token is CommentToken)
                    {
                        String str = token.ToString();
                        Log.Info("Token, Comment: " + str.Substring(0, str.Length - 1));
                    }
                    else if (token is TermEndToken)
                    {
                        Log.Info("Token, TermEnd.");
                    }
                    else if (token is NumericExpression)
                    {
                        string str = token.ToString();
                        if (str.IndexOf('\n') > 0) Log.Info("Token, NumericExpression (multi-line)\n" + str);
                        else Log.Info("Token, NumericExpression: " + str);
                    }
                    else if (token is ListToken)
                    {
                        string str = token.ToString();
                        if (str.IndexOf('\n') > 0) Log.Info("Token, List (multi-line)\n" + str);
                        else Log.Info("Token, List: " + str);
                    }
                    else if (token is StringToken)
                    {
                        string str = token.ToString();
                        if (str.IndexOf('\n') > 0) Log.Info("Token, String (multi-line)\n" + str);
                        else Log.Info("Token, String: \"" + str + '"');
                    }
                    else if (token is TupleToken)
                    {
                        string str = token.ToString();
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
            StringBuilder result = new StringBuilder();
            foreach (ParserToken token in Document.Tokens) result.Append(token.ToString());
            return result.ToString();
        }

        public void Save()
        {
            // Make a backup of configution files before saving.
            // These can be restored manually if necessary.
            string filename = _file.FullName;
            string bak = filename + ".bak";
            FileInfo bakFile = new FileInfo(bak);
            if (bakFile.Exists) bakFile.Delete();
            _file.MoveTo(bak);
            _file = new FileInfo(filename);

            // Write the new updated file.
            FileStream fs = _file.OpenWrite();
            byte[] info = new UTF8Encoding(true).GetBytes(Document.ToString());
            fs.Write(info, 0, info.Length);
            fs.Close();
        }

        #region Methods to find named values.

        public NamedValue FindNamedValue(string name)
        {
            if (Log.IsDebugEnabled) Log.Debug("Looking for name = " + name);

            NamedValue result = null;
            foreach (ParserToken token in Document.Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named value from tuple.
                TupleToken tuple = (TupleToken)token;
                NamedValue named = NamedValue.CreateFrom(tuple);
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
            foreach (ParserToken token in Document.Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named string from tuple.
                TupleToken tuple = (TupleToken)token;
                NamedString named = NamedString.CreateFrom(tuple);
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
            foreach (ParserToken token in Document.Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named boolean from tuple.
                TupleToken tuple = (TupleToken)token;
                NamedBoolean named = NamedBoolean.CreateFrom(tuple);
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
            foreach (ParserToken token in Document.Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                // Try to create a named boolean from tuple.
                TupleToken tuple = (TupleToken)token;
                NamedInteger named = NamedInteger.CreateFrom(tuple);
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
            foreach (ParserToken token in Document.Tokens)
            {
                // Only interested in tuples.
                if (!(token is TupleToken)) continue;

                if (Log.IsDebugEnabled) Log.Debug("TupleToken = " + token);

                // Try to create a named value from tuple.
                TupleToken tuple = (TupleToken)token;
                NamedList named = NamedList.CreateFrom(tuple);
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
