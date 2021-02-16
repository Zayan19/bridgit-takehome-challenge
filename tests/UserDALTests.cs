using Xunit;
using Tasklify.DAL;
using Tasklify.Contracts;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace takehome.tests.csproj
{
    public class UserDALTests
    {
        [Fact]
        public async Task UserDALSameEmailTestAsync()
        {
            var userDAL = new DemoUsersDAL();
            var userList = await userDAL.GetUsersAsync();
            Assert.Empty(userList);

            var email = "test@gmail.com";

            await userDAL.AddUserAsync(email, "test");
            await userDAL.AddUserAsync("anotheremail@gmail.com", "test2");

            userList = await userDAL.GetUsersAsync();

            Assert.Collection(userList.OrderBy(user => user.Id), 
                user => {
                    Assert.Equal(1, user.Id);
                    Assert.Equal(email, user.Email);
                    Assert.Equal("test", user.Name);
                },
                user => {
                    Assert.Equal(2, user.Id);
                    Assert.Equal("anotheremail@gmail.com", user.Email);
                    Assert.Equal("test2", user.Name);
                });

            // Attempt to add a user with the same email as an existing one, exception should be thrown
            var exception = await Assert.ThrowsAsync<ValidationException>(() => userDAL.AddUserAsync(email, "test3"));
            Assert.Equal("Email already exists for a different user.", exception.Message);

            await userDAL.RemoveUserByIdAsync(1);
            await userDAL.AddUserAsync(email, "test3");
            userList = await userDAL.GetUsersAsync();

            Assert.Collection(userList.OrderBy(user => user.Id), 
                user => {
                    Assert.Equal(2, user.Id);
                    Assert.Equal("anotheremail@gmail.com", user.Email);
                    Assert.Equal("test2", user.Name);
                },
                user => {
                    Assert.Equal(3, user.Id);
                    Assert.Equal(email, user.Email);
                    Assert.Equal("test3", user.Name);
                });
        }

        [Fact]
        public async Task UserDALValidationTestAsync()
        {
            async Task TestValidation(Func<string, string, Task<TasklifyUser>> testMethod)
            {
                var exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("", "test"));
                Assert.Equal("Email cannot be empty or all whitespace.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("  ", "test"));
                Assert.Equal("Email cannot be empty or all whitespace.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod(new String('a', 101), "test"));
                Assert.Equal("User email should be less than or equal to 100 characters", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("vaildemail@gmail.com", ""));
                Assert.Equal("Name cannot be empty or all whitespace.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("vaildemail@gmail.com", "     "));
                Assert.Equal("Name cannot be empty or all whitespace.", exception.Message);

                exception = await Assert.ThrowsAsync<ValidationException>(() => testMethod("vaildemail@gmail.com", new String('a', 151)));
                Assert.Equal("User name should be less than or equal to 150 characters", exception.Message);
            }

            var uDAL = new DemoUsersDAL();
            await TestValidation((string email, string name) => uDAL.AddUserAsync(email, name));

            await uDAL.AddUserAsync("validemail@gmail.com", "test");
            await TestValidation((string email, string name) => uDAL.UpdateUserByIdAsync(1, new TasklifyUser(1, email, name)));
 
            await uDAL.UpdateUserByIdAsync(1, new TasklifyUser(1, "newemail@gmail.com", "newname"));
            var userList = await uDAL.GetUsersAsync();

            Assert.Collection(userList, 
                user => {
                    Assert.Equal(1, user.Id);
                    Assert.Equal("newemail@gmail.com", user.Email);
                    Assert.Equal("newname", user.Name);
                });
        }

        [Fact]
        public async Task UserDALGetByEmailTestAsync()
        {
            async Task TestSearchByEmail(DemoUsersDAL userDAL, string email, int expectedId, string expectedName)
            {
                var user = await userDAL.GetUserByEmailAsync(email);
                Assert.Equal(expectedId, user.Id);
                Assert.Equal(email, user.Email);
                Assert.Equal(expectedName, user.Name);
            }           

            var uDAL = new DemoUsersDAL();
            await uDAL.AddUserAsync("email1@gmail.com", "test1");
            await uDAL.AddUserAsync("email2@gmail.com", "test2"); 
            await uDAL.AddUserAsync("email3@gmail.com", "test3");

            await TestSearchByEmail(uDAL, "email2@gmail.com", 2, "test2");
            await TestSearchByEmail(uDAL, "email1@gmail.com", 1, "test1");
            await TestSearchByEmail(uDAL, "email3@gmail.com", 3, "test3");
        }
    }
}