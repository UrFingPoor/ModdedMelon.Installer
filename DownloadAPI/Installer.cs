using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModdedMelonInstaller
{
    public class Installer
    {
        public async void InitDownload()
        {
            string zipPath = $"{Path.GetTempPath()}\\{Guid.NewGuid().ToString("N").Substring(0, 8)}.zip";
            using (OpenFileDialog fdb = new OpenFileDialog())
            {
                if(fdb.ShowDialog() == DialogResult.OK) 
                {
                    fdb.RestoreDirectory = true; 
                    if (Directory.Exists($"{Path.GetDirectoryName(fdb.FileName)}\\MelonLoader")) Directory.Delete($"{Path.GetDirectoryName(fdb.FileName)}\\MelonLoader", true);
                    await DownloadFile("https://cdn.wtfblaze.com/MelonLoader.zip", $"{zipPath}"); Task.WaitAll();
                    ZipFile.ExtractToDirectory(zipPath, $"{Path.GetDirectoryName(fdb.FileName)}");
                    MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [info] [+] Done!", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
        }
      
        public async Task DownloadFile(string Url, string filename)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(Url))
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        using (var resStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = File.Create(filename)) { resStream.CopyTo(fileStream); }
                        break;
                }
        }
    }
}
