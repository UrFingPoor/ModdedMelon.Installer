using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RBXLoader
{
    public class RBXLoaderAPI
    {
        //skidded WRDAPI DLL FUCK Using the WeAreDevs.dll all it does is download the flux injector then inject
        //exploit-main.dll that allows you to have coms from you to the game which allows you to send and execute lua. all cmds in the wWRD Dll are below. 
        //Cleaned the fuck out ur class which was more lines then a coke head could snort in a lifetime forfcksake
        private string luapipe = "WeAreDevsPublicAPI_Lua";
        private string luacpipe = "WeAreDevsPublicAPI_LuaC";
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WaitNamedPipe(string name, int timeout);
        public bool IsAPIAttached => NamedPipeExist(luapipe);
        public void SendLuaScript(string Script) => SMTP(luapipe, Script);
        public void LuaC_getglobal(string service) => SendLuaCScript($"getglobal {service}");
        public void LuaC_getfield(int index, string instance) => SendLuaCScript($"getglobal {index} {instance}" );
        public void LuaC_setfield(int index, string property) => SendLuaCScript($"setfield {index} {property}");
        public void LuaC_pushvalue(int index) => SendLuaCScript($"pushvalue {index}");
        public void LuaC_pushstring(string text) => SendLuaCScript($"pushstring {text}");
        public void LuaC_pushnumber(int number) => SendLuaCScript($"pushnumber {number}");
        public void LuaC_settop(int index) => SendLuaCScript($"settop {index}");
        public void LuaC_pushboolean(string value = "false") => SendLuaCScript($"pushboolean {value}");
        public void LuaC_gettop() => SendLuaCScript("gettop");
        public void LuaC_pushnil() => SendLuaCScript("pushnil");
        public void LuaC_next(int index) => SendLuaCScript("next");
        public void LuaC_pop(int quantity) => SendLuaCScript($"pop {quantity}");
        public void ConsolePrint(string text = "") => SendLuaScript($"rconsoleprint {text}");
        public void ConsoleWarn(string text = "") => SendLuaScript($"rconsolewarn {text}");
        public void ConsoleError(string text = "") => SendLuaScript($"rconsoleerr {text}");
        public void DoBTools(string username = "me") => SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/BTools.txt\"))()");
        public void Suicide(string username = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character:BreakJoints()");
        public void AddForcefield(string username = "me") => SendLuaScript("Instance.new(\"ForceField\", game:GetService(\"Players\").LocalPlayer.Character)");
        public void RemoveForceField(string username = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character.ForceField:Destroy()");
        public void ToggleFloat(string username = "me") => SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/Float Character.txt\"))()");
        public void RemoveArms(string username = "me") => SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/Remove Arms.txt\"))()");
        public void RemoveLegs(string username = "me") => SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/Remove Legs.txt\"))()");
        public void AddFire(string username = "me") => SendLuaScript("Instance.new(\"Fire\", game:GetService(\"Players\").LocalPlayer.Character.HumanoidRootPart)");
        public void RemoveFire(string username = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character.HumanoidRootPart.Fire:Destroy()");
        public void AddSparkles(string username = "me") => SendLuaScript("Instance.new(\"Sparkles\", game:GetService(\"Players\").LocalPlayer.Character.HumanoidRootPart)");
        public void RemoveSparkles(string username = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character.HumanoidRootPart.Sparkles:Destroy()");
        public void AddSmoke(string username = "me") => SendLuaScript("Instance.new(\"Smoke\", game:GetService(\"Players\").LocalPlayer.Character.HumanoidRootPart)");
        public void RemoveSmoke(string username = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character.HumanoidRootPart.Smoke:Destroy()");
        public void DoBlockHead(string username = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character.Head.Mesh:Destroy()");
        public void SetWalkSpeed(string username = "me", int value = 100) => SendLuaScript($"game:GetService(\"Players\").LocalPlayer.Character.Humanoid.WalkSpeed = {value}");
        public void ToggleClickTeleport() => SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/Click Teleport.txt\"))()");
        public void SetFogStart(int value = 0) => SendLuaScript($"game:GetService(\"Lighting\").FogStart = {value}");
        public void SetFogEnd(int value = 0) => SendLuaScript($"game:GetService(\"Lighting\").FogEnd = {value}");
        public void SetJumpPower(int value = 100) => SendLuaScript($"game:GetService(\"Players\").LocalPlayer.Character.Humanoid.JumpPower = {value}");
        public void TeleportToPlayer(string targetUsername = "me") => SendLuaScript("game:GetService(\"Players\").LocalPlayer.Character:MoveTo(game:GetService(\"Players\"):FindFirstChild(" + targetUsername + ").Character.HumanoidRootPart.Position)");
        public void LuaC_pcall(int numberOfArguments, int numberOfResults, int ErrorFunction) => SendLuaCScript($"pushnumber {numberOfArguments} {numberOfResults} {ErrorFunction}");
        public void RemoveLimbs(string username = "me")
        {
            SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/Remove Arms.txt\"))()");
            SendLuaScript("loadstring(game:HttpGet(\"https://cdn.wearedevs.net/scripts/Remove Legs.txt\"))()");
        }
        public void SendLuaCScript(string Script)
        {
            foreach (string input in Script.Split("\r\n".ToCharArray()))
            {
                try
                {
                    SMTP(luacpipe, input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
        public static bool NamedPipeExist(string pipeName)
        {
            bool result;
            try
            {
                int timeout = 0;
                if (!WaitNamedPipe(Path.GetFullPath(string.Format("\\\\.\\pipe\\{0}", pipeName)), timeout))
                {
                    switch (Marshal.GetLastWin32Error())
                    {
                        case 0:
                            return false;
                        case 2:
                            return false;
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        private void SMTP(string pipe, string input)
        {
            if (NamedPipeExist(pipe))
            {
                try
                {
                    using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", pipe, PipeDirection.Out))
                    {
                        namedPipeClientStream.Connect();
                        using (StreamWriter streamWriter = new StreamWriter(namedPipeClientStream))
                        {
                            streamWriter.Write(input);
                        }
                    }
                    return;
                }
                catch (IOException)
                {
                    Console.WriteLine("Error occured sending message to the game!", "Connection Failed!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return;
                }
            }
            Console.WriteLine("Error occured. Did the dll properly inject?", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }   
    }
}