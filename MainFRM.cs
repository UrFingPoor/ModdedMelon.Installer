using System;
using System.Windows.Forms;

namespace ModdedMelonInstaller
{
    public partial class MainFRM : Form
    {
        public Installer Installer = new Installer();
        public MainFRM()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) => Installer.InitDownload();
    }
}
