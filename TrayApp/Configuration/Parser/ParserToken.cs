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

using System.Reflection;
using log4net;

namespace Windar.TrayApp.Configuration.Parser
{
    /// <summary>
    /// This is a stub class required by the generic Parser class. This class 
    /// should be extended to override the ToString method. By design, using
    /// the ToString method should be used to re-write the parsed data.
    /// </summary>
    class ParserToken
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
