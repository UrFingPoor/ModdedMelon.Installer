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
        public async Task DownloadModdedMelon()
        {
            string zipPath = $"{Path.GetTempPath()}\\{Guid.NewGuid().ToString("N").Substring(0, 8)}.zip";
            using (OpenFileDialog fdb = new OpenFileDialog())
            {
                if (fdb.ShowDialog() == DialogResult.OK)
                {
                    fdb.RestoreDirectory = true;
                    if (Directory.Exists($"{Path.GetDirectoryName(fdb.FileName)}\\MelonLoader")) Directory.Delete($"{Path.GetDirectoryName(fdb.FileName)}\\MelonLoader", true);
                    using (var client = new HttpClient())
                    using (var response = await client.GetAsync("https://github.com/UrFingPoor/ModdedMelon.Installer/files/10948581/MelonLoader.zip"))
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                using (var resStream = await response.Content.ReadAsStreamAsync())
                                using (var fileStream = File.Create(zipPath)) { resStream.CopyTo(fileStream); }
                                ZipFile.ExtractToDirectory(zipPath, $"{Path.GetDirectoryName(fdb.FileName)}");
                                MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [info] [+] Done!", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case HttpStatusCode.NotFound:
                                MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [info] [+] Reqested data not found.", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case HttpStatusCode.BadRequest:
                                MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [error] [+] Bad request :(", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case HttpStatusCode.ServiceUnavailable:
                                MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [error] [+] Requested data seeems unavailable, please make sure you aren't connected to a vpn.", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case HttpStatusCode.Conflict:
                                MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [error] [+] Seems we are conflicting please contact owner :(", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case (HttpStatusCode)429: //ratelimited
                                MessageBox.Show($"[{DateTime.Now:hh:mm:ss}] [error] [+] Request is being ratelimited, try again in a bit.", "EAC Melon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                }
                return;
            }
        }
    }
}

