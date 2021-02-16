using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;

using Tasklify.Interfaces;
using Tasklify.Contracts;

namespace Tasklify.DAL
{
    public class DemoUsersDAL : IUsersDAL
    {
        int _current_id = 0;
        IDictionary<int, TasklifyUser> _users = new ConcurrentDictionary<int, TasklifyUser>();
        IDictionary<string, int> _userEmails = new ConcurrentDictionary<string, int>();

        public async Task<TasklifyUser> AddUserAsync(string email, string name)
        {
            ValidateUser(email, name);

            _current_id += 1;
            var tmpUser = new TasklifyUser(_current_id, email, name);
            _users.Add(tmpUser.Id, tmpUser);
            _userEmails[email] = _current_id;

            return await Task.FromResult(tmpUser);
        }

        void ValidateUser(string email, string name)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ValidationException("Email cannot be empty or all whitespace.");
            }
            if (email.Length > 100)
            {
                throw new ValidationException("User email should be less than or equal to 100 characters");
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Name cannot be empty or all whitespace.");
            }
            if (name.Length > 150)
            {
                throw new ValidationException("User name should be less than or equal to 150 characters");
            }
            if (_userEmails.ContainsKey(email))
            {
                throw new ValidationException("Email already exists for a different user.");
            }
        }
       
        public async Task<IList<TasklifyUser>> GetUsersAsync()
        {
            return await Task.FromResult(_users.Values.ToList());
        }

        public async Task<TasklifyUser> GetUserByIdAsync(int id)
        {
            return await Task.Run(() => {
                if (!_users.ContainsKey(id))
                {
                    return null;
                }
                return _users[id];
            });
        }

        public async Task<TasklifyUser> GetUserByEmailAsync(string email)
        {
            return await Task.Run(() => {
                if (!_userEmails.ContainsKey(email))
                {
                    return null;
                }
                return _users[_userEmails[email]];
            });
        }

        public async Task<TasklifyUser> UpdateUserByIdAsync(int id, TasklifyUser user)
        {
            return await Task.Run(() => {
                if (!_users.ContainsKey(id))
                {
                    return null;
                }
                ValidateUser(user.Email, user.Name);
                _users[id].Name = user.Name;
                _users[id].Email = user.Email;
                return _users[id];
            });
        }

        public async Task<bool> RemoveUserByIdAsync(int id)
        {
            return await Task.Run(() => {
                try
                {
                    _userEmails.Remove(_users[id].Email);
                    _users.Remove(id);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            });
        }

        public bool UserExists(int userId) => _users.ContainsKey(userId);
    }
}