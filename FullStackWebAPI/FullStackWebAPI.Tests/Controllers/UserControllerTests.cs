using FullStackWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullStackWebAPI.Models;
using NUnit.Framework;
using System.Web.Http;
using System.Web.Http.Routing;

namespace FullStackWebAPI.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {

        [Test]
        public void GetAndPostTest()
        {
            // Arrange
            UserController controller = new UserController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/user")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });

            int userId = 0;

            // Act
            User userTest = new User();
            userTest.EmployeeId = "2222";
            userTest.FirstName = "fnameTest";
            userTest.LastName = "lnameTest";

            controller.Post(userTest);
            IEnumerable<User> result = controller.Get();
            bool resultBool = false;

            if (result.Count() > 0 && result.Any(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))))
            {
                resultBool = true;
                userId = result.First(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))).UserId;
                controller.DeleteUser(userId);
            }

            controller.Dispose();

            // Assert
            Assert.That(resultBool, Is.EqualTo(true));            
        }

        [Test]
        public void GetbyIdTest()
        {
            // Arrange
            UserController controller = new UserController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/user")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });
            int userId = 0;
            User result = null;

            // Act
            User userTest = new User();
            userTest.EmployeeId = "2222";
            userTest.FirstName = "fnameTest";
            userTest.LastName = "lnameTest";

            controller.Post(userTest);
            IEnumerable<User> resultUsers = controller.Get();

            if (resultUsers.Count() > 0 && resultUsers.Any(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))))
            {                
                userId = resultUsers.First(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))).UserId;
                result = controller.Get(userId);
                controller.DeleteUser(userId);
            }

            controller.Dispose();

            // Assert
            Assert.That(result.EmployeeId, Is.EqualTo("2222"));
            Assert.That(result.FirstName, Is.EqualTo("fnameTest"));
            Assert.That(result.LastName, Is.EqualTo("lnameTest"));
        }
                
        [Test]
        public void PutTest()
        {
            // Arrange
            UserController controller = new UserController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/user")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });
            int userId = 0;            

            // Act
            User userTest = new User();
            userTest.EmployeeId = "2222";
            userTest.FirstName = "fnameTest";
            userTest.LastName = "lnameTest";

            controller.Post(userTest);
            IEnumerable<User> resultUsers = controller.Get();

            if (resultUsers.Count() > 0 && resultUsers.Any(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))))
            {
                userId = resultUsers.First(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))).UserId;                
            }

            User userModifyTest = controller.Get(userId);
            userModifyTest.FirstName = "nameChanged";
            controller.Put(userId, userModifyTest);
            User userTestForPut = controller.Get(userId);
            controller.DeleteUser(userId);
            controller.Dispose();

            // Assert
            Assert.That(userTestForPut.FirstName, Is.EqualTo("nameChanged"));
        }

        [Test]
        public void DeleteUserTest()
        {
            // Arrange
            UserController controller = new UserController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:50328/api/user")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });
            int userId = 0;

            // Act
            User userTest = new User();
            userTest.EmployeeId = "2222";
            userTest.FirstName = "fnameTest";
            userTest.LastName = "lnameTest";

            controller.Post(userTest);
            IEnumerable<User> resultUsers = controller.Get();

            if (resultUsers.Count() > 0 && resultUsers.Any(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))))
            {
                userId = resultUsers.First(x => (x.EmployeeId == "2222" && x.FirstName.Equals("fnameTest") && x.LastName.Equals("lnameTest"))).UserId;
            }

            int initialCount = resultUsers.Count();
            controller.DeleteUser(userId);
            int resultCount = controller.Get().Count();

            controller.Dispose();

            // Assert
            Assert.That(resultCount, Is.EqualTo(initialCount - 1));
        }
    }
}