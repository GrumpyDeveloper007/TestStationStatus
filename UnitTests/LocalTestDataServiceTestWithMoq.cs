using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestStationStatusInfrastructure.Service;

namespace UnitTests
{
    class LocalTestDataServiceTestWithMoq
    {

        [TestMethod]
        public void FileUploadTest_usingMoq()
        {
            var objectUnderTest = new LocalTestDataService(new IpAddressService(new string[] { }));
            var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();

            files.Setup(x => x.Count).Returns(1);

            file.Setup(x => x.InputStream).Returns(stream);
            file.Setup(x => x.ContentLength).Returns((int)stream.Length);
            file.Setup(x => x.FileName).Returns("test.txt");

            files.Setup(x => x.Get(0).InputStream).Returns(file.Object.InputStream);

            // make sure we have a clean folder
            if (Directory.Exists(@".\E420MonitorTestPlan") == false)
            {
                Directory.CreateDirectory(@".\E420MonitorTestPlan");
            }
            if (File.Exists(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.Object.FileName) == true)
            {
                File.Delete(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.Object.FileName);
            }

            objectUnderTest.UploadFile(files.Object, "1.0.0.7",null);

            Assert.IsTrue(File.Exists(objectUnderTest.WorkingFolder + @"\E420MonitorTestPlan\" + file.Object.FileName));
        }
    }
}
