using FullStackWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FullStackWebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private ProjectManagementContext _db;

        public UserController()
        {
            _db = new ProjectManagementContext();
            _db.Configuration.ProxyCreationEnabled = false;
        }

        // GET api/user
        public IEnumerable<User> Get()
        {
            return _db.Users;
        }

        // GET api/user/5
        public User Get(int id)
        {
            User user = _db.Users.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return user;
        }

        // POST api/user
        public HttpResponseMessage Post([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/user/5
        public HttpResponseMessage Put(int id, [FromBody]User user)
        {
            if (ModelState.IsValid && id == user.UserId)
            {
                _db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    _db.SaveChanges();
                }
                catch(DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/user/5
        public HttpResponseMessage DeleteUser(int id)
        {
            User user = _db.Users.Find(id);
            if(user==null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _db.Users.Remove(user);

            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
