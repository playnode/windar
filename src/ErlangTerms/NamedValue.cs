﻿using System.Reflection;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class NamedValue : TupleToken
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

        public IValueToken Value
        {
            get
            {
                IValueToken result = null;
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
                    if (Log.IsDebugEnabled) Log.Debug("Found and returning value = " + (IValueToken) token);
                    result = (IValueToken) token;
                    break;
                }
                return result;
            }
            set
            {
                bool foundName = false;
                for (int i = 0; i < Tokens.Count; i++)
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

        public NamedValue(string name, IValueToken value)
        {
            Tokens.Add(new AtomToken(name));
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken(" "));
            Tokens.Add((ParserToken) value);
        }

        /// <summary>
        /// Extending class to add value token.
        /// </summary>
        /// <param name="name">Name for the value.</param>
        protected NamedValue(string name)
        {
            Tokens.Add(new AtomToken(name));
            Tokens.Add(new CommaToken());
            Tokens.Add(new WhitespaceToken(" "));
        }

        private NamedValue()
        {
            // Private constructor used in CreateFrom method.
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedValue instance.</param>
        /// <returns>An instance of NamedValue based on the give tuple.</returns>
        public static NamedValue CreateFrom(TupleToken tuple)
        {
            NamedValue result = null;
            foreach (ParserToken tupleToken in tuple.Tokens)
            {
                // Seek out the first value token, ignoring spaces.
                if (!(tupleToken is IValueToken)) continue;

                // We're expecting the atom name to be the first value token.
                // Otherwise, quite and return false.
                if (!(tupleToken is AtomToken)) break;

                // Create the NamedValue instance and return.
                result = new NamedValue();
                result.Tokens = tuple.Tokens;
                break;
            }
            return result;
        }
    }
}
