using System.Reflection;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    public class NamedString : NamedValue
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public new string Value
        {
            get
            {
                return ((StringToken) base.Value).Text;
            }
            set
            {
                base.Value = new StringToken(value);
            }
        }

        public NamedString(string name, string value) : base(name)
        {
            Tokens.Add(new StringToken(value));
        }

        /// <summary>
        /// This method will return null if the given tuple is not found to be
        /// suitable. A suitable tuple would have a single value, and a single
        /// atom as first part of the tuple.
        /// </summary>
        /// <param name="tuple">Tuple to use in creating a NamedString instance.</param>
        /// <returns>An instance of NamedString based on the give tuple.</returns>
        public static new NamedString CreateFrom(TupleToken tuple)
        {
            if (Log.IsDebugEnabled) Log.Debug("Trying to create a NamedString from tuple = " + tuple);

            NamedString result = null;
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

                // We're expecting a string to be the next value token.
                // Otherwise, quit and return false.
                if (!(tupleToken is StringToken)) break;

                // Create the NamedString instance and return.
                string value = ((StringToken) tupleToken).Text;
                result = new NamedString(name, value);
                result.Tokens = tuple.Tokens;
                break;
            }
            return result;
        }
    }
}
