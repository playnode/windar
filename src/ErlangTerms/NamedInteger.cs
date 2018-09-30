using System;
using System.Reflection;
using log4net;

namespace Windar.ErlangTermsParser
{
    public class NamedInteger : NamedValue
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public new int Value
        {
            get
            {
                return Int32.Parse(((IntegerToken) base.Value).Text);
            }
            set
            {
                base.Value = new IntegerToken(value.ToString());
            }
        }

        public NamedInteger(string name, int value) : base(name)
        {
            Tokens.Add(new IntegerToken(value.ToString()));
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedInteger instance.</param>
        /// <returns>An instance of NamedInteger based on the give tuple.</returns>
        public static new NamedInteger CreateFrom(TupleToken tuple)
        {
            if (Log.IsDebugEnabled) Log.Debug("Trying to create a NamedInteger from tuple = " + tuple);

            NamedInteger result = null;
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

                // We're expecting an integer to be the next value token.
                // Otherwise, quit and return false.
                if (!(tupleToken is IntegerToken)) break;

                // Create the NamedInteger instance and return.
                int value = Int32.Parse(((IntegerToken) tupleToken).Text);
                result = new NamedInteger(name, value);
                result.Tokens = tuple.Tokens;
                break;
            }
            return result;
        }
    }
}
