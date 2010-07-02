/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
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

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace Playnode.ErlangTerms.Parser
{
    /// <summary>
    /// This class provides the character input stream to the Parser class.
    /// It supports a pushback queue, character counting and checksum.
    /// </summary>
    public class ParserInputStream
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public int ColNo { get; private set; }
        public int LineNo { get; private set; }
        public int CharCount { get; private set; }
        public int Checksum { get; private set; }

        private TextReader _stream;
        private readonly Stack<int> _pushbackQueue;

        public ParserInputStream(string text)
        {
            Checksum = (char) 0;
            _stream = new StringReader(text);
            _pushbackQueue = new Stack<int>();
        }

        public ParserInputStream(TextReader reader)
        {
            Checksum = (char) 0;
            _stream = reader;
            _pushbackQueue = new Stack<int>();
        }

        /// <summary>
        /// This method returns a single character from the input stream.
        /// </summary>
        /// <returns>Next character from input.</returns>
        public int NextChar()
        {
            int nextChar;
            do
            {
                // Pop last character on the queue if there are items pushed-back.
                // Otherwise read the next character from the input stream.
                nextChar = _pushbackQueue.Count > 0 ? _pushbackQueue.Pop() : _stream.Read();

                // Increment char count.
                if (nextChar != -1) CharCount++;
            } while (nextChar == '\r'); // Ignore linefeed.

            if (nextChar == '\n')
            {
                // Count new lines and track column position.
                LineNo += 1;
                ColNo = 0;
            }
            else
            {
                ColNo++;
            }

            // Update checksum and return next character.
            UpdateChecksum(nextChar);
            if (Log.IsDebugEnabled) Log.Debug("Char: " + ToNameString(nextChar));

            // Nullify the stream on EOF.
            if (nextChar == -1) _stream = null;

            return nextChar;
        }

        /// <summary>
        /// This method is used to update the checksum for the stream.
        /// </summary>
        /// <param name="c">The next character on the input stream.</param>
        private void UpdateChecksum(int c)
        {
            // http://atlas.csd.net/~cgadd/knowbase/CRC0013.HTM
            // xor char with the checksum
            if (c != -1) Checksum ^= c;
        }

        /// <summary>
        /// Push a single character back onto the stream to be read again.
        /// </summary>
        /// <param name="c">Character to push back.</param>
        public void PushBack(int c)
        {
            if (Log.IsDebugEnabled) Log.Debug("Pushback Char: '" + ToNameString(c) + '\'');
            _pushbackQueue.Push(c);
            CharCount--;
        }

        /// <summary>
        /// Push a whole string back into the stream.
        /// </summary>
        /// <param name="str">String to push back.</param>
        public void PushBack(string str)
        {
            if (Log.IsDebugEnabled) Log.Debug("Pushback String:\n" + str);
            for (var i = str.Length - 1; i > -1; i--)
            {
                _pushbackQueue.Push(str[i]);
                CharCount--;
            }
        }

        /// <summary>
        /// This method returns the string representation of a character.
        /// </summary>
        /// <param name="c">Character value to represent.</param>
        /// <returns>Label used to represent all characters in text logging.</returns>
        public static string ToNameString(int c)
        {
            switch (c)
            {
                case 0x00: return "0x" + c.ToString("X2").ToUpper() + " <NUL>";
                case 0x01: return "0x" + c.ToString("X2").ToUpper() + " <SOH>";
                case 0x02: return "0x" + c.ToString("X2").ToUpper() + " <STX>";
                case 0x03: return "0x" + c.ToString("X2").ToUpper() + " <ETX>";
                case 0x04: return "0x" + c.ToString("X2").ToUpper() + " <EOT>";
                case 0x05: return "0x" + c.ToString("X2").ToUpper() + " <ENQ>";
                case 0x06: return "0x" + c.ToString("X2").ToUpper() + " <ACK>";
                case 0x07: return "0x" + c.ToString("X2").ToUpper() + " <BEL>";
                case 0x08: return "0x" + c.ToString("X2").ToUpper() + " <BS>";
                case 0x09: return "0x" + c.ToString("X2").ToUpper() + " <HT>";
                case 0x0A: return "0x" + c.ToString("X2").ToUpper() + " <LF>";
                case 0x0B: return "0x" + c.ToString("X2").ToUpper() + " <VT>";
                case 0x0C: return "0x" + c.ToString("X2").ToUpper() + " <FF>";
                case 0x0D: return "0x" + c.ToString("X2").ToUpper() + " <CR>";
                case 0x0E: return "0x" + c.ToString("X2").ToUpper() + " <SO>";
                case 0x0F: return "0x" + c.ToString("X2").ToUpper() + " <SI>";
                case 0x10: return "0x" + c.ToString("X2").ToUpper() + " <DLE>";
                case 0x11: return "0x" + c.ToString("X2").ToUpper() + " <DC1>";
                case 0x12: return "0x" + c.ToString("X2").ToUpper() + " <DC2>";
                case 0x13: return "0x" + c.ToString("X2").ToUpper() + " <DC3>";
                case 0x14: return "0x" + c.ToString("X2").ToUpper() + " <DC4>";
                case 0x15: return "0x" + c.ToString("X2").ToUpper() + " <NAK>";
                case 0x16: return "0x" + c.ToString("X2").ToUpper() + " <SYN>";
                case 0x17: return "0x" + c.ToString("X2").ToUpper() + " <ETB>";
                case 0x18: return "0x" + c.ToString("X2").ToUpper() + " <CAN>";
                case 0x19: return "0x" + c.ToString("X2").ToUpper() + " <EM>";
                case 0x1A: return "0x" + c.ToString("X2").ToUpper() + " <SUB>";
                case 0x1B: return "0x" + c.ToString("X2").ToUpper() + " <ESC>";
                case 0x1C: return "0x" + c.ToString("X2").ToUpper() + " <FS>";
                case 0x1D: return "0x" + c.ToString("X2").ToUpper() + " <GS>";
                case 0x1E: return "0x" + c.ToString("X2").ToUpper() + " <RS>";
                case 0x1F: return "0x" + c.ToString("X2").ToUpper() + " <US>";
                case 0x20: return "0x" + c.ToString("X2").ToUpper() + " <SP>";
                case 0x21:
                case 0x22:
                case 0x23:
                case 0x24:
                case 0x25:
                case 0x26:
                case 0x27:
                case 0x28:
                case 0x29:
                case 0x2A:
                case 0x2B:
                case 0x2C:
                case 0x2D:
                case 0x2E:
                case 0x2F:
                case 0x30:
                case 0x31:
                case 0x32:
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                case 0x39:
                case 0x3A:
                case 0x3B:
                case 0x3C:
                case 0x3D:
                case 0x3E:
                case 0x3F:
                case 0x40:
                case 0x41:
                case 0x42:
                case 0x43:
                case 0x44:
                case 0x45:
                case 0x46:
                case 0x47:
                case 0x48:
                case 0x49:
                case 0x4A:
                case 0x4B:
                case 0x4C:
                case 0x4D:
                case 0x4E:
                case 0x4F:
                case 0x50:
                case 0x51:
                case 0x52:
                case 0x53:
                case 0x54:
                case 0x55:
                case 0x56:
                case 0x57:
                case 0x58:
                case 0x59:
                case 0x5A:
                case 0x5B:
                case 0x5C:
                case 0x5D:
                case 0x5E:
                case 0x5F:
                case 0x60:
                case 0x61:
                case 0x62:
                case 0x63:
                case 0x64:
                case 0x65:
                case 0x66:
                case 0x67:
                case 0x68:
                case 0x69:
                case 0x6A:
                case 0x6B:
                case 0x6C:
                case 0x6D:
                case 0x6E:
                case 0x6F:
                case 0x70:
                case 0x71:
                case 0x72:
                case 0x73:
                case 0x74:
                case 0x75:
                case 0x76:
                case 0x77:
                case 0x78:
                case 0x79:
                case 0x7A:
                case 0x7B:
                case 0x7C:
                case 0x7D:
                case 0x7E:
                    {
                        var result = new StringBuilder();
                        result.Append((char) c);
                        return result.ToString();
                    }
                case 0x7F: return "0x" + c.ToString("X2").ToUpper() + " <DEL>";
                case -1: return "EOF";
                default:
                    {
                        var result = new StringBuilder();
                        result.Append("0x");
                        result.Append(c.ToString("X2").ToUpper());
                        return result.ToString();
                    }
            }
        }
    }
}
