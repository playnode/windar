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

using System;
using System.Reflection;
using log4net;
using Windar.TrayApp.Configuration.Parser.Basic;
using Windar.TrayApp.Configuration.Parser.Tokens;

namespace Windar.TrayApp.Configuration.Parser
{
    class TupleParser : Parser<Tuple>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public TupleParser(ParserInputStream stream) : base(stream) { }

        #region State

        private enum State
        {
            Initial,
        }

        private State _state = State.Initial;

        private void ChangeState(State to)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(GetStateChangeMessage(_state.ToString(), to.ToString()));
            }
            _state = to;
        }

        #endregion

        public override Tuple NextToken()
        {
            throw new NotImplementedException();
        }
    }
}
