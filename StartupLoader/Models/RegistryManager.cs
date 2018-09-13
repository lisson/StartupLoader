using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;


namespace StartupLoader.Models
{
    public class RegistryManager
    {
        private string RegRoot;
        private string SubTree;
        RegistryKey rkHKLM;
        Logger logger;

        public RegistryManager(string r, bool HKLM = false)
        {
            RegRoot = r;
            SubTree = r + "\\StartupLoader";
            logger = NLog.LogManager.GetCurrentClassLogger();
            if (HKLM)
            {
                rkHKLM = Registry.LocalMachine;
            }
            else
            {
                rkHKLM = Registry.CurrentUser;
            }
            rkHKLM.OpenSubKey(SubTree);
        }

        public bool WriteValue(string key, string val)
        {
            RegistryKey r = rkHKLM.CreateSubKey(SubTree);
            r.SetValue(key, val);
            return true;
        }

        public string GetValue(string key)
        {
            RegistryKey k = rkHKLM.OpenSubKey(SubTree);
            if (k == null)
            {
                return "";
            }
            return (string)k.GetValue(key);
        }

        public void SetRunOnce(string val)
        {
            RegistryKey k = rkHKLM.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce");
            k.SetValue("StartupLoader", val);
        }

        public void Cleanup()
        {
            RegistryKey r = rkHKLM.CreateSubKey(RegRoot);
            if (r.OpenSubKey("StartupLoader") != null)
            {
                logger.Info($"Cleaning up {SubTree}");
                r.DeleteSubKeyTree("StartupLoader");
            }
        }
    }
}
