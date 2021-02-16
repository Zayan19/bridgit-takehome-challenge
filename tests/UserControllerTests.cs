using Xunit;
using Tasklify.DAL;
using Tasklify.Contracts;
using Tasklify.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace takehome.tests.csproj
{
    public class UserControllerTests
    {
        [Fact]
        public async Task UserControllerAddAndGetTest()
        {
            var uDAL = new DemoUsersDAL();
            var userController = new UsersController(uDAL);

            var result = await userController.GetAll();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var userList = Assert.IsType<List<TasklifyUser>>(okObjectResult.Value);
            Assert.Empty(userList);

            result = await userController.AddUser(new TasklifyUser(1, "testemail@gmail.com", "test"));
            okObjectResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<TasklifyUser>(okObjectResult.Value);

            Assert.Equal(1, value.Id);
            Assert.Equal("testemail@gmail.com", value.Email);
            Assert.Equal("test", value.Name);

            result = await userController.AddUser(new TasklifyUser(1, "", "test"));
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            var errorString = Assert.IsType<String>(objectResult.Value);
            Assert.Equal("Email cannot be empty or all whitespace.", errorString);

            await userController.AddUser(new TasklifyUser(2, "testemail2@gmail.com", "test2"));
            result = await userController.GetAll();
            okObjectResult = Assert.IsType<OkObjectResult>(result);
            userList = Assert.IsType<List<TasklifyUser>>(okObjectResult.Value);

            Assert.Collection(userList.OrderBy(user => user.Id), 
                user => {
                    Assert.Equal(1, user.Id);
                    Assert.Equal("testemail@gmail.com", user.Email);
                    Assert.Equal("test", user.Name);
                },
                user => {
                    Assert.Equal(2, user.Id);
                    Assert.Equal("testemail2@gmail.com", user.Email);
                    Assert.Equal("test2", user.Name);
                });
        }

        [Fact]
        public async Task UserControllerGetByIdOrEmailTest()
        {
            void CheckExpectedResult(IActionResult result)
            {
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                var user = Assert.IsType<TasklifyUser>(okObjectResult.Value);
                Assert.Equal(2, user.Id);
                Assert.Equal("testemail2@gmail.com", user.Email);
                Assert.Equal("test2", user.Name);
            }

            var uDAL = new DemoUsersDAL();
            var userController = new UsersController(uDAL);

            await userController.AddUser(new TasklifyUser(1, "testemail@gmail.com", "test"));
            await userController.AddUser(new TasklifyUser(2, "testemail2@gmail.com", "test2"));
            await userController.AddUser(new TasklifyUser(3, "testemail3@gmail.com", "test3"));
            
            var result = await userController.GetById(2);
            CheckExpectedResult(result);
            result = await userController.GetByEmail("testemail2@gmail.com");
            CheckExpectedResult(result);

            result = await userController.GetById(5);
            var objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);

            result = await userController.GetByEmail("nonexistentemail@gmail.com");
            objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);
        }

        [Fact]
        public async Task UserControllerUpdateUserTest()
        {
            var uDAL = new DemoUsersDAL();
            var userController = new UsersController(uDAL);

            var result = await userController.UpdateById(1, new TasklifyUser(1, "newemail@gmail.com", "test"));
            var objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);

            await userController.AddUser(new TasklifyUser(1, "testemail@gmail.com", "test"));
            result = await userController.UpdateById(1, new TasklifyUser(1, "newemail@gmail.com", "test"));

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<TasklifyUser>(okObjectResult.Value);
            Assert.Equal(1, user.Id);
            Assert.Equal("newemail@gmail.com", user.Email);
            Assert.Equal("test", user.Name);
        }

        [Fact]
        public async Task UserControllerDeleteUserTest()
        {
            var uDAL = new DemoUsersDAL();
            var userController = new UsersController(uDAL);

            var result = await userController.DeleteById(1);
            var objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);

            await userController.AddUser(new TasklifyUser(1, "testemail@gmail.com", "test"));
            result = await userController.DeleteById(1);
            Assert.IsType<NoContentResult>(result);

            result = await userController.GetAll();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var userList = Assert.IsType<List<TasklifyUser>>(okObjectResult.Value);
            Assert.Empty(userList);
        }
    }
}