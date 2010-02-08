﻿/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json.Linq;

namespace Windar.PlayerPlugin
{
    public partial class PlayerTabPage : UserControl
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        delegate void AddResultHandler(PlayItem item);
        delegate void SetStatusHandler(string text);
        delegate void IncrementRefreshTimerHandler();

        readonly Player _player;
        readonly Scrobbler _scrobber;

        internal PlayerPlugin Plugin { get; private set; }

        string _qid;
        int _pollCount;
        int _maxPollCount;

        class PlaydarResults
        {
            public int PollInterval { get; set; }
            public int PollLimit { get; set; }
            public bool Solved { get; set; }
            public List<PlayItem> PlayItems { get; private set; }

            public PlaydarResults()
            {
                PlayItems = new List<PlayItem>();
            }
        }

        internal PlayerTabPage(PlayerPlugin plugin)
        {
            InitializeComponent();
            EnablePlayControls(false);
            Plugin = plugin;
            _player = new Player(this);
            _scrobber = new Scrobbler(this);
        }

        #region Form state handling

        void ResetForm(bool resetQuery)
        {
            // Previous results.
            resultsGrid.Rows.Clear();

            // Status message.
            SetStatus("");

            // Player.
            EnablePlayControls(false);
            if (_player.State == Player.PlayState.Playing)
                _player.Stop();

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
            stopButton.Enabled = false;
            volumeTrackbar.Enabled = enable;
            positionTrackbar.Enabled = enable;
        }

        #endregion

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
                var results = GetResults(_qid);
                foreach (var result in results.PlayItems) AddResult(result);

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
                    if (resultsGrid.Rows.Count > 0) EnablePlayControls(true);
                }

                // Deselect the last row added.
                resultsGrid.CurrentCell = null;
                if (resultsGrid.Rows.Count > 0) resultsGrid.Rows[0].Selected = false;
            }
        }

        void StopPlaying()
        {
            stopButton.Enabled = false;
            _scrobber.Stop();
            _player.Stop();
            SetStatus("Stopped.");
        }

        string GetQId(string artistName, string trackTitle, string albumName)
        {
            string result = null;

            // Build the request URL.
            var url = new StringBuilder();
            url.Append(Plugin.Host.PlaydarPath).Append("api/?method=resolve");
            url.Append("&artist=").Append(artistName);
            url.Append("&album=").Append(albumName);
            url.Append("&track=").Append(trackTitle);
            if (Log.IsDebugEnabled) Log.Debug("GetQId " + url);

            // Get and process result.
            var response = PlayerPlugin.WGet(url.ToString());
            if (response != null)
            {
                var json = JObject.Parse(response);
                result = DeQuotify(json["qid"]);
                if (Log.IsDebugEnabled) Log.Debug("GetQId result: " + result);
            }

            return result;
        }

        PlaydarResults GetResults(string qid)
        {
            // Build the request URL.
            var url = new StringBuilder();
            url.Append(Plugin.Host.PlaydarPath).Append("api/?method=get_results");
            url.Append("&qid=").Append(qid);
            if (Log.IsDebugEnabled) Log.Debug("GetResults " + url);

            // Get and process result.
            var results = new PlaydarResults();
            var response = PlayerPlugin.WGet(url.ToString());
            if (response != null)
            {
                var json = JObject.Parse(response);
                results.Solved = ToBool(json["solved"]);
                results.PollInterval = ToInt(json["poll_interval"]);
                results.PollLimit = ToInt(json["poll_limit"]);
                foreach (var result in json["results"].Children())
                {
                    var item = new PlayItem
                    {
                        SId = DeQuotify(result["sid"]),
                        Artist = DeQuotify(result["artist"]),
                        Track = DeQuotify(result["track"]),
                        Album = DeQuotify(result["album"]),
                        MimeType = DeQuotify(result["mimetype"]),
                        Score = ToFloat(result["score"]),
                        Duration = ToInt(result["duration"]),
                        Bitrate = ToInt(result["bitrate"]),
                        Size = ToInt(result["size"]),
                        Source = DeQuotify(result["source"])
                    };
                    if (Log.IsDebugEnabled) Log.Debug("SId: " + item.SId);
                    results.PlayItems.Add(item);
                }
            }

            return results;
        }

        void PlayItem(PlayItem item)
        {
            // Build the request URL.
            var url = new StringBuilder();
            url.Append(Plugin.Host.PlaydarPath).Append("sid/").Append(item.SId);
            if (Log.IsDebugEnabled) Log.Debug("Stream filename " + url);

            // Update the status.
            var str = new StringBuilder();
            str.Append("Playing: ");
            str.Append(item.Artist).Append(" - ");
            str.Append(item.Track);
            SetStatus(str.ToString());

            // Play the music!
            _player.Play(url.ToString());
            
            // Enable the stop button.
            stopButton.Enabled = true;
        }

        PlayItem GetSelectedItem()
        {
            if (resultsGrid.SelectedRows.Count == 0 && resultsGrid.Rows.Count > 0)
                resultsGrid.Rows[0].Selected = true;

            return (PlayItem) resultsGrid.SelectedRows[0].Tag;
        }

        static DataGridViewRow GetResultsRow(PlayItem item)
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Artist });
            row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Track });
            row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Album });
            row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Source });
            row.Tag = item;
            return row;
        }

        static string DeQuotify(JToken token)
        {
            if (token == null) return null;
            var str = token.ToString();
            var result = str;

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

        #region Invokable methods

        void AddResult(PlayItem result)
        {
            if (!InvokeRequired) resultsGrid.Rows.Add(GetResultsRow(result));
            else BeginInvoke(new AddResultHandler(AddResult), result);
            if (resultsGrid.Rows.Count > 0) EnablePlayControls(true);
        }

        void SetStatus(string text)
        {
            if (!InvokeRequired) statusLabel.Text = text;
            else BeginInvoke(new SetStatusHandler(SetStatus), text);
        }

        void IncrementRefreshTimer()
        {
            if (!InvokeRequired) _pollCount++;
            else BeginInvoke(new IncrementRefreshTimerHandler(IncrementRefreshTimer));
        }

        #endregion

        #region Event handlers

        #region Text input boxes

        private void artistTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (artistTextbox.Text == "" || trackTextbox.Text == "") return;
            if (e.KeyCode == Keys.Enter) Resolve();
        }

        private void trackTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (artistTextbox.Text == "" || trackTextbox.Text == "") return;
            if (e.KeyCode == Keys.Enter) Resolve();
        }

        private void albumTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (artistTextbox.Text == "" || trackTextbox.Text == "") return;
            if (e.KeyCode == Keys.Enter) Resolve();
        }

        private void trackTextbox_Enter(object sender, EventArgs e)
        {
            trackTextbox.SelectAll();
        }

        private void artistTextbox_Enter(object sender, EventArgs e)
        {
            artistTextbox.SelectAll();
        }

        private void albumTextbox_Enter(object sender, EventArgs e)
        {
            albumTextbox.SelectAll();
        }

        #endregion

        private void PlayerTabPage_Load(object sender, EventArgs e)
        {
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
        }

        private void queryTimer_Tick(object sender, EventArgs e)
        {
            IncrementRefreshTimer();

            // Stop the timer. Restart if required.
            queryTimer.Stop();

            // Get the first results.
            var str = new StringBuilder();
            str.Append("Refreshing results (");
            str.Append(_pollCount).Append('/');
            str.Append(_maxPollCount).Append(')');
            SetStatus(str.ToString());
            var results = GetResults(_qid);
            foreach (var result in results.PlayItems)
            {
                // Don't add the new result if already listed.
                var skip = false;
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
                    EnablePlayControls(true);
            }

            // Deselect the last row added.
            resultsGrid.CurrentCell = null;
            if (resultsGrid.Rows.Count > 0) 
                resultsGrid.Rows[0].Selected = false;
        }

        private void resultsGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_player.State == Player.PlayState.Playing) return;
            var item = GetSelectedItem();
            _scrobber.Start(item.Artist, item.Album, item.Track, item.Source, item.Duration);
            PlayItem(item);
        }

        void playButton_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (Log.IsDebugEnabled) Log.Debug("Player state = " + _player.State);
            switch (_player.State)
            {
                case Player.PlayState.Initial:
                case Player.PlayState.Stopped:
                    if (resultsGrid.Rows.Count > 0)
                    {
                        PlayItem(item);
                        _scrobber.Start(item.Artist, item.Album, item.Track, item.Source, item.Duration);
                    }
                    break;
                case Player.PlayState.Playing:
                    _player.Pause();
                    _scrobber.Pause();
                    break;
                case Player.PlayState.Paused:
                    _player.Resume();
                    _scrobber.Resume();
                    break;
            }
            if (Log.IsDebugEnabled) Log.Debug("Player state = " + _player.State);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopPlaying();
        }

        private void volumeTrackbar_Scroll(object sender, EventArgs e)
        {

        }

        private void positionTrackbar_Scroll(object sender, EventArgs e)
        {

        }

        private void resultsGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (resultsGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) return;
            foreach (var obj in resultsGrid.SelectedRows) ((DataGridViewRow) obj).Selected = false;
            resultsGrid.CurrentCell = null;
        }

        #endregion
    }
}
