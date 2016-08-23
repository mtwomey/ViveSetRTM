using System;
using System.Collections.Generic;
using System.Linq;
using SteamVR;

namespace setrtm
{
    class Program
    {
        //private static string[] commandLineArguments;
        private static double _targetNum = 1.0;
        private static Dictionary<string, bool> Flags = new Dictionary<string, bool>();
        private static string SteamDir = "C:\\Program Files (x86)\\Steam\\";

        static void Main()
        {
            ParseCommandLine();
            SteamVRHelper.VrmonitorPath = SteamDir + "steamapps\\common\\SteamVR\\tools\\bin\\win32\\vrmonitor.exe";
            SteamVRHelper.VrsettingsPath = SteamDir + "config\\steamvr.vrsettings";
            if (Flags["help"])
            {
                PrintHelp();
                return;
            }

            if (Flags["get"])
            {
                try
                {
                    Console.Write(SteamVRHelper.GetRenderTargetMultiplier());
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write("A very bad accident...\n\n");
                    Console.Write(ex.Message);
                    return;
                }
            }

            if (_targetNum > 0)
            {
                try
                {

                    if (Flags["nr"])
                    {
                        SteamVRHelper.SetRenderTargetMultiplier(_targetNum);
                    } else
                    {
                        if (SteamVRHelper.IsRunning())
                        {
                            SteamVRHelper.StopSteamVr();
                            SteamVRHelper.SetRenderTargetMultiplier(_targetNum);
                            SteamVRHelper.StartSteamVr();
                        } else
                        {
                            SteamVRHelper.SetRenderTargetMultiplier(_targetNum);
                        }
                    }
                } catch (Exception ex)
                {
                    Console.Write("A very bad accident...\n\n");
                    Console.Write(ex.Message);
                }
            }

        }

        private static void ParseCommandLine()
        {
            string[] commandLineArguments = Environment.GetCommandLineArgs();
            Flags["help"] = false;
            Flags["nr"] = false;
            Flags["get"] = false;
            foreach(string cla in commandLineArguments.Select(cla => cla.ToLower()).ToArray())
            {
                if (cla == "/?")
                {
                    Flags["help"] = true;
                }
                if (cla == "/nr")
                {
                    Flags["nr"] = true;
                }
                if (cla == "/get")
                {
                    Flags["get"] = true;
                }
                if (cla.StartsWith("/steamdir:"))
                {
                    SteamDir = cla.Replace("/steamdir:", "").Replace("\"", "");
                    if (SteamDir[SteamDir.Length - 1] != '\\')
                    {
                        SteamDir += "\\";
                    }
                }
            }
            if (commandLineArguments.Length < 2)
            {
                Flags["help"] = true;
            } else
            {
                try
                {
                    _targetNum = Convert.ToDouble(commandLineArguments[1]);
                } catch (Exception)
                {
                    _targetNum = 0d;
                }
            }
        }

        private static void PrintHelp()
        {
            Console.Write("Set the Steam Render Target Multiplier");
            Console.Write("\n\n");
            Console.Write("Usage: setrtm [TARGET NUMBER] [/nr]");
            Console.Write("\n\n");
            Console.Write("/nr - Do not restart SteamVR, even if it's running.\n");
            Console.Write("/get - Just get the current Render Target Multiplier and exit.\n");
            Console.Write("/steamdir - Option to set your steam directory. Example: /steamdir:\"D:\\Program Files (x86)\\Steam\\\"\n");
        }
    }
}
