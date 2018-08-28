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
            RegistryManager rm = new Models.RegistryManager(@"SOFTWARE\MyKey");
            rm.WriteValue("Test", "val");
            string val = rm.GetValue("Test");
            Assert.That(val, Is.EqualTo("val"));
        }
        [TearDown]
        public void TestTearDown()
        {
            RegistryKey r = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MyKey");
            if (r.GetValue("Test") != null)
            {
                r.DeleteValue("Test");
            }
        }
    }
}
