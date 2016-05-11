using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass()]
    public class TestBase
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            UnityConfig.GetConfiguredContainer();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {

        }
    }
}