using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace StartupLoader
{
    class AppSetting : ConfigurationElement
    {
        public AppSetting()
        {

        }

        [ConfigurationProperty("path", DefaultValue = "")]
        public String path
        {
            get { return (String)this["path"]; }
            set { }
        }

        [ConfigurationProperty("arguments", DefaultValue = "")]
        public String arguments
        {
            get { return (String)this["arguments"]; }
            set { }
        }

        [ConfigurationProperty("label", DefaultValue = "")]
        public String label
        {
            get { return (String)this["label"]; }
            set { }
        }

        [ConfigurationProperty("restart", DefaultValue = "0", IsRequired =false)]
        public String restart
        {
            get { return (String)this["restart"]; }
            set { }
        }
    }

    class AppSettingsSection : ConfigurationSection
    {
        private AppSettingsSection() { }

        [ConfigurationProperty("Applications")]
        [ConfigurationCollection(typeof(AppSetting), AddItemName = "AppSetting")]
        public AppSettingsCollection Applications
        {
            get { return (AppSettingsCollection)this["Applications"]; }
        }
    }

    [ConfigurationCollection(typeof(AppSetting))]
    public class AppSettingsCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "AppSetting";

        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AppSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppSetting)element).path;
        }
    }
}
