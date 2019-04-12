using FullStackWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FullStackWebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ProjectController : ApiController
    {
        private ProjectManagementContext _db;

        public ProjectController()
        {
            _db = new ProjectManagementContext();
            _db.Configuration.ProxyCreationEnabled = false;
        }

        // GET api/project
        public IEnumerable<ProjectUI> Get()
        {
            IList<ProjectUI> projectUIlist = new List<ProjectUI>();
            ProjectUI projectUI;

            //_db.Projects.Include(x => x.User);

            foreach (Project project in _db.Projects.Include(x => x.User).Include(y => y.Task))
            {
                projectUI = new ProjectUI(project);
                User user = project.User.FirstOrDefault();

                if (user != null)
                {
                    projectUI.UserId = user.UserId;
                    projectUI.Username = user.FirstName;
                }

                projectUI.TotalTasks = project.Task.Count();
                projectUI.TotalCompleted = project.Task.Count(x => x.Status.Equals("Completed"));

                projectUIlist.Add(projectUI);
            }

            return projectUIlist;
        }

        // GET api/project/5
        public string Get(int id)
        {
            return "method not used";
        }

        // POST api/project
        public HttpResponseMessage Post([FromBody]ProjectUI projectUI)
        {
            Project project = new Project(projectUI);            

            if (ModelState.IsValid)
            {
                User user = _db.Users.Find(projectUI.UserId);
                //Project projectTemp = _db.Projects.Include(x => x.User.Select(y => y.UserId == projectUI.UserId)).First(z => z.User.Any());
                var projectList = _db.Projects.Include(x => x.User).Where(y => y.User.Any(z => z.UserId == projectUI.UserId));
                Project projectTemp = null;

                if(projectList != null && projectList.Any())
                {
                    projectTemp = projectList.FirstOrDefault(tempUser => tempUser.User.Any());
                }

                if (projectTemp != null)
                {
                    projectTemp.User.Clear();
                }

                if (user != null)
                {
                    if (project.User == null)
                    {
                        project.User = new List<User>();
                    }
                    project.User.Add(user);
                }

                _db.Projects.Add(project);
                _db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, project);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = project.ProjectId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/project/5
        public HttpResponseMessage Put(int id, [FromBody]ProjectUI projectUI)
        {
            //Project project = new Project(projectUI);
            Project project = _db.Projects.Include(x => x.User).First(y => y.ProjectId == projectUI.ProjectId);
            //User previousUser = project.User.FirstOrDefault(); 
            // _db.Projects.Find(project.ProjectId)!=null?  project.User.First();
            User user = _db.Users.Find(projectUI.UserId);

            if (user != null && ModelState.IsValid)
            {
                project.User.Clear();
                project.User.Add(user);
                
                project.Project_Name = projectUI.Project_Name;
                project.StartDate = projectUI.StartDate;
                project.EndDate = projectUI.EndDate;
                project.Priority = projectUI.Priority;

                _db.Entry(project).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
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

        // DELETE api/project/5
        public HttpResponseMessage Delete(int id)
        {
            Project project = _db.Projects.Include(x => x.User).Where(y => y.ProjectId == id).FirstOrDefault();
            User user = project.User.First();

            if (project == null || user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            project.User.Clear();            
            _db.Projects.Remove(project);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
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