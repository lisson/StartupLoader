using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StartupLoader.Models;
using Microsoft.Win32;

namespace StartupLoader.Tests
{
    [TestFixture]
    class RegistryManagerTest
    {
        [Test]
        public void ShouldAddRegistry()
        {
            RegistryManager rm = new Models.RegistryManager(@"SOFTWARE\MyKey", true);
            rm.WriteValue("Test", "val");
            string val = rm.GetValue("Test");
            Assert.That(val, Is.EqualTo("val"));
        }

        [Test]
        public void AddNestedRegistry()
        {
            RegistryManager rm = new Models.RegistryManager(@"SOFTWARE\MyKey\Level1\Level2");
            rm.WriteValue("test", "val");
            RegistryKey r = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MyKey\Level1\Level2");
            Assert.NotNull(r);
        }

        [Test]
        public void ShouldCleanup()
        {
            RegistryManager rm = new Models.RegistryManager(@"SOFTWARE\MyKey");
            rm.Cleanup();
            RegistryKey r = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MyKey\StartupLoader");
            Assert.IsNull(r);
        }

        [TearDown]
        public void TestTearDown()
        {
            RegistryKey r = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MyKey");
            if (r.OpenSubKey("StartupLoader") != null)
            {
                r.DeleteSubKeyTree("StartupLoader");
            }
            if (r.OpenSubKey("Level1") != null)
            {
                r.DeleteSubKeyTree("Level1");
            }
        }
    }
}
