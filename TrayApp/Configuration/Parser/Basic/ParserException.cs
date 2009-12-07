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

namespace Windar.TrayApp.Configuration.Parser.Basic
{
    class ParserException : Exception
    {
        public ParserToken IncompleteToken { get; private set; }

        public ParserException() { }
        public ParserException(string msg) : base(msg) { }
        public ParserException(string msg, Exception ex) : base(msg, ex) { }
        public ParserException(string msg, ParserToken partialToken) : base(msg) { IncompleteToken = partialToken; }
    }
}
