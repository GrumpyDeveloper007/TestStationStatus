using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestStationStatusDomain.Entities;
using TestStationStatusInfrastructure;

namespace TestStationStatus.Controllers
{
    public class CompletedTestsController : Controller
    {
        private TestStationContext db = new TestStationContext();

        // GET: CompletedTests
        public ActionResult Index()
        {
            return View(new List< CompletedTest> { });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult PageData(IDataTablesRequest request)
        {
            // Nothing important here. Just creates some mock data.
            var data = db.CompletedTests;


            // Global filtering.
            // Filter is being manually applied due to in-memmory (IEnumerable) data.
            // If you want something rather easier, check IEnumerableExtensions Sample.
            var filteredData = data;

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            IEnumerable<CompletedTest> dataPage = filteredData;
            if (request != null)
            {
                if (request.Length != -1)
                {
                    dataPage = filteredData.OrderBy ((a) =>a.Id ).Skip(request.Start).Take(request.Length).ToList();
                }
            }

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }
    }
}
