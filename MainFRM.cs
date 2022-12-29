using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ModdedMelonInstaller
{
    public partial class MainFRM : Form
    {
        #region "Mouse Move Events"
        private bool _dragging = false;
        private Point _start_point = new Point(0, 0);
        private void Object_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;  // _dragging is your variable flag
            _start_point = new Point(e.X, e.Y);
        }
        private void Object_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }
        private void Object_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }
        #endregion

        public Installer Installer = new Installer();
        public MainFRM() => InitializeComponent();
        private async void DownloadBTN_Click(object sender, EventArgs e) => await Installer.DownloadModdedMelon();
        private void EacMelonSite_Click(object sender, EventArgs e) => Process.Start("https://thats.gg/melonloader/");
        private void Exit_Click(object sender, EventArgs e) => Application.Exit();
    
    }
}
