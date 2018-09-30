using System;
using System.Reflection;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class NamedBoolean : NamedValue
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public new bool Value
        {
            get
            {
                //return Convert.ToBoolean(((AtomToken) base.Value).Text);
                return new BooleanToken((AtomToken) base.Value).Value;
            }
            set
            {
                base.Value = new BooleanToken(value);
            }
        }

        public NamedBoolean(string name, bool value) : base(name)
        {
            Tokens.Add(new BooleanToken(value));
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedBoolean instance.</param>
        /// <returns>An instance of NamedBoolean based on the give tuple.</returns>
        public static new NamedBoolean CreateFrom(TupleToken tuple)
        {
            if (Log.IsDebugEnabled) Log.Debug("Trying to create a NamedBoolean from tuple = " + tuple);

            NamedBoolean result = null;
            string name = null;
            bool foundName = false;
            foreach (ParserToken tupleToken in tuple.Tokens)
            {
                if (Log.IsDebugEnabled) Log.Debug("name = " + name);

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

                // We're expecting an atom to be the next value token.
                // Otherwise, quit and return false.
                if (!(tupleToken is AtomToken)) break;

                // Create the NamedBoolean instance and return.
                bool value = Convert.ToBoolean(((AtomToken) tupleToken).Text);
                result = new NamedBoolean(name, value);
                result.Tokens = tuple.Tokens;
                if (Log.IsDebugEnabled) Log.Debug("Result = " + result);
                break;
            }
            return result;
        }
    }
}
