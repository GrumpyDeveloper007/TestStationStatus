using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UnitTests.Helpers
{
    class TestHttpFileCollectionBase : HttpFileCollectionBase
    {
        public List<HttpPostedFileBase> Files { get; set; }

        //public override HttpPostedFileBase this[string name] { get { return Files[name]; } }
        public override HttpPostedFileBase this[int index] { get { return Files[index]; } }

        public override int Count { get { return Files.Count; } }


        public TestHttpFileCollectionBase()
        {
            Files = new List<HttpPostedFileBase>();
        }
    }
}
