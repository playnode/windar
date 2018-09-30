using System.Reflection;
using log4net;

namespace Windar.ErlangTermsParser
{
    /// <summary>
    /// This is a stub class required by the generic Parser class. This class 
    /// should be extended to override the ToString method. By design, using
    /// the ToString method should be used to re-write the parsed data.
    /// </summary>
    public class ParserToken
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public ParserToken()
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug("Constructing " + GetType().Name);
            }
        }

        /// <summary>
        /// Require all extending classes to override ToString method.
        /// Otherwise NotImplementedException is thrown.
        /// </summary>
        /// <returns>Not implemented.</returns>
        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}
