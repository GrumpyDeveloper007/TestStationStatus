using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStationStatusInfrastructure.Service;
using System.Web;
using UnitTests.Helpers;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class LocalTestDataServiceTest
    {
        [TestMethod]
        public void Check_KillCurrentTestCase()
        {
            var objectUnderTest = new LocalTestDataService(new IpAddressService(new string[] { }));
            objectUnderTest.WorkingFolder = ".";

            // make sure we have a clean folder
            if (Directory.Exists (@".\e420")==false)
            {
                Directory.CreateDirectory(@".\e420");
            }
            if (File.Exists(@".\e420\KillSwitch.txt") == true)
            {
                File.Delete(@".\e420\KillSwitch.txt");
            }


            objectUnderTest.KillCurrentTestCase();

            Assert.IsTrue(System.IO.File.Exists(@".\e420\KillSwitch.txt"));

        }

        [TestMethod]
        public void Check_UploadFile()
        {
            var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var file = new TestPostedFileBase(stream, "NA", "test.txt");
            var objectUnderTest = new LocalTestDataService(new IpAddressService(new string[] { }));
            objectUnderTest.WorkingFolder = ".";

            // make sure we have a clean folder
            if (Directory.Exists(@".\E420MonitorTestPlan") == false)
            {
                Directory.CreateDirectory(@".\E420MonitorTestPlan");
            }
            if (File.Exists(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.FileName) == true)
            {
                File.Delete(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.FileName);
            }

            objectUnderTest.UploadFile (new TestPostedFileBase[] { file },"1.0.0.7","test@email.com" );

            Assert.IsTrue(File.Exists(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.FileName));
        }

        [TestMethod]
        public void Check_UploadFile_Overload()
        {
            var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var file = new TestPostedFileBase(stream, "NA", "test.txt");
            var filecollection = new TestHttpFileCollectionBase();
            filecollection.Files.Add(file);

            var objectUnderTest = new LocalTestDataService(new IpAddressService(new string[] { }));
            objectUnderTest.WorkingFolder = ".";

            // make sure we have a clean folder
            if (Directory.Exists(@".\E420MonitorTestPlan") == false)
            {
                Directory.CreateDirectory(@".\E420MonitorTestPlan");
            }
            if (File.Exists(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.FileName) == true)
            {
                File.Delete(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.FileName);
            }


            objectUnderTest.UploadFile(filecollection, "1.0.0.7",null);

            Assert.IsTrue(File.Exists(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.FileName));
        }


    }
}
