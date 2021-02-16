using Xunit;
using Tasklify.DAL;
using Tasklify.Contracts;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch;

namespace takehome.tests.csproj
{
    public class TasksDALTests
    {
        [Fact]
        public async Task AddGetDeleteTaskTestAsync()
        {
            void TestParameters(TasklifyTask task, int id, string summary, string description, int? assignee_id)
            {
                Assert.Equal(id, task.Id);
                Assert.Equal(summary, task.Summary);
                Assert.Equal(description, task.Description);
                Assert.Equal(assignee_id, task.Assignee_Id);
            }

            var tDAL = new DemoTasksDAL();

            var result = await tDAL.AddAsync("Summary1", "Description1", null);
            TestParameters(result, 1, "Summary1", "Description1", null);

            result = await tDAL.AddAsync("Summary2", "Description2", null);
            TestParameters(result, 2, "Summary2", "Description2", null);

            result = await tDAL.AddAsync("Summary3", "Description3", null);
            TestParameters(result, 3, "Summary3", "Description3", null);

            // Try to remove a task with an id that does not exist, should return false
            var removeResult = await tDAL.RemoveByIdAsync(5);
            Assert.False(removeResult);

            // A task with ID 1 should exist and thus the operation should go through
            removeResult = await tDAL.RemoveByIdAsync(1);
            Assert.True(removeResult);

            var taskList = await tDAL.GetAllAsync();
            Assert.Collection(taskList.OrderBy(task => task.Id), 
                task => {
                    Assert.Equal(2, task.Id);
                    Assert.Equal("Summary2", task.Summary);
                    Assert.Equal("Description2", task.Description);
                },
                task => {
                    Assert.Equal(3, task.Id);
                    Assert.Equal("Summary3", task.Summary);
                    Assert.Equal("Description3", task.Description);
                });

            result = await tDAL.GetByIdAsync(5);
            Assert.Null(result);

            result = await tDAL.GetByIdAsync(2);
            TestParameters(result, 2, "Summary2", "Description2", null);
        }

        [Fact]
        public async Task TaskDALValidationTestAsync()
        {
            async Task TestValidation(Func<string, string, Task<TasklifyTask>> testMethod)
            {
                var exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("", "test"));
                Assert.Equal("Task summary cannot be empty or all whitespace.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("  ", "test"));
                Assert.Equal("Task summary cannot be empty or all whitespace.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod(new String('a', 101), "test"));
                Assert.Equal("Task summary should be less than or equal to 100 characters.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("Here is a valid summary.", new String('a', 501)));
                Assert.Equal("Task description should be less than or equal to 500 characters.", exception.Message);
            }

            var tDAL = new DemoTasksDAL();
            await TestValidation((string summary, string description) => tDAL.AddAsync(summary, description, null));

            // valid parameters added this time so the add operation will go through but the next set operation should fail due to invalid parameters
            await tDAL.AddAsync("This is the old summary", "This is the old description");
            await TestValidation((string summary, string description) => tDAL.UpdateByIdAsync(1, new TasklifyTask(1, summary, description)));

            // This time both the summary and description are valid so the set operation should go through
            await tDAL.UpdateByIdAsync(1, new TasklifyTask(1, "This is a valid new summary", "This is a valid new description"));
            var taskList = await tDAL.GetAllAsync();

            Assert.Collection(taskList, 
                task => {
                    Assert.Equal(1, task.Id);
                    Assert.Equal("This is a valid new summary", task.Summary);
                    Assert.Equal("This is a valid new description", task.Description);
                });
        }

        [Fact]
        public async Task TaskDALPatchTestAsync()
        {
            var tDAL = new DemoTasksDAL();
            
            var patchDocument = new JsonPatchDocument<TasklifyTask>();
            patchDocument.Replace(task => task.Description, "newdescription");
            var result = await tDAL.PatchByIdAsync(1, patchDocument);
            Assert.Null(result);

            await tDAL.AddAsync("summary", "description");
            await tDAL.PatchByIdAsync(1, patchDocument);

            var task = await tDAL.GetByIdAsync(1);
            Assert.Equal(1, task.Id);
            Assert.Equal("summary", task.Summary);
            Assert.Equal("newdescription", task.Description);
        }
    }
}