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

namespace Lyricbuddy
{
    public partial class frmLyrics : Form
    {
        SpotifyController spotifyController;
        public frmLyrics()
        {
            InitializeComponent();

            spotifyController = new SpotifyController();
            switch(spotifyController.Connect())
            {
                case SpotifyController.ConnectionStatus.SuccessfulConnection:
                    MessageBox.Show("Successful Connection");
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
            
        }
    }
}
