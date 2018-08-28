using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace StartupLoader.Models
{
    public class RegistryManager
    {
        private string reg_root;
        RegistryKey rkHKLM;

        public RegistryManager(string r)
        {
            reg_root = r;
            rkHKLM = Registry.LocalMachine;

        }

        public bool WriteValue(string key, string val)
        {
            RegistryKey r = rkHKLM.CreateSubKey(reg_root);
            r.SetValue(key, val);
            return true;
        }

        public string GetValue(string key)
        {
            RegistryKey k = rkHKLM.OpenSubKey(reg_root);
            return (string)k.GetValue(key);
        }
    }
}
