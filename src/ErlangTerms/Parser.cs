/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010, 2011 Steven Robertson <steve@playnode.com>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Text;

namespace Playnode.ErlangTerms.Parser
{
    public abstract class Parser<T> where T : new()
    {
        #region Properties

        ParserInputStream _inputStream;
        int _numErrors;
        int _numWarnings;
        int _numRecoveries;

        protected ParserInputStream InputStream
        {
            get { return _inputStream; }
            set { _inputStream = value; }
        }

        protected int NumErrors
        {
            get { return _numErrors; }
            set { _numErrors = value; }
        }

        protected int NumWarnings
        {
            get { return _numWarnings; }
            set { _numWarnings = value; }
        }

        protected int NumRecoveries
        {
            get { return _numRecoveries; }
            set { _numRecoveries = value; }
        }

        public int CharCount
        {
            get { return InputStream.CharCount; }
        }

        public int Checksum
        {
            get { return InputStream.Checksum; }
        }

        #endregion

        protected Parser(ParserInputStream stream)
        {
            InputStream = stream;
        }

        public abstract T NextToken();

        #region Reporting

        /// <summary>
        /// A standard message for state changes.
        /// </summary>
        /// <param name="fromState">From state name.</param>
        /// <param name="toState">Target state name.</param>
        /// <returns></returns>
        protected static string GetStateChangeMessage(string fromState, string toState)
        {
            StringBuilder result = new StringBuilder();
            result.Append("Changing state from ").Append(fromState);
            result.Append(" to ").Append(toState);
            result.Append('.');
            return result.ToString();
        }

        /// <summary>
        /// Provides a simple error message naming an unexpected state.
        /// </summary>
        /// <param name="state">Unexpected state name.</param>
        /// <returns>Error message.</returns>
        protected static string GetUnexpectedStateErrorMessage(string state)
        {
            StringBuilder result = new StringBuilder();
            result.Append("Unexpected state ").Append(state);
            result.Append('.');
            return result.ToString();
        }

        /// <summary>
        /// This produces a warning message when a character is accepted by default.
        /// </summary>
        /// <param name="c">The character accepted by default.</param>
        /// <param name="stateName">The parser state name.</param>
        /// <returns>The warning message.</returns>
        protected string GetCharDefaultAcceptWarningMessage(char c, string stateName)
        {
            StringBuilder result = new StringBuilder();
            result.Append(GetCharacterPositionMessagePrefix());
            result.Append("Character ");
            result.Append(ParserInputStream.ToNameString(c));
            result.Append(" defaulted from the ");
            result.Append(stateName);
            result.Append(" state");
            return result.ToString();
        }

        /// <summary>
        /// Produces the error message for invalid name character.
        /// </summary>
        /// <param name="c">The invalid character.</param>
        /// <param name="stateName">The parser state.</param>
        /// <returns>The error message.</returns>
        protected string GetInvalidCharErrorMessage(char c, string stateName)
        {
            StringBuilder result = new StringBuilder();
            result.Append(GetCharacterPositionMessagePrefix());
            result.Append("Character ");
            result.Append(ParserInputStream.ToNameString(c));
            result.Append(" cannot be accepted from the ");
            result.Append(stateName);
            result.Append(" state");
            return result.ToString();
        }

        /// <summary>
        /// This method reports that there does not exist an edge from the current
        /// state which matches the next character from input.
        /// </summary>
        /// <param name="edge">Next input character.</param>
        /// <param name="stateName">Current parse state.</param>
        /// <returns>Error message for when invalid character found.</returns>
        protected string GetEdgeUnknownErrorMessage(int edge, string stateName)
        {
            StringBuilder result = new StringBuilder();
            result.Append(GetCharacterPositionMessagePrefix());
            result.Append("No edge labelled ");
            result.Append(ParserInputStream.ToNameString(edge));
            result.Append(" from the ");
            result.Append(stateName);
            result.Append(" state");
            return result.ToString();
        }

        /// <summary>
        /// Produces the line number and column number to be added to error
        /// and warning messages.
        /// </summary>
        /// <returns>Character position prefix.</returns>
        protected string GetCharacterPositionMessagePrefix()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Ln: ").Append(InputStream.LineNo.ToString().PadLeft(4)).Append("   ");
            result.Append("Col: ").Append(InputStream.ColNo.ToString().PadLeft(3)).Append("   ");
            return result.ToString();
        }

        /// <summary>
        /// This method is used to report the final status of the parser.
        /// </summary>
        /// <returns>The completion report as a string.</returns>
        public string GetCompletionReport()
        {
            StringBuilder report = new StringBuilder();
            report.Append("Parsed ");
            report.Append(InputStream.LineNo);
            report.Append(" line");
            if (InputStream.LineNo > 1) report.Append('s');
            report.Append(" containing ");
            report.Append(InputStream.CharCount);
            report.Append(" characters.");
            if ((NumErrors + NumRecoveries) > 0 || NumWarnings > 0)
            {
                report.Append(" Reported ");
                if ((NumErrors + NumRecoveries) > 0)
                {
                    report.Append(NumErrors + NumRecoveries);
                    report.Append(" error");
                    if (NumErrors + NumRecoveries != 1)
                    {
                        report.Append('s');
                    }
                    if (NumRecoveries > 0)
                    {
                        report.Append(" (recovered ");
                        report.Append(NumRecoveries).Append(')');
                    }
                    if (NumWarnings == 0)
                    {
                        report.Append('.');
                    }
                    else
                    {
                        report.Append(" and ");
                    }
                }
                if (NumWarnings > 0)
                {
                    report.Append(NumWarnings);
                    report.Append(" warnings.");
                }
            }
            return report.ToString();
        }

        #endregion
    }
}
