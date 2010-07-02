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

using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Playnode.ErlangTerms.Parser;

namespace Windar.TrayApp.Configuration
{
    public class TcpConfigFile : ErlangTermsDocument
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        NamedBoolean _listen;
        NamedInteger _port;
        NamedBoolean _share;
        NamedList _peers;
        NamedBoolean _fwd;
        NamedInteger _fwdDelay;
        NamedBoolean _rewriteIdentity;

        public bool Listen
        {
            get
            {
                var result = false;
                if (_listen == null) _listen = FindNamedBoolean("listen");
                if (_listen != null) result = _listen.Value;
                return result;
            }
            set
            {
                if (_listen == null) _listen = FindNamedBoolean("listen");
                if (_listen != null) _listen.Value = value;
                else
                {
                    _listen = new NamedBoolean("listen", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_listen);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        public int Port
        {
            get
            {
                var result = -1;
                if (_port == null) _port = FindNamedInteger("port");
                if (_port != null) result = _port.Value;
                return result;
            }
            set
            {
                if (_port == null) _port = FindNamedInteger("port");
                if (_port != null) _port.Value = value;
                else
                {
                    _port = new NamedInteger("port", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_port);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        public bool DefaultShare
        {
            get
            {
                var result = false;
                if (_share == null) _share = FindNamedBoolean("share");
                if (_share != null) result = _share.Value;
                return result;
            }
            set
            {
                if (_share == null) _share = FindNamedBoolean("share");
                if (_share != null) _share.Value = value;
                else
                {
                    _share = new NamedBoolean("share", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_share);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        #region Peers

        public List<PeerInfo> GetPeers()
        {
            var result = new List<PeerInfo>();
            if (_peers == null) _peers = FindNamedList("peers");
            if (_peers != null)
            {
                var list = (ListToken) _peers.GetValueTokens()[1];
                foreach (var tuple in list.GetTupleTokens())
                {
                    var values = tuple.GetValueTokens();
                    var host = ((StringToken) values[0]).Text;
                    var port = ((IntegerToken) values[1]).Value;
                    if (values.Count > 2 && values[2] != null && values[2] is AtomToken)
                    {
                        var share = ((BooleanToken) values[2]).Value;
                        result.Add(new PeerInfo(host, port, share));
                    }
                    else
                    {
                        result.Add(new PeerInfo(host, port));
                    }
                }
            }
            return result;
        }

        TupleToken GetPeerInfoTuple(string host, int port)
        {
            TupleToken result = null;
            if (_peers == null) _peers = FindNamedList("peers");
            if (_peers != null)
            {
                // Get the actual list from the peers tuple.
                var list = (ListToken) _peers.GetValueTokens()[1];
                foreach (var tuple in list.GetTupleTokens())
                {
                    // Try to match host and port.
                    var values = tuple.GetValueTokens();
                    if (!(values[0] is StringToken) || ((StringToken) values[0]).Text != host ||
                        !(values[1] is IntegerToken) || ((IntegerToken) values[1]).Value != port) continue;

                    result = tuple;
                }
            }
            return result;
        }

        public PeerInfo GetPeerInfo(string host, int port)
        {
            PeerInfo result = null;
            var tuple = GetPeerInfoTuple(host, port);
            if (tuple != null)
            {
                var values = tuple.GetValueTokens();
                if (values.Count > 2 && values[2] != null && values[2] is AtomToken)
                {
                    result = new PeerInfo(host, port, Convert.ToBoolean(((AtomToken) values[2]).Text));
                }
                else
                {
                    result = new PeerInfo(host, port);
                }
            }
            return result;
        }

        public void SetPeerInfo(string host, int port, bool share)
        {
            var tuple = GetPeerInfoTuple(host, port);
            if (_peers == null)
            {
                _peers = new NamedList("peers", new ListToken());
                Document.Tokens.Add(new WhitespaceToken("\n\n"));
                Document.Tokens.Add(new AddedComment());
                Document.Tokens.Add(_peers);
                Document.Tokens.Add(new TermEndToken());
            }
            if (tuple == null)
            {
                var n = _peers.List.CountValues();
                if (n == 0)
                {
                    // Check if there is already some leading newline or comment line.
                    var c = 0;
                    foreach (var token in _peers.List.Tokens)
                    {
                        if (!(token is WhitespaceToken)) break;
                        c++;
                        if (token is CommentToken) break;
                        if (((WhitespaceToken) token).Text == "\n") break;
                    }
                    if (c == 0) _peers.List.Tokens.Insert(_peers.List.Tokens.Count, new WhitespaceToken("\n"));
                }
                if (n > 0) _peers.List.Tokens.Insert(_peers.List.Tokens.Count - 1, new CommaToken());
                _peers.List.Tokens.Add(new WhitespaceToken("\n"));
                _peers.List.Tokens.Add(new AddedComment());
                tuple = new TupleToken();
                tuple.Tokens.Add(new StringToken(host));
                tuple.Tokens.Add(new CommaToken());
                tuple.Tokens.Add(new WhitespaceToken(" "));
                tuple.Tokens.Add(new IntegerToken(port));
                tuple.Tokens.Add(new CommaToken());
                tuple.Tokens.Add(new WhitespaceToken(" "));
                tuple.Tokens.Add(new BooleanToken(share));
                _peers.List.Tokens.Add(tuple);
                _peers.List.Tokens.Add(new WhitespaceToken("\n"));
            }
            else
            {
                var values = tuple.GetValueTokens();
                ((StringToken) values[0]).Text = host;
                ((IntegerToken) values[1]).Value = port;
                if (values.Count > 2) ((BooleanToken) values[2]).Value = share;
                else
                {
                    tuple.Tokens.Add(new CommaToken());
                    tuple.Tokens.Add(new WhitespaceToken(" "));
                    tuple.Tokens.Add(new BooleanToken(share));
                }
            }
        }

        public void RemovePeer(string host, int port)
        {
            //TODO: This is almost exactly the same code as in NamedList.RemoveStringsListItem() ... Try to use a common method.
            //TODO: When first item is removed, there are newlines being written. Fix.

            var previousTokens = new Stack<ParserToken>();
            foreach (var token in _peers.List.Tokens)
            {
                if (Log.IsDebugEnabled) Log.Debug("Token = " + token);
                if (token is TupleToken)
                {
                    var tuple = (TupleToken) token;

                    // Try to match host and port.
                    var values = tuple.GetValueTokens();
                    if (!(values[0] is StringToken) || ((StringToken) values[0]).Text != host ||
                        !(values[1] is IntegerToken) || ((IntegerToken) values[1]).Value != port) continue;

                    // Found peer. Now remove it from the list.
                    _peers.List.Tokens.Remove(token);

                    if (previousTokens.Peek() is WhitespaceToken
                        && !(previousTokens.Peek() is CommentToken))
                    {
                        // Remove preceeding whitespace.
                        while (previousTokens.Peek() is WhitespaceToken
                            && !(previousTokens.Peek() is CommentToken))
                        {
                            _peers.List.Tokens.Remove(previousTokens.Pop());
                        }

                        // Remove comma token.
                        if (previousTokens.Peek() is CommaToken)
                        {
                            _peers.List.Tokens.Remove(previousTokens.Pop());
                        }
                    }
                    else
                    {
                        // Remove previous Windar comment (if applicable).
                        if (previousTokens.Count > 0
                            && previousTokens.Peek() is CommentToken
                            && ((CommentToken) previousTokens.Peek()).Text.StartsWith(AddedComment.AddedCommentBegin))
                        {
                            _peers.List.Tokens.Remove(previousTokens.Pop());

                            // Remove previous newline.
                            if (previousTokens.Count > 0
                                && previousTokens.Peek() is WhitespaceToken
                                && ((WhitespaceToken) previousTokens.Peek()).Text == "\n\n")
                            {
                                _peers.List.Tokens.Remove(previousTokens.Pop());

                                // Remove comma token, if found.
                                if (previousTokens.Count > 0
                                    && previousTokens.Peek() is CommaToken)
                                {
                                    _peers.List.Tokens.Remove(previousTokens.Pop());
                                }
                            }
                        }
                    }

                    // Find position of first non-whitespace token.
                    var pos = -1;
                    foreach (var tok in _peers.List.Tokens)
                    {
                        pos++;
                        if (!(tok is WhitespaceToken)) break;
                        continue;
                    }

                    // Remove previous comma token.
                    if (_peers.List.Tokens[pos] is CommaToken) _peers.List.Tokens.Remove(_peers.List.Tokens[pos]);

                    // Remove last newline if no more items in list.
                    if (_peers.List.CountValues() == 0)
                    {
                        if (_peers.List.Tokens[pos] is WhitespaceToken
                            && ((WhitespaceToken) _peers.List.Tokens[pos]).Text == "\n")
                            _peers.List.Tokens.Remove(_peers.List.Tokens[pos]);
                    }

                    break;
                }
                previousTokens.Push(token);
                continue;
            }
        }

        #endregion

        public bool Forward
        {
            get
            {
                var result = false;
                if (_fwd == null) _fwd = FindNamedBoolean("fwd");
                if (_fwd != null) result = _fwd.Value;
                return result;
            }
            set
            {
                if (_fwd == null) _fwd = FindNamedBoolean("fwd");
                if (_fwd != null) _fwd.Value = value;
                else
                {
                    _fwd = new NamedBoolean("fwd", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_fwd);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        public int ForwardDelay
        {
            get
            {
                var result = -1;
                if (_fwdDelay == null) _fwdDelay = FindNamedInteger("fwd_delay");
                if (_fwdDelay != null) result = _fwdDelay.Value;
                return result;
            }
            set
            {
                if (_fwdDelay == null) _fwdDelay = FindNamedInteger("fwd_delay");
                if (_fwdDelay != null) _fwdDelay.Value = value;
                else
                {
                    _fwdDelay = new NamedInteger("fwd_delay", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_fwdDelay);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        public bool RewriteIdentity
        {
            get
            {
                var result = false;
                if (_rewriteIdentity == null) _rewriteIdentity = FindNamedBoolean("rewrite_identity");
                if (_rewriteIdentity != null) result = _rewriteIdentity.Value;
                return result;
            }
            set
            {
                if (_rewriteIdentity == null) _rewriteIdentity = FindNamedBoolean("rewrite_identity");
                if (_rewriteIdentity != null) _rewriteIdentity.Value = value;
                else
                {
                    _rewriteIdentity = new NamedBoolean("rewrite_identity", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_rewriteIdentity);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }
    }
}
