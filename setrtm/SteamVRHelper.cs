using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using WinProcess;

namespace SteamVR
{
    /// <summary>
    /// Provides some helper functions for working with SteamVR
    /// </summary>
    static class SteamVRHelper
    {
        public static string VrmonitorPath = null;
        public static string VrsettingsPath = null;

        /// <summary>
        /// Sets the render target multiplier in the file
        /// </summary>
        /// <param name="n"></param>
        public static void SetRenderTargetMultiplier(double n)
        {
            JObject config;
            try
            {
                config = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(VrsettingsPath));
            } catch (Exception)
            {
                throw new Exception("Could not open file: " + VrsettingsPath);
            }

            if ((double)config["steamvr"]["renderTargetMultiplier"] != n)
            {
                config["steamvr"]["renderTargetMultiplier"] = n;
                try
                {
                    JsonSave(VrsettingsPath, config, 3);
                    //string configString = JsonConvert.SerializeObject(config, Formatting.Indented);
                    //File.WriteAllText(filename, configString);
                }
                catch (Exception)
                {
                    throw new Exception("Could not save vrsettings to: " + VrsettingsPath);
                }
            }
        }

        /// <summary>
        /// Read the render target multiplier from the file and return it
        /// </summary>
        /// <returns></returns>
        public static double GetRenderTargetMultiplier()
        {
            JObject config;
            try
            {
                config = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(VrsettingsPath));
            }
            catch (Exception)
            {
                throw new Exception("Could not open file: " + VrsettingsPath);
            }
            return (double)config["steamvr"]["renderTargetMultiplier"];
        }

        /// <summary>
        /// Starts up SteamVR (vrmonitor.exe), which in turn starts several subprocesses
        /// </summary>
        public static void StartSteamVr()
        {
            try
            {
                Process.Start(VrmonitorPath);
            } catch (Exception)
            {
                throw new Exception("Could not start SteamVR: " + VrmonitorPath);
            }
        }

        /// <summary>
        /// Kills SteamVR and waits for it's sub-processes to close out as well (they die when vrmonitor dies)
        /// </summary>
        public static void StopSteamVr()
        {
            WinProcessHelper.KillAllByName("vrmonitor");
            WinProcessHelper.WaitForExit("vrserver");
            WinProcessHelper.WaitForExit("vrdashboard.exe");
            WinProcessHelper.WaitForExit("vrcompositor.exe");
        }

        /// <summary>
        /// Saves a JSON string to a file with the specified number of spaces for indenting
        /// 
        /// We have to so it this long way - see the commented out code in setRenderTargetMultiplier which is the short way, but you can't specify the spaces
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="obj"></param>
        /// <param name="indent"></param>
        private static void JsonSave(string filename, JObject obj, int indent)
        {
            using (FileStream fs = File.Open(filename, FileMode.Truncate)) // Important to Truncate, we want to overwrite the file completely (errors if not)
            {
                using (StreamWriter sw = new StreamWriter(fs)){
                    using (JsonTextWriter jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;
                        jw.IndentChar = ' ';
                        jw.Indentation = indent;
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jw, obj);
                        jw.Flush();
                    }
                }
            }
        }

        public static bool IsRunning()
        {
            return (WinProcessHelper.NumProcessByName("vrmonitor") > 0);
        }
    }
}
