using NUnit.Framework;
using FullStackWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using FullStackWebAPI.Models;

namespace FullStackWebAPI.Controllers.Tests
{
    [TestFixture]
    public class ProjectControllerTests
    {        

        [Test]
        public void GetandPostTest()
        {
            // Arrange
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

            int userId = 0;
            int projectId = 0;

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
            IEnumerable<ProjectUI> result = controller.Get();
            bool resultBool = false;

            if (result.Count() > 0 && result.Any(x => (x.Project_Name == "unitTestingProject" && x.Priority==3 && x.StartDate==DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId==userId)))
            {
                resultBool = true;
                projectId = result.First(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)).ProjectId;
                controller.Delete(projectId);
            }

            controller.Dispose();
            userController.Dispose();

            // Assert
            Assert.That(resultBool, Is.EqualTo(true));
        }
        
        [Test]
        public void PutTest()
        {
            // Arrange
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

            int userId = 0;
            int projectId = 0;

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
            IEnumerable<ProjectUI> result = controller.Get();            

            if (result.Count() > 0 && result.Any(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)))
            {                
                projectId = result.First(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)).ProjectId;                
            }

            projectTest.ProjectId = projectId;
            projectTest.Project_Name = "projectNameUpdated";
            controller.Put(projectId, projectTest);

            result = controller.Get();
            ProjectUI projectTestResult = null;

            if (result.Count() > 0 && result.Any(x => (x.Project_Name == "projectNameUpdated" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)))
            {
                projectTestResult = result.First(x => (x.Project_Name == "projectNameUpdated" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId));
            }

            controller.Delete(projectId);
            controller.Dispose();
            userController.Dispose();

            // Assert
            Assert.That(projectTestResult.Project_Name, Is.EqualTo("projectNameUpdated"));
            Assert.That(projectTestResult.Priority, Is.EqualTo(3));
            Assert.That(projectTestResult.StartDate, Is.EqualTo(DateTime.Now.Date));
            Assert.That(projectTestResult.EndDate, Is.EqualTo(DateTime.Now.Date.AddDays(2)));
            Assert.That(projectTestResult.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void DeleteTest()
        {
            // Arrange
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

            int userId = 0;
            int projectId = 0;

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
            IEnumerable<ProjectUI> result = controller.Get();            

            if (result.Count() > 0 && result.Any(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)))
            {                
                projectId = result.First(x => (x.Project_Name == "unitTestingProject" && x.Priority == 3 && x.StartDate == DateTime.Now.Date && x.EndDate == DateTime.Now.Date.AddDays(2) && x.UserId == userId)).ProjectId;                
            }

            int initialCount = controller.Get().Count();
            controller.Delete(projectId);
            int finalCount = controller.Get().Count();

            controller.Dispose();
            userController.Dispose();

            // Assert
            Assert.That(finalCount, Is.EqualTo(initialCount - 1));
        }
    }
}