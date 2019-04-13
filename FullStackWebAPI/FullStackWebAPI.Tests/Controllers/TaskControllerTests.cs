using FullStackWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Http;
using System.Web.Http.Routing;
using FullStackWebAPI.Models;

namespace FullStackWebAPI.Controllers.Tests
{
    [TestFixture]
    public class TaskControllerTests
    {
        [Test]
        public void GetTest()
        {
            ProjectController controller = new ProjectController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "project" } });

            UserController userController = new UserController();
            userController.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            userController.Configuration = new System.Web.Http.HttpConfiguration();
            userController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            userController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });

            TaskController taskController = new TaskController();
            taskController.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            taskController.Configuration = new System.Web.Http.HttpConfiguration();
            taskController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            taskController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "task" } });

            ParentTaskController parentTaskController = new ParentTaskController();
            parentTaskController.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            parentTaskController.Configuration = new System.Web.Http.HttpConfiguration();
            parentTaskController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            parentTaskController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "task" } });

            int userId = 0;
            int projectId = 0;            
            int parentTaskId = 0;

            User userTest = new User();
            userTest.EmployeeId = "2222";
            userTest.FirstName = "fnameTest";
            userTest.LastName = "lnameTest";

            userController.Post(userTest);

            IEnumerable<User> userResult = userController.Get();

            if (userResult.Count() > 0 && userResult.Any(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))))
            {
                userId = userResult.First(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))).UserId;
                userTest = userController.Get(userId);
            }

            // Act
            ProjectUI projectTest = new ProjectUI();
            projectTest.Project_Name = "unitTestingProject";
            projectTest.Priority = 3;
            projectTest.StartDate = DateTime.Now.Date;
            projectTest.EndDate = DateTime.Now.Date.AddDays(2);
            projectTest.UserId = userId;
            projectTest.Username = userTest.FirstName;

            controller.Post(projectTest);
            IEnumerable<ProjectUI> projectResult = controller.Get();
            

            if (projectResult.Count() > 0 && projectResult.Any(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)))
            {
                projectTest = projectResult.First(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId));
                projectId = projectTest.ProjectId;    
            }

            ParentTask parentTaskTest = new ParentTask();
            parentTaskTest.Parent_Task = "parentTaskTest";

            parentTaskController.Post(parentTaskTest);
            var parentTaskResult = parentTaskController.Get();

            if (parentTaskResult.Count() > 0 && parentTaskResult.Any(x => (x.Parent_Task == "parentTaskTest")))
            {
                parentTaskTest = parentTaskResult.First(x => (x.Parent_Task == "parentTaskTest"));
                parentTaskId = parentTaskTest.ParentTaskId;
            }

            TaskUI taskTest = new TaskUI();
            taskTest.ParentTaskId = parentTaskId;
            taskTest.ProjectId = projectId;
            taskTest.Priority = 3;
            taskTest.Start_Date = DateTime.Now.Date;
            taskTest.End_date = DateTime.Now.Date.AddDays(2);
            taskTest.UserId = userId;

            taskController.Post(taskTest);
            var taskResult = taskController.Get();
            bool resultBool = false;

            if (taskResult.Count() > 0 && taskResult.Any(x => (x.ParentTaskId == parentTaskId && x.Priority == 3 && x.Start_Date == DateTime.Now.Date && x.End_date == DateTime.Now.Date.AddDays(2) && x.ProjectId == projectId)))
            {
                taskTest = taskResult.First(x => (x.ParentTaskId == parentTaskId && x.Priority == 3 && x.Start_Date == DateTime.Now.Date && x.End_date == DateTime.Now.Date.AddDays(2) && x.ProjectId == projectId));
                taskTest = taskController.Get(taskTest.TaskId);
                resultBool = true;

                taskController.Delete(taskTest.TaskId);
                parentTaskController.Delete(parentTaskId);
                controller.Delete(projectId);
                userController.DeleteUser(userId);
            }


            controller.Dispose();
            userController.Dispose();
            parentTaskController.Dispose();
            taskController.Dispose();

            // Assert
            Assert.That(resultBool, Is.EqualTo(true));
        }
                
        [Test]
        public void PutTest()
        {
            ProjectController controller = new ProjectController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "project" } });

            UserController userController = new UserController();
            userController.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            userController.Configuration = new System.Web.Http.HttpConfiguration();
            userController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            userController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });

            TaskController taskController = new TaskController();
            taskController.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            taskController.Configuration = new System.Web.Http.HttpConfiguration();
            taskController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            taskController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "task" } });

            ParentTaskController parentTaskController = new ParentTaskController();
            parentTaskController.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/project")
            };
            parentTaskController.Configuration = new System.Web.Http.HttpConfiguration();
            parentTaskController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            parentTaskController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "task" } });

            int userId = 0;
            int projectId = 0;            
            int parentTaskId = 0;

            User userTest = new User();
            userTest.EmployeeId = "2222";
            userTest.FirstName = "fnameTest";
            userTest.LastName = "lnameTest";

            userController.Post(userTest);

            IEnumerable<User> userResult = userController.Get();

            if (userResult.Count() > 0 && userResult.Any(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))))
            {
                userId = userResult.First(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))).UserId;
                userTest = userController.Get(userId);
            }

            // Act
            ProjectUI projectTest = new ProjectUI();
            projectTest.Project_Name = "unitTestingProject";
            projectTest.Priority = 3;
            projectTest.StartDate = DateTime.Now.Date;
            projectTest.EndDate = DateTime.Now.Date.AddDays(2);
            projectTest.UserId = userId;
            projectTest.Username = userTest.FirstName;

            controller.Post(projectTest);
            IEnumerable<ProjectUI> projectResult = controller.Get();


            if (projectResult.Count() > 0 && projectResult.Any(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)))
            {
                projectTest = projectResult.First(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId));
                projectId = projectTest.ProjectId;
            }

            ParentTask parentTaskTest = new ParentTask();
            parentTaskTest.Parent_Task = "parentTaskTest";

            parentTaskController.Post(parentTaskTest);
            var parentTaskResult = parentTaskController.Get();

            if (parentTaskResult.Count() > 0 && parentTaskResult.Any(x => (x.Parent_Task == "parentTaskTest")))
            {
                parentTaskTest = parentTaskResult.First(x => (x.Parent_Task == "parentTaskTest"));
                parentTaskId = parentTaskTest.ParentTaskId;
            }

            TaskUI taskTest = new TaskUI();
            taskTest.ParentTaskId = parentTaskId;
            taskTest.ProjectId = projectId;
            taskTest.Priority = 3;
            taskTest.Start_Date = DateTime.Now.Date;
            taskTest.End_date = DateTime.Now.Date.AddDays(2);
            taskTest.UserId = userId;

            taskController.Post(taskTest);
            var taskResult = taskController.Get();
            
            if (taskResult.Count() > 0 && taskResult.Any(x => (x.ParentTaskId == parentTaskId && x.Priority == 3 && x.Start_Date == DateTime.Now.Date && x.End_date == DateTime.Now.Date.AddDays(2) && x.ProjectId == projectId)))
            {
                taskTest = taskResult.First(x => (x.ParentTaskId == parentTaskId && x.Priority == 3 && x.Start_Date == DateTime.Now.Date && x.End_date == DateTime.Now.Date.AddDays(2) && x.ProjectId == projectId));                
            }

            taskTest.Priority = 15;

            taskController.Put(taskTest.TaskId, taskTest);

            taskTest = taskController.Get(taskTest.TaskId);

            taskController.Delete(taskTest.TaskId);
            parentTaskController.Delete(parentTaskId);
            controller.Delete(projectId);
            userController.DeleteUser(userId);

            controller.Dispose();
            userController.Dispose();
            parentTaskController.Dispose();
            taskController.Dispose();

            // Assert
            Assert.That(taskTest.Priority, Is.EqualTo(15));
        }        
    }
}