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

        public bool IsPlaying { get { return _spotify.GetStatus().Playing; } }
        public Track GetTrack { get { return _currentTrack; } }
        public double GetTrackTime { get { return _spotify.GetStatus().PlayingPosition;  } }
        public double GetVolume { get { return _spotify.GetStatus().Volume; } }

        public event EventHandler _OnTrackChanged;
        public event EventHandler _OnVolumeChange;
        public event EventHandler _OnTrackTimeChange;
        public event EventHandler _OnPlayStateChange;
        
        public SpotifyController()
        {
            
            _config = new SpotifyLocalAPIConfig
            {
                ProxyConfig = new SpotifyAPI.ProxyConfig()
            };

            _spotify = new SpotifyLocalAPI(_config);
            _spotify.OnVolumeChange += OnVolumeChange;
            _spotify.OnTrackTimeChange += OnTrackTimeChange;
            _spotify.OnTrackChange += OnTrackChange;
            _spotify.OnPlayStateChange += OnPlayStateChange;
        }
        
        public ConnectionStatus Connect()
        {
            RetryConnection:
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                // DRY
                DialogResult reconnect = MessageBox.Show("Couldn't connect to the Spotify client. Would you like to retry?", "Retry Connection", MessageBoxButtons.YesNo);
                if (reconnect == DialogResult.Yes)
                {
                    goto RetryConnection; // don't kill me
                }
                else
                {
                    return ConnectionStatus.SpotifyNotRunning;
                }
            }
            try
            {
                if (_spotify.Connect())
                {
                    FetchInformation();
                    _spotify.ListenForEvents = true;
                    return ConnectionStatus.SuccessfulConnection;
                }
                else
                {
                    // DRY
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
            catch
            {
                // DRY
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
        }
        
        private void FetchInformation()
        {
            StatusResponse status = _spotify.GetStatus();
            if (status == null) { return; }
            if (status.Track != null)
            {
                _currentTrack = status.Track;
            }
        }

        #region EventHandlers
        public void OnVolumeChange(object sender, VolumeChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnVolumeChange(sender, e)));
                return;
            }
            
            _OnVolumeChange?.Invoke(sender, null);
        }

        public void OnTrackTimeChange(object sender, TrackTimeChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnTrackTimeChange(sender, e)));
                return;
            }
            _OnTrackTimeChange?.Invoke(sender, null);
        }

        public void OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnTrackChange(sender, e)));
                return;
            }
            _currentTrack = e.NewTrack;
            _OnTrackChanged?.Invoke(sender, null);
        }

        public void OnPlayStateChange(object sender, PlayStateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnPlayStateChange(sender, e)));
                return;
            }
            _OnPlayStateChange?.Invoke(sender, null);
        }
        #endregion
    }
}