using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;

namespace Lyricbuddy.Classes
{

    class SpotifyController : UserControl
    {
        private readonly SpotifyLocalAPIConfig _config;
        private SpotifyLocalAPI _spotify;
        private Track _currentTrack;

        public enum ConnectionStatus { SpotifyNotRunning, SuccessfulConnection, CancelledConnection, RetriedConnection }
        public SpotifyController()
        {

            _config = new SpotifyLocalAPIConfig
            {
                ProxyConfig = new SpotifyAPI.ProxyConfig()
            };

            _spotify = new SpotifyLocalAPI(_config);
            //TODO: Add event handlers
        }

        public ConnectionStatus Connect()
        {
            RetryConnection:
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                DialogResult reconnect = MessageBox.Show("Couldn't connect to the Spotify client. Would you like to retry?", "Retry Connection", MessageBoxButtons.YesNo);
                if (reconnect == DialogResult.Yes)
                {
                    goto RetryConnection;
                }
                else
                {
                    return ConnectionStatus.SpotifyNotRunning;
                }
            }

            if (_spotify.Connect())
            {
                _spotify.ListenForEvents = true;
                return ConnectionStatus.SuccessfulConnection;
            }
            else
            {
                DialogResult reconnect = MessageBox.Show("Couldn't connect to the Spotify client. Would you like to retry?", "Retry Connection", MessageBoxButtons.YesNo);
                if (reconnect == DialogResult.Yes)
                {
                    goto RetryConnection;
                }
                else
                {
                    return ConnectionStatus.CancelledConnection;
                }
            }

        }

        #region EventHandlers
        private void OnVolumeChange(object sender, VolumeChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnVolumeChange(sender, e)));
                return;
            }
            //TODO
        }

        private void OnTrackTimeChange(object sender, TrackTimeChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnTrackTimeChange(sender, e)));
                return;
            }
            //TODO
        }

        private void OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnTrackChange(sender, e)));
                return;
            }
            //TODO
        }

        private void OnPlayStateChange(object sender, PlayStateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnPlayStateChange(sender, e)));
                return;
            }
            //TODO
        }
        #endregion
    }
}