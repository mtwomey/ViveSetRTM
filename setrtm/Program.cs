using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamVR;

namespace setrtm
{
    class Program
    {
        private static string[] commandLineArguments;
        private static double targetNum = 1.0;
        private static Dictionary<string, bool> flags = new Dictionary<string, bool>();

        static void Main(string[] args)
        {
            SteamVRHelper.vrmonitorPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\SteamVR\\tools\\bin\\win32\\vrmonitor.exe";
            SteamVRHelper.vrsettingsPath = "C:\\Program Files (x86)\\Steam\\config\\steamvr.vrsettings";
            parseCommandLine();
            if (flags["help"])
            {
                printHelp();
                return;
            }

            if (flags["get"])
            {
                Console.Write(SteamVRHelper.getRenderTargetMultiplier());
                return;
            }

            if (targetNum > 0)
            {
                try
                {

                    if (flags["nr"])
                    {
                        SteamVRHelper.setRenderTargetMultiplier(targetNum);
                    } else
                    {
                        if (SteamVRHelper.isRunning())
                        {
                            SteamVRHelper.stopSteamVR();
                            SteamVRHelper.setRenderTargetMultiplier(targetNum);
                            SteamVRHelper.startSteamVR();
                        } else
                        {
                            SteamVRHelper.setRenderTargetMultiplier(targetNum);
                        }
                    }
                } catch (Exception ex)
                {
                    Console.Write("A very bad accident...\n\n");
                    Console.Write(ex.Message);
                }
            }

        }

        private static void parseCommandLine()
        {
            commandLineArguments = Environment.GetCommandLineArgs();
            flags["help"] = false;
            flags["nr"] = false;
            flags["get"] = false;
            foreach(string cla in commandLineArguments)
            {
                if (cla == "/?")
                {
                    flags["help"] = true;
                }
                if (cla == "/nr")
                {
                    flags["nr"] = true;
                }
                if (cla == "/get")
                {
                    flags["get"] = true;
                }
            }
            if (commandLineArguments.Length < 2)
            {
                flags["help"] = true;
            } else
            {
                try
                {
                    targetNum = Convert.ToDouble(commandLineArguments[1]);
                } catch (Exception ex)
                {
                    targetNum = 0d;
                }
            }
        }

        private static void printHelp()
        {
            Console.Write("Set the Steam Render Target Multiplier");
            Console.Write("\n\n");
            Console.Write("Usage: setrtm [TARGET NUMBER] [/nr]");
            Console.Write("\n\n");
            Console.Write("/nr - Do not restart SteamVR, even if it's running.");
            Console.Write("/get - Just get the current Render Target Multiplier and exit.");
        }
    }
}
