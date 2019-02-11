using System.Threading.Tasks;
using System.Collections.Generic;

using Tasklify.Contracts;

namespace Tasklify.Interfaces
{
    public interface IUsersDAL
    {
        Task<IList<TasklifyUser>> GetUsersAsync();
        Task<TasklifyUser> AddUserAsync(string email, string name);
        Task<bool> RemoveUserByIdAsync(int id);
        Task<TasklifyUser> GetUserByIdAsync(int id);
        Task<bool> UpdateUserByIdAsync(int id, TasklifyUser user);
    }
}