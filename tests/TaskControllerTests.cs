using Xunit;
using Tasklify.DAL;
using Tasklify.Contracts;
using Tasklify.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;

namespace takehome.tests.csproj
{
    public class TaskControllerTests
    {
        [Fact]
        public async Task TaskControllerAddAndGetTest()
        {
            var uDAL = new DemoUsersDAL();
            var tBLL = new DemoTasksBLL(uDAL);
            var tasksController = new TasksController(tBLL);

            var result = await tasksController.GetAll();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var taskList = Assert.IsType<List<TasklifyTask>>(okObjectResult.Value);
            Assert.Empty(taskList);

            result = await tasksController.Add(new TasklifyTask(1, "summary", "description"));
            okObjectResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<TasklifyTask>(okObjectResult.Value);

            Assert.Equal(1, value.Id);
            Assert.Equal("summary", value.Summary);
            Assert.Equal("description", value.Description);

            result = await tasksController.Add(new TasklifyTask(1, "", "description"));
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            var errorString = Assert.IsType<String>(objectResult.Value);
            Assert.Equal("Task summary cannot be empty or all whitespace.", errorString);

            await tasksController.Add(new TasklifyTask(2, "summary2", "description2"));
            result = await tasksController.GetAll();
            okObjectResult = Assert.IsType<OkObjectResult>(result);
            taskList = Assert.IsType<List<TasklifyTask>>(okObjectResult.Value);

            Assert.Collection(taskList.OrderBy(user => user.Id), 
                task => {
                    Assert.Equal(1, task.Id);
                    Assert.Equal("summary", task.Summary);
                    Assert.Equal("description", task.Description);
                },
                task => {
                    Assert.Equal(2, task.Id);
                    Assert.Equal("summary2", task.Summary);
                    Assert.Equal("description2", task.Description);
                });
        }

        [Fact]
        public async Task TaskControllerUpdateTaskTest()
        {
            var uDAL = new DemoUsersDAL();
            var tBLL = new DemoTasksBLL(uDAL);
            var tasksController = new TasksController(tBLL);

            var result = await tasksController.UpdateById(1, new TasklifyTask(1, "summary", "description"));
            var objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);

            result = await tasksController.PatchById(1, new JsonPatchDocument<TasklifyTask>());
            objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);

            await tasksController.Add(new TasklifyTask(1, "summary", "description"));
            result = await tasksController.UpdateById(1, new TasklifyTask(1, "", "description"));
            var errorObject = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, errorObject.StatusCode);

            var patchDocument = new JsonPatchDocument<TasklifyTask>();
            patchDocument.Replace(task => task.Summary, "");
            result = await tasksController.PatchById(1, patchDocument);
            errorObject = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, errorObject.StatusCode);
            
            var errorString = Assert.IsType<String>(errorObject.Value);
            Assert.Equal("Task summary cannot be empty or all whitespace.", errorString);

            result = await tasksController.UpdateById(1, new TasklifyTask(1, "newsummary", "description"));
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var task = Assert.IsType<TasklifyTask>(okObjectResult.Value);
            Assert.Equal(1, task.Id);
            Assert.Equal("newsummary", task.Summary);
            Assert.Equal("description", task.Description);

            patchDocument = new JsonPatchDocument<TasklifyTask>();
            patchDocument.Replace(task => task.Summary, "updatedsummary");
            result = await tasksController.PatchById(1, patchDocument);
            okObjectResult = Assert.IsType<OkObjectResult>(result);
            task = Assert.IsType<TasklifyTask>(okObjectResult.Value);
            Assert.Equal(1, task.Id);
            Assert.Equal("updatedsummary", task.Summary);
            Assert.Equal("description", task.Description);
        }

        [Fact]
        public async Task TaskControllerDeleteUserTest()
        {
            var uDAL = new DemoUsersDAL();
            var tBLL = new DemoTasksBLL(uDAL);
            var tasksController = new TasksController(tBLL);

            var result = await tasksController.DeleteById(1);
            var objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, objectResult.StatusCode);

            await tasksController.Add(new TasklifyTask(1, "summary", "description"));
            result = await tasksController.DeleteById(1);
            Assert.IsType<NoContentResult>(result);

            result = await tasksController.GetAll();
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var taskList = Assert.IsType<List<TasklifyTask>>(okObjectResult.Value);
            Assert.Empty(taskList);
        }
    }
}