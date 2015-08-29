using System;
using DataDiscovery.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDiscoveryUnitTestProject
{
    [TestClass]
    public class CSVUnitTest
    {
        [TestMethod]
        public void TestSetColumnsNamesFromLine()
        {
            var dfCSVFile = new CSVFile("Unit Test File");

            dfCSVFile.SetColumnsNamesFromLine("H1,H2,H3,H4,H5");

            Assert.AreEqual(5, dfCSVFile.ElmentColumnsCounter);

            

        }
    }
}
