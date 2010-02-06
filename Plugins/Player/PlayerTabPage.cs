/*************************************************************************
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
using System.IO;
using System.Net;
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

        readonly PlayerPlugin _plugin;
        readonly Player _player;

        internal PlayerTabPage(PlayerPlugin plugin)
        {
            InitializeComponent();
            EnablePlayControls(false);
            _plugin = plugin;
            _player = new Player();
        }

        #region Form event handlers

        void resolveButton_Click(object sender, EventArgs e)
        {
            var qid = GetQId(artistTextbox.Text, trackTextbox.Text, albumTextbox.Text);
            if (qid == null) return;
            ResetForm();
            System.Threading.Thread.Sleep(200);
            var results = GetResults(qid);
            if (results.Count == 0) return;
            foreach (var result in results) resultsGrid.Rows.Add(GetResultsRow(result));
            resultsGrid.CurrentCell = null;
            if (resultsGrid.Rows.Count == 0) return;
            resultsGrid.Rows[0].Selected = false;
            EnablePlayControls(true);
        }

        void resetButton_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        void playButton_Click(object sender, EventArgs e)
        {
            if (_player.Playing)
            {
                _player.Pause();
            }
            else
            {
                var qid = GetQId(artistTextbox.Text, trackTextbox.Text, albumTextbox.Text);
                if (qid == null) return;
                System.Threading.Thread.Sleep(200);
                var results = GetResults(qid);
                if (results.Count > 0) PlaySelectedItem();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            _player.Stop();
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

        private void resultsGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            PlaySelectedItem();
        }

        #endregion

        #region Support functions

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

        void ResetForm()
        {
            EnablePlayControls(false);
            statusLabel.Text = "";
            artistTextbox.Text = "";
            trackTextbox.Text = "";
            albumTextbox.Text = "";
            if (_player.Playing) _player.Stop();
            resultsGrid.Rows.Clear();
        }

        void EnablePlayControls(bool enable)
        {
            playButton.Enabled = enable;
            stopButton.Enabled = enable;
            volumeTrackbar.Enabled = enable;
            positionTrackbar.Enabled = enable;
        }

        void PlaySelectedItem()
        {
            // Get the selected PlayItem instance.
            var item = GetSelectedItem();

            // Build the request URL.
            var url = new StringBuilder();
            url.Append(_plugin.Host.PlaydarPath).Append("sid/").Append(item.SId);
            if (Log.IsDebugEnabled) Log.Debug("Stream filename " + url);

            // Update the status.
            var str = new StringBuilder();
            str.Append("Loading ").Append(url);
            statusLabel.Text = str.ToString();

            // Play the music!
            _player.Play(url.ToString());
        }

        PlayItem GetSelectedItem()
        {
            return (PlayItem) resultsGrid.SelectedRows[0].Tag;
        }

        static string WGet(string url)
        {
            string result = null;

            if (Log.IsDebugEnabled) Log.Debug("WGet " + url);
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Timeout = 10000; // 10 secs
            request.UserAgent = "Windar";
            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var enc = Encoding.GetEncoding(1252);
                    var stream = new StreamReader(response.GetResponseStream(), enc);
                    result = stream.ReadToEnd();
                    response.Close();
                    stream.Close();
                    if (Log.IsDebugEnabled) Log.Debug("WGet result:\n" + result);
                }
            }
            catch (WebException ex)
            {
                switch (((HttpWebResponse) ex.Response).StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        if (Log.IsErrorEnabled) Log.Error("404 Not Found");
                        break; // Ignore.
                    default:
                        if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                        break;
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
            }
            return result;
        }

        static string DeQuotify(string str)
        {
            var result = str;

            if (str[0] == '"' && str[str.Length - 1] == '"')
                result = str.Substring(1, str.Length - 2);

            return result;
        }

        string GetQId(string artistName, string trackTitle, string albumName)
        {
            string result = null;

            // Build the request URL.
            var url = new StringBuilder();
            url.Append(_plugin.Host.PlaydarPath).Append("api/?method=resolve");
            url.Append("&artist=").Append(artistName);
            url.Append("&album=").Append(albumName);
            url.Append("&track=").Append(trackTitle);
            if (Log.IsDebugEnabled) Log.Debug("GetQId " + url);

            // Get and process result.
            var response = WGet(url.ToString());
            if (response != null)
            {
                var json = JObject.Parse(response);
                result = DeQuotify(json["qid"].ToString());
                if (Log.IsDebugEnabled) Log.Debug("GetQId result: " + result);
            }

            return result;
        }

        List<PlayItem> GetResults(string qid)
        {
            // Build the request URL.
            var url = new StringBuilder();
            url.Append(_plugin.Host.PlaydarPath).Append("api/?method=get_results");
            url.Append("&qid=").Append(qid);
            if (Log.IsDebugEnabled) Log.Debug("GetResults " + url);

            // Get and process result.
            var results = new List<PlayItem>();
            var response = WGet(url.ToString());
            if (response != null)
            {
                var json = JObject.Parse(response);
                foreach (var result in json["results"].Children())
                {
                    var item = new PlayItem
                                   {
                                       SId = DeQuotify(result["sid"].ToString()),
                                       Artist = DeQuotify(result["artist"].ToString()),
                                       Track = DeQuotify(result["track"].ToString()),
                                       Album = DeQuotify(result["album"].ToString()),
                                       MimeType = DeQuotify(result["mimetype"].ToString()),
                                       Score = Convert.ToSingle(result["score"].ToString()),
                                       Duration = Convert.ToInt32(result["duration"].ToString()),
                                       Bitrate = Convert.ToInt32(result["bitrate"].ToString()),
                                       Size = Convert.ToInt32(result["size"].ToString()),
                                       Source = DeQuotify(result["source"].ToString())
                                   };
                    if (Log.IsDebugEnabled) Log.Debug("SId: " + item.SId);
                    results.Add(item);
                }
            }

            return results;
        }

        #endregion
    }
}
