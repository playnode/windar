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

using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json.Linq;
using Windar.PlayerPlugin.Commands;

namespace Windar.PlayerPlugin
{
    public partial class PlayerTabPage : UserControl
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        delegate void AddResultHandler(PlayItem item);
        delegate void SetStatusHandler(string text);
        delegate void IncrementPollCountHandler();
        delegate void StopPlayingHandler();
        delegate void StateChangedHandler(MPlayer.State to, string msg);

        Scrobbler _scrobber;
        PlayerPlugin _plugin;
        MPlayer _player;

        internal PlayerPlugin Plugin
        {
            get { return _plugin; }
            set { _plugin = value; }
        }

        internal MPlayer Player
        {
            get { return _player; }
            set { _player = value; }
        }

        string _qid;
        int _pollCount;
        int _maxPollCount;

        internal PlayerTabPage(PlayerPlugin plugin)
        {
            InitializeComponent();
            playButton.Enabled = false;
            stopButton.Enabled = false;
            positionTrackbar.Enabled = false;
            Plugin = plugin;
            Player = null;
            _scrobber = null;
        }

        #region Event handlers

        #region Text input boxes

        void artistTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (artistTextbox.Text == "" || trackTextbox.Text == "") return;
            if (e.KeyCode == Keys.Enter) Resolve();
        }

        void trackTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (artistTextbox.Text == "" || trackTextbox.Text == "") return;
            if (e.KeyCode == Keys.Enter) Resolve();
        }

        void albumTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (artistTextbox.Text == "" || trackTextbox.Text == "") return;
            if (e.KeyCode == Keys.Enter) Resolve();
        }

        void trackTextbox_Enter(object sender, EventArgs e)
        {
            trackTextbox.SelectAll();
        }

        void artistTextbox_Enter(object sender, EventArgs e)
        {
            artistTextbox.SelectAll();
        }

        void albumTextbox_Enter(object sender, EventArgs e)
        {
            albumTextbox.SelectAll();
        }

        #endregion

        void PlayerTabPage_Load(object sender, EventArgs e)
        {
            if (CanScrobble()) _scrobber = new Scrobbler(this);
            artistTextbox.Focus();
        }

        void resolveButton_Click(object sender, EventArgs e)
        {
            Resolve();
        }

        void resetButton_Click(object sender, EventArgs e)
        {
            StopPlaying();
            queryTimer.Stop();
            ResetForm(true);
            artistTextbox.Focus();
        }

        void queryTimer_Tick(object sender, EventArgs e)
        {
            IncrementPollCount();

            // Stop the timer. Restart if required.
            queryTimer.Stop();

            // Get the first results.
            StringBuilder str = new StringBuilder();
            str.Append("Resolving (");
            str.Append(_pollCount).Append('/');
            str.Append(_maxPollCount).Append(')');
            SetStatus(str.ToString());
            PlaydarResults results = GetResults(_qid);
            foreach (PlayItem result in results.PlayItems)
            {
                // Don't add the new result if already listed.
                bool skip = false;
                foreach (DataGridViewRow row in resultsGrid.Rows)
                {
                    if (!((PlayItem) row.Tag).SId.Equals(result.SId)) continue;
                    skip = true;
                    break;
                }
                if (!skip) AddResult(result);
            }

            // Set a timer to get further results.
            if (!results.Solved && _pollCount <= _maxPollCount)
            {
                //queryTimer.Interval = results.PollInterval;
                queryTimer.Start();
            }
            else {
                if (results.Solved) SetStatus("Solved.");
                else
                {
                    if (resultsGrid.Rows.Count > 0)
                        SetStatus("Could not find an exact match for the query.");
                    else
                    {
                        ResetForm(false);
                        SetStatus("No results for the query found.");
                    }
                }
                if (resultsGrid.Rows.Count > 0)
                    playButton.Enabled = true;
            }

            // Deselect the last row added.
            resultsGrid.CurrentCell = null;
            if (resultsGrid.Rows.Count > 0) 
                resultsGrid.Rows[0].Selected = false;
        }

        void resultsGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Player != null && (Player.PlayerState == MPlayer.State.Playing || Player.PlayerState == MPlayer.State.Paused)) return;
            PlayItem item = GetSelectedItem();
            if (_scrobber != null)
                _scrobber.Start(item.Artist, item.Album, item.Track, item.Source, item.Duration);
            PlayItem(item);
        }

        void playButton_Click(object sender, EventArgs e)
        {
            PlayItem item = GetSelectedItem();
            
            if (Player == null)
            {
                if (resultsGrid.Rows.Count > 0)
                {
                    PlayItem(item);
                    if (_scrobber != null) 
                        _scrobber.Start(item.Artist, item.Album, item.Track, item.Source, item.Duration);
                }
            }
            else
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Player state = " + Player.PlayerState);

                switch (Player.PlayerState)
                {
                    case MPlayer.State.Playing:
                        Player.Pause();
                        if (_scrobber != null) 
                            _scrobber.Pause();
                        SetStatus("Paused.");
                        break;

                    case MPlayer.State.Paused:
                        Player.Resume();
                        if (_scrobber != null) 
                            _scrobber.Resume();
                        SetStatus(PlayerPlugin.GetPlayingMessage(item));
                        break;
                }
            }
        }

        void stopButton_Click(object sender, EventArgs e)
        {
            StopPlaying();
        }

        void volumeTrackbar_Scroll(object sender, EventArgs e)
        {
            if (Player != null)
                Player.Volume = volumeTrackbar.Value;
        }

        void positionTrackbar_Scroll(object sender, EventArgs e)
        {
            //TODO: Change track position.
        }

        void resultsGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (resultsGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) return;
            DeselectRows();
        }

        void progressTimer_Tick(object sender, EventArgs e)
        {
            if (Player != null && Player.PlayerState == MPlayer.State.Playing)
            {
                positionTrackbar.Value = Player.Progress;
            }
            else
            {
                if (Log.IsWarnEnabled)
                    Log.Warn("Stopping progress timer.");
                progressTimer.Stop();
            }
        }

        #endregion

        #region Playdar methods

        bool CanScrobble()
        {
            bool result = false;

            // Build the request URL.
            StringBuilder url = new StringBuilder();
            url.Append(Plugin.Host.Paths.LocalPlaydarUrl).Append("api/?method=stat");

            // Get and process result.
            string response = PlayerPlugin.WGet(url.ToString());
            if (response != null)
            {
                JObject json = JObject.Parse(response);
                result = json["capabilities"]["audioscrobbler"] != null;
            }

            return result;
        }

        void Resolve()
        {
            ResetForm(false);
            EnableQueryForm(false);
            _pollCount = 0;
            _qid = GetQId(artistTextbox.Text, trackTextbox.Text, albumTextbox.Text);
            resultsGrid.Focus();
            if (_qid == null) SetStatus("Failed to get a QId!");
            else
            {
                // Get the first results.
                PlaydarResults results = GetResults(_qid);
                foreach (PlayItem result in results.PlayItems)
                    AddResult(result);

                // Set a timer to get further results.
                if (!results.Solved)
                {
                    queryTimer.Interval = results.PollInterval;
                    _maxPollCount = results.PollLimit;
                    queryTimer.Start();
                }
                else
                {
                    SetStatus("Solved.");
                    if (resultsGrid.Rows.Count > 0) 
                        playButton.Enabled = true;
                }

                // Deselect the last row added.
                resultsGrid.CurrentCell = null;
                if (resultsGrid.Rows.Count > 0) 
                    resultsGrid.Rows[0].Selected = false;
            }
        }

        string GetQId(string artistName, string trackTitle, string albumName)
        {
            string result = null;

            // Build the request URL.
            StringBuilder url = new StringBuilder();
            url.Append(Plugin.Host.Paths.LocalPlaydarUrl).Append("api/?method=resolve");
            url.Append("&artist=").Append(artistName);
            url.Append("&album=").Append(albumName);
            url.Append("&track=").Append(trackTitle);
            if (Log.IsDebugEnabled) Log.Debug("GetQId " + url);

            // Get and process result.
            string response = PlayerPlugin.WGet(url.ToString());
            if (response != null)
            {
                JObject json = JObject.Parse(response);
                result = DeQuotify(json["qid"]);
                if (Log.IsDebugEnabled) Log.Debug("GetQId result: " + result);
            }

            return result;
        }

        PlaydarResults GetResults(string qid)
        {
            // Build the request URL.
            StringBuilder url = new StringBuilder();
            url.Append(Plugin.Host.Paths.LocalPlaydarUrl).Append("api/?method=get_results");
            url.Append("&qid=").Append(qid);
            if (Log.IsDebugEnabled) Log.Debug("GetResults " + url);

            // Get and process result.
            PlaydarResults results = new PlaydarResults();
            string response = PlayerPlugin.WGet(url.ToString());
            if (response != null)
            {
                JObject json = JObject.Parse(response);
                results.Solved = ToBool(json["solved"]);
                results.PollInterval = ToInt(json["poll_interval"]);
                results.PollLimit = ToInt(json["poll_limit"]);
                foreach (JToken result in json["results"].Children())
                {
                    PlayItem item = new PlayItem();
                    item.SId = DeQuotify(result["sid"]);
                    item.Artist = DeQuotify(result["artist"]);
                    item.Track = DeQuotify(result["track"]);
                    item.Album = DeQuotify(result["album"]);
                    item.MimeType = DeQuotify(result["mimetype"]);
                    item.Score = ToFloat(result["score"]);
                    item.Duration = ToInt(result["duration"]);
                    item.Bitrate = ToInt(result["bitrate"]);
                    item.Size = ToInt(result["size"]);
                    item.Source = DeQuotify(result["source"]);
                    if (Log.IsDebugEnabled) Log.Debug("SId: " + item.SId);
                    results.PlayItems.Add(item);
                }
            }

            return results;
        }

        #endregion

        #region JSON conversions

        static string DeQuotify(JToken token)
        {
            if (token == null) return null;
            string str = token.ToString();
            string result = str;

            if (str[0] == '"' && str[str.Length - 1] == '"')
                result = str.Substring(1, str.Length - 2);

            return result;
        }

        static float ToFloat(JToken token)
        {
            return token == null ?
                0 : Convert.ToSingle(token.ToString());
        }

        static int ToInt(JToken token)
        {
            return token == null ?
                0 : Convert.ToInt32(token.ToString());
        }

        static bool ToBool(JToken token)
        {
            return token == null ?
                false : Convert.ToBoolean(token.ToString());
        }

        #endregion

        #region Form state handling

        void ResetForm(bool resetQuery)
        {
            // Previous results.
            resultsGrid.Rows.Clear();

            // Status message.
            SetStatus("");

            // Player.
            if (Player != null && Player.PlayerState == MPlayer.State.Playing)
                Player.Stop();
            EnablePlayControls(false);

            // Query form.
            if (resetQuery)
            {
                artistTextbox.Text = "";
                trackTextbox.Text = "";
                albumTextbox.Text = "";
            }
            EnableQueryForm(true);
        }

        void EnableQueryForm(bool enable)
        {
            artistTextbox.Enabled = enable;
            trackTextbox.Enabled = enable;
            albumTextbox.Enabled = enable;
            resolveButton.Enabled = enable;
        }

        void EnablePlayControls(bool enable)
        {
            playButton.Enabled = enable;
            stopButton.Enabled = enable;
            positionTrackbar.Enabled = false;
        }

        #endregion

        #region Playlist

        void PlayItem(PlayItem item)
        {
            // Disable controls. Controls re-activated on mplayer state change.
            EnablePlayControls(false);
            resetButton.Enabled = false;

            // Build the request URL.
            StringBuilder url = new StringBuilder();
            url.Append(Plugin.Host.Paths.LocalPlaydarUrl).Append("sid/").Append(item.SId);
            if (Log.IsDebugEnabled) Log.Debug("Stream filename " + url);

            // Play the music!
            SetStatus("Requesting audio stream.");
            Player = new MPlayer(item, url.ToString(), this);
            Player.Volume = volumeTrackbar.Value;
            Player.RunAsync();
        }

        PlayItem GetSelectedItem()
        {
            if (resultsGrid.SelectedRows.Count == 0 && resultsGrid.Rows.Count > 0)
                resultsGrid.Rows[0].Selected = true;

            return (PlayItem) resultsGrid.SelectedRows[0].Tag;
        }

        static DataGridViewRow GetResultsRow(PlayItem item)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell cellArtist = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellTrack = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellAlbum = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellSource = new DataGridViewTextBoxCell();
            cellArtist.Value = item.Artist;
            cellTrack.Value = item.Track;
            cellAlbum.Value = item.Album;
            cellSource.Value = item.Source;
            row.Cells.Add(cellArtist);
            row.Cells.Add(cellTrack);
            row.Cells.Add(cellAlbum);
            row.Cells.Add(cellSource);
            row.Tag = item;
            return row;
        }

        void DeselectRows()
        {
            foreach (object obj in resultsGrid.SelectedRows) ((DataGridViewRow) obj).Selected = false;
            resultsGrid.CurrentCell = null;
        }

        #endregion

        #region Invokable methods

        internal void StopPlaying()
        {
            Application.DoEvents();
            if (InvokeRequired)
                BeginInvoke(new StopPlayingHandler(StopPlaying));
            else
            {
                progressTimer.Stop();
                EnablePlayControls(false);
                if (_scrobber != null) 
                    _scrobber.Stop();
                if (Player != null) Player.Stop();
                Player = null;
                SetStatus("Stopped.");
                DeselectRows();
            }
        }

        /// <summary>
        /// This method is only used by the MPlayer class instance.
        /// </summary>
        /// <param name="to">The new state of the MPlayer instance.</param>
        /// <param name="msg">A status message associated with the new state.</param>
        internal void StateChanged(MPlayer.State to, string msg)
        {
            Application.DoEvents();
            if (InvokeRequired)
                BeginInvoke(new StateChangedHandler(StateChanged), new object[] {to, msg});
            else
            {
                if (Log.IsInfoEnabled) Log.Info("Changing state to: " + to);
                switch (to)
                {
                    case MPlayer.State.Initial:
                    case MPlayer.State.Running:
                    case MPlayer.State.Caching:
                        EnablePlayControls(false);
                        resetButton.Enabled = false;
                        break;
                    case MPlayer.State.Playing:
                        progressTimer.Start();
                        EnablePlayControls(true);
                        resetButton.Enabled = false;
                        break;
                    case MPlayer.State.Paused:
                        progressTimer.Stop();
                        EnablePlayControls(true);
                        resetButton.Enabled = false;
                        break;
                    case MPlayer.State.Stopped:
                        progressTimer.Stop();
                        EnablePlayControls(false);
                        resetButton.Enabled = true;
                        playButton.Enabled = true;
                        if (_scrobber != null) 
                            _scrobber.Stop();
                        msg = "Stopped.";
                        break;
                    case MPlayer.State.Ended:
                        progressTimer.Stop();
                        EnablePlayControls(false);
                        resetButton.Enabled = true;
                        playButton.Enabled = true;
                        if (_scrobber != null) 
                            _scrobber.Stop();
                        positionTrackbar.Value = 100;
                        msg = "Finished.";
                        break;
                    case MPlayer.State.Error:
                        StopPlaying();
                        EnablePlayControls(false);
                        resetButton.Enabled = false;
                        break;
                    default:
                        if (Log.IsWarnEnabled)
                            Log.Warn("Unexpected state " + Player.PlayerState);
                        break;
                }
                if (msg != null) SetStatus(msg);
            }
        }

        void SetStatus(string text)
        {
            Application.DoEvents();
            if (!InvokeRequired) statusLabel.Text = text;
            else BeginInvoke(new SetStatusHandler(SetStatus), text);
        }

        void IncrementPollCount()
        {
            Application.DoEvents();
            if (!InvokeRequired) _pollCount++;
            else BeginInvoke(new IncrementPollCountHandler(IncrementPollCount));
        }

        void AddResult(PlayItem result)
        {
            Application.DoEvents();
            if (!InvokeRequired) resultsGrid.Rows.Add(GetResultsRow(result));
            else BeginInvoke(new AddResultHandler(AddResult), result);
            if (resultsGrid.Rows.Count > 0) playButton.Enabled = true;
        }

        #endregion
    }
}
