using System;
using System.Threading.Tasks;
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
        private async void DownloadBTN_Click(object sender, EventArgs e) => await Installer.DownloadModdedMelon();
    }
}
