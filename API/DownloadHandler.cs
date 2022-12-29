using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RBXLoader
{
    public class DownloadHandler
    {
        public static void InitializeDownloadHandler()
        {
            try
            {
                dynamic Json = JObject.Parse(Task.Run(() => GetRequestAsync(new Uri($"https://cdn.wearedevs.net/software/exploitapi/latestdata.json"))).Result);               
                Config.Injector = Json["qdRFzx_exe"]; //injector download
                Config.DependencyDLL = Json["injDep"]; // injector dependency
                Config.ExecutorDLL = Json["exploit-module"].download; //Lua interpreter\Executor
                Config.Version = Json["exploit-module"].version; //epxloit version
                Config.Patched = Json["exploit-module"].patched; //patched check
                Config.BasePath = Environment.CurrentDirectory;
            }
            catch (Exception Error)
            {
               Console.WriteLine(Error.StackTrace, "Oh Shit!");
            } 
        }
        public static async void Download_Injector()
        {
            try
            {
                await DownloadFile(Config.Injector, $"{Config.BasePath}\\finj.exe");
                await DownloadFile(Config.DependencyDLL, $"{Config.BasePath}\\kernel64.sys.dll");
                await DownloadFile(Config.ExecutorDLL, $"{Config.BasePath}\\exploit-main.dll");
                Task.WaitAll();        
                LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Download Progress: Done!.\n");
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "finj.exe");
                LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Lauching RBX Injector.\n");
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.StackTrace);
            }
        }
        public static void Cleanup()
        {
            try
            {
                LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Cleaning Up ..\n");
                string[] FileNames = { "exploit-main.dll", "kernel64.sys.dll", "finj.exe" };
                for (int i = 0; i < FileNames.Length; i++)
                {
                    if (i == FileNames.Length) break;
                    if (File.Exists($"{Config.BasePath + FileNames[i]}"))
                    {
                        File.Delete($"{Config.BasePath + FileNames[i]}");
                        LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Removed {Config.BasePath + FileNames[i]}\n");
                    }
                }
            }
            catch(IOException Error)
            {
                Console.WriteLine(Error.StackTrace, "Oh Shit!");
            }
        }
        private static async Task<string> GetRequestAsync(Uri u)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                    response = await result.Content.ReadAsStringAsync();
            }
            return response;
        }

        public static void RBXLoader()
        {
            bool isRunning = Process.GetProcessesByName("RobloxPlayerBeta").Any();
            InitializeDownloadHandler(); Thread.Sleep(3000);
            if (Config.Patched.Contains("False") && isRunning && !Program.API.IsAPIAttached)
            {
                Cleanup(); Thread.Sleep(3000);
                Download_Injector(); Thread.Sleep(3000);
            }
            else if (!Config.Patched.Contains("False"))
            {
                LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] The Exploit Might Be Patched Check back in a few days!\n");
            }
            else if (!isRunning)
            {
                LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Roblox Must Be Running!\n");
            }
            else if (!Program.API.IsAPIAttached)
            {
                LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Exploit Has Already Been Injected!\n");
            }
        }

        public static async Task DownloadFile(string Url, string filename)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(Url))
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        LethalLog($"[{DateTime.Now:hh:mm:ss}] [%#Green%info%/%] [+] Downloading Required Files.\n");
                        using (var resStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = File.Create(filename)) { resStream.CopyTo(fileStream); }
                        break;
                }
        }

        public static void LethalLogo(string message)
        {
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + message.Length / 2) + "}", message);
        }
        public static void LethalLog(string message)
        {
            string[] ss = message.Split('%', '%');
            foreach (var s in ss)
                if (s.StartsWith("/")) Console.ResetColor();
                else if (s.StartsWith("#") && Enum.TryParse(s.Substring(1), out ConsoleColor c))
                    Console.ForegroundColor = c;
                else
                    Console.Write($"{s}");
           
        }
    }
}
