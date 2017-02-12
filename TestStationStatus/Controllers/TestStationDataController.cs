using System;
using System.Collections.Generic;
using System.Web.Http;

using TestStationStatusDomain.Entities;
using TestStationStatusInfrastructure;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatus.Controllers
{
    public class TestStationDataController : ApiController
    {
        ServerDataService _ServerDataService;

        public TestStationDataController()
        {
            _ServerDataService = PoorMansIOC.GetServerDataService();
        }



        // GET: api/API
        public IEnumerable<StatusUpdate> Get()
        {
            return _ServerDataService.GetCurrentStatusUpdate();
        }

        // GET: api/API/5
        public StatusUpdate Get(int id)
        {
            return null;
        }

        // POST: api/API
        public IHttpActionResult Post([FromBody]StatusUpdate value)
        {
            string errorMessage;
            errorMessage = _ServerDataService.SaveStatusUpdate(value);
            if (String.IsNullOrWhiteSpace (errorMessage ))
            {
                return Ok();
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        // PUT: api/API/5
        public void Put(int id, [FromBody]StatusUpdate value)
        {

        }

        // DELETE: api/API/5
        public void Delete(int id)
        {
        }
    }
}
