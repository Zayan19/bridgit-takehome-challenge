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
using System.ComponentModel.DataAnnotations;

namespace takehome.tests.csproj
{
    public class TaskBLLTests
    {
        [Fact]
        public async Task TaskBLLValidAssigneeIdTest()
        {
            async Task TestValidation(Func<Task<TasklifyTask>> testMethod)
            {
                var exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod());
                Assert.Equal("User with id 1 does not exist.", exception.Message);
            }
            var uDAL = new DemoUsersDAL();
            var tBLL = new DemoTasksBLL(uDAL);

            await TestValidation(() => tBLL.AddAsync("summary", "description", 1));
            await TestValidation(() => tBLL.UpdateByIdAsync(1, new TasklifyTask(1, "newsummary", "description", 1)));

            var patchDocument = new JsonPatchDocument<TasklifyTask>();
            patchDocument.Replace(task => task.Assignee_Id, 1);
            await TestValidation(() => tBLL.PatchByIdAsync(1, patchDocument));

            // Add a user with id 1, now the above operations should go through
            await uDAL.AddUserAsync("validemail@gmail.com", "test");
            await tBLL.AddAsync("summary", "description", 1);
            await tBLL.UpdateByIdAsync(1, new TasklifyTask(1, "newsummary", "description", 1));

            // Add another user so a user with id 2 exists
            await uDAL.AddUserAsync("validemail2@gmail.com", "test2");

            // Now this id can be assigned to the assignee_id for task 1
            patchDocument = new JsonPatchDocument<TasklifyTask>(); 
            patchDocument.Replace(task => task.Assignee_Id, 2);
            var task = await tBLL.PatchByIdAsync(1, patchDocument);
            Assert.Equal(1, task.Id);
            Assert.Equal("newsummary", task.Summary);
            Assert.Equal("description", task.Description);
            Assert.Equal(2, task.Assignee_Id);
        }
    }
}