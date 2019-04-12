using FullStackWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FullStackWebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ParentTaskController : ApiController
    {
        private ProjectManagementContext _db;

        public ParentTaskController()
        {
            _db = new ProjectManagementContext();
            _db.Configuration.ProxyCreationEnabled = false;
        }

        // GET api/<controller>
        public IEnumerable<ParentTask> Get()
        {
            return _db.ParentTasks;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ParentTask parentTask)
        {
            if (ModelState.IsValid)
            {
                _db.ParentTasks.Add(parentTask);
                _db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, parentTask);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = parentTask.ParentTaskId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if(_db!=null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}