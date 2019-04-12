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
    public class TaskController : ApiController
    {

        private ProjectManagementContext _db;

        public TaskController()
        {
            _db = new ProjectManagementContext();
            _db.Configuration.ProxyCreationEnabled = false;
        }

        // GET api/task
        public IEnumerable<TaskUI> Get()
        {
            IList<TaskUI> taskUIlist = new List<TaskUI>();
            TaskUI taskUI;

            foreach(Task task in _db.Tasks.Include(x=>x.User))
            {
                taskUI = new TaskUI(task);
                var parentTaskList = _db.ParentTasks.Include(x => x.Task).Where(y => y.Task.Any(z=>z.TaskId == task.TaskId));
                var projectList = _db.Projects.Include(x => x.Task).Where(y => y.Task.Any(z => z.TaskId == task.TaskId));
                Project project = null;
                ParentTask parentTask = null;
                User user = null;

                if(parentTaskList!=null && parentTaskList.Any())
                {
                    parentTask = parentTaskList.First(x => x.Task.Any());
                }
                if (projectList != null && projectList.Any())
                {
                    project = projectList.First(x => x.Task.Any());
                }

                if (parentTask != null)
                {
                    taskUI.ParentTaskId = parentTask.ParentTaskId;
                    taskUI.Parent_task = parentTask.Parent_Task;
                }

                if (project != null)
                {
                    taskUI.ProjectId = project.ProjectId;
                    taskUI.Project_Name = project.Project_Name;
                }

                user = task.User.FirstOrDefault();                

                if (user!=null)
                {
                    taskUI.UserId = user.UserId;
                    taskUI.User_name = user.FirstName;
                }

                taskUIlist.Add(taskUI);
            }
            return taskUIlist;
        }

        // GET api/task/5
        public TaskUI Get(int id)
        {
            //return "value";

            var taskList = _db.Tasks.Include(x => x.User).Where(y => y.TaskId == id);
            Task task = null;

            if (taskList!=null && taskList.Any())
            {
                task = taskList.First();
            }
            
            TaskUI taskUI;

            if (task != null)
            {
                taskUI = new TaskUI(task);

                var parentTaskList = _db.ParentTasks.Include(x => x.Task).Where(y => y.Task.Any(z => z.TaskId == task.TaskId));
                var projectList = _db.Projects.Include(x => x.Task).Where(y => y.Task.Any(z => z.TaskId == task.TaskId));
                Project project = null;
                ParentTask parentTask = null;
                User user = null;

                if (parentTaskList != null && parentTaskList.Any())
                {
                    parentTask = parentTaskList.First(x => x.Task.Any());
                }
                if (projectList != null && projectList.Any())
                {
                    project = projectList.First(x => x.Task.Any());
                }

                if (parentTask != null)
                {
                    taskUI.ParentTaskId = parentTask.ParentTaskId;
                    taskUI.Parent_task = parentTask.Parent_Task;
                }

                if (project != null)
                {
                    taskUI.ProjectId = project.ProjectId;
                    taskUI.Project_Name = project.Project_Name;
                }

                if (task.User != null && task.User.Any())
                {
                    user = task.User.First();
                }

                if (user != null)
                {
                    taskUI.UserId = user.UserId;
                    taskUI.User_name = user.FirstName;
                }
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return taskUI;
        }

        // POST api/task
        public HttpResponseMessage Post([FromBody]TaskUI taskUI)
        {
            Task task = new Task(taskUI);
            if(taskUI.UserId!=null)
            {
                User user = _db.Users.Find(taskUI.UserId);

                if(user!=null)
                {
                    task.User = new List<User>();
                    task.User.Add(user);                    
                }
            }            

            _db.Tasks.Add(task);

            try
            {
                if (ModelState.IsValid)
                {
                    _db.SaveChanges();

                    if (taskUI.ParentTaskId != null)
                    {
                        var parentTaskList = _db.ParentTasks.Include(x => x.Task).Where(y => y.ParentTaskId == taskUI.ParentTaskId);
                        ParentTask parentTask = null;

                        if (parentTaskList != null && parentTaskList.Any())
                        {
                            parentTask = parentTaskList.First();
                        }

                        if (parentTask != null)
                        {
                            if (parentTask.Task == null)
                            {
                                parentTask.Task = new List<Task>();
                            }

                            parentTask.Task.Add(task);
                            _db.Entry(parentTask).State = EntityState.Modified;
                        }
                    }

                    if (taskUI.ProjectId != null)
                    {
                        var projectList = _db.Projects.Include(x => x.Task).Where(y => y.ProjectId == taskUI.ProjectId);
                        Project project = null;

                        if (projectList != null && projectList.Any())
                        {
                            project = projectList.First();
                        }

                        if (project != null)
                        {
                            if (project.Task == null)
                            {
                                project.Task = new List<Task>();
                            }

                            project.Task.Add(task);
                            _db.Entry(project).State = EntityState.Modified;
                        }
                    }

                    _db.SaveChanges();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch(DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, taskUI);
            httpResponseMessage.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = task.TaskId }));
            return httpResponseMessage;
        }

        // PUT api/task/5
        public HttpResponseMessage Put(int id, [FromBody]TaskUI taskUI)
        {
            Task task = _db.Tasks.Find(id);

            if (task == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (taskUI.UserId != null)
            {
                User user = _db.Users.Find(taskUI.UserId);

                if (user != null)
                {
                    task.User = new List<User>();
                    task.User.Add(user);
                }
            }            

            if (taskUI.ParentTaskId != null)
            {
                var previousParentTaskList = _db.ParentTasks.Include(x => x.Task).Where(y => y.Task.Any(z => z.TaskId == taskUI.TaskId));
                ParentTask previousParentTask = null;
                if (previousParentTaskList != null && previousParentTaskList.Any())
                {
                    previousParentTask = previousParentTaskList.First();

                    if (previousParentTask.Task != null)
                    {
                        previousParentTask.Task.Remove(task);
                        _db.Entry(previousParentTask).State = EntityState.Modified;
                    }
                }

                var parentTaskList = _db.ParentTasks.Include(x => x.Task).Where(y => y.ParentTaskId == taskUI.ParentTaskId);
                ParentTask parentTask = null;

                if(parentTaskList!=null && parentTaskList.Any())
                {
                    parentTask = parentTaskList.First();
                }

                if (parentTask != null)
                {
                    if(parentTask.Task==null)
                    {
                        parentTask.Task = new List<Task>();
                    }
                    
                    parentTask.Task.Add(task);
                    _db.Entry(parentTask).State = EntityState.Modified;
                }                
            }

            if (taskUI.ProjectId != null)
            {
                var previousProjectList = _db.Projects.Include(x => x.Task).Where(y => y.Task.Any(z => z.TaskId == taskUI.TaskId));
                Project previousProject = null;
                if (previousProjectList != null && previousProjectList.Any())
                {
                    previousProject = previousProjectList.First();

                    if (previousProject.Task != null)
                    {
                        previousProject.Task.Remove(task);
                        _db.Entry(previousProject).State = EntityState.Modified;
                    }
                }

                var projectList = _db.Projects.Include(x => x.Task).Where(y => y.ProjectId == taskUI.ProjectId);
                Project project = null;

                if (projectList != null && projectList.Any())
                {
                    project = projectList.First();
                }

                if (project != null)
                {
                    if (project.Task == null)
                    {
                        project.Task = new List<Task>();
                    }

                    project.Task.Add(task);
                    _db.Entry(project).State = EntityState.Modified;
                }                
            }
            task.Status = taskUI.Status;

            _db.Entry(task).State = EntityState.Modified;

            try
            {
                if (ModelState.IsValid)
                {
                    _db.SaveChanges();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK, taskUI);
        }

        // DELETE api/task/5
        public HttpResponseMessage Delete(int id)
        {
            Task task = _db.Tasks.Find(id);

            ParentTask parentTask = _db.ParentTasks.Include(t => t.Task.Select(x => x.TaskId == id)).First(y => y.Task.Any());

            if (task == null || parentTask == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            parentTask.Task.Clear();
            _db.Entry(parentTask).State = System.Data.Entity.EntityState.Modified;

            Project project = _db.Projects.Include(t => t.Task.Select(x => x.TaskId == id)).First(y => y.Task.Any());

            if (task == null || project == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            project.Task.Clear();
            _db.Entry(project).State = System.Data.Entity.EntityState.Modified;

            task.User.Clear();

            _db.Tasks.Remove(task);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, task);
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