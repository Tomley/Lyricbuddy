using Lyricbuddy.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;

namespace Lyricbuddy
{
    public partial class frmLyrics : Form
    {
        SpotifyController spotifyController;
        public frmLyrics()
        {
            InitializeComponent();

            spotifyController = new SpotifyController();
            spotifyController._OnTrackChanged += OnTrackChanged;

            switch (spotifyController.Connect())
            {
                case SpotifyController.ConnectionStatus.SuccessfulConnection:
                    break;
                case SpotifyController.ConnectionStatus.SpotifyNotRunning:
                    MessageBox.Show("Spotify not running.");
                    Environment.Exit(0);
                    break;
                case SpotifyController.ConnectionStatus.RetriedConnection:
                    break;
                case SpotifyController.ConnectionStatus.CancelledConnection:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
           
        }

        private void frmLyrics_Load(object sender, EventArgs e)
        {
            label1.Text = spotifyController.GetTrack.TrackResource.Name + " - " + spotifyController.GetTrack.ArtistResource.Name;
        }
        private void OnTrackChanged(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                UpdateLyrics();
            }));
        }
        
        private void UpdateLyrics()
        {
            label1.Text = spotifyController.GetTrack.TrackResource.Name + " - " + spotifyController.GetTrack.ArtistResource.Name;
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            GeniusScraper G = new GeniusScraper();

            string test = await G.SearchGeniusASync(textBox1.Text);

            var test1 = JsonConvert.DeserializeObject<GeniusApiObject.RootObject>(test);
            MessageBox.Show(test1.response.hits[0].result.title);
        }
    }
}
