using Microsoft.VisualStudio.TestTools.UnitTesting;
using FullStackWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullStackWebAPI.Models;

namespace FullStackWebAPI.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void UserControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
            IEnumerable<User> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod()]
        public void GetbyIdTest()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
            User result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod()]
        public void PostTest()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
            controller.Post(new User());

            // Assert
        }

        [TestMethod()]
        public void PutTest()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
            controller.Put(5, new User());

            // Assert
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
            controller.DeleteUser(5);

            // Assert
        }
    }
}