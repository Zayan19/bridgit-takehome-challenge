using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

using Tasklify.Interfaces;
using Tasklify.Contracts;

namespace Tasklify.DAL
{
    public class DemoUsersDAL : IUsersDAL
    {
        int _current_id = 0;
        IDictionary<int, TasklifyUser> _users = new ConcurrentDictionary<int, TasklifyUser>();

        public async Task<TasklifyUser> AddUserAsync(string email, string name)
        {
            _current_id += 1;
            var tmpUser = new TasklifyUser(_current_id, email, name);
            _users.Add(tmpUser.Id, tmpUser);

            return await Task.FromResult(tmpUser);
        }

        public async Task<IList<TasklifyUser>> GetUsersAsync()
        {
            return await Task.FromResult(_users.Values.ToList());
        }

        public async Task<TasklifyUser> GetUserByIdAsync(int id)
        {
            return await Task.Run(() => {
                return _users[id];
            });
        }

        public async Task<bool> UpdateUserByIdAsync(int id, TasklifyUser user)
        {
            return await Task.Run(() => {
                _users[id].Name = user.Name;
                _users[id].Email = user.Email;
                return true;
            });
        }

        public async Task<bool> RemoveUserByIdAsync(int id)
        {
            return await Task.Run(() => {
                try
                {
                    _users.Remove(id);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            });
        }
    }
}