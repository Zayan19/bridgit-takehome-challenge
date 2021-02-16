using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Tasklify.Interfaces;
using Tasklify.Contracts;
using Microsoft.AspNetCore.JsonPatch;

namespace Tasklify.DAL
{
    public class DemoTasksBLL : ITasksBLL
    {
        DemoTasksDAL _tDal;
        DemoUsersDAL _uDal;

        public DemoTasksBLL(DemoUsersDAL usersDAL)
        {
            _tDal = new DemoTasksDAL();
            _uDal = usersDAL;
        }

        public async Task<TasklifyTask> AddAsync(string summary, string description, int? assignee_id)
        {
            ValidateAssigneeId(assignee_id);
            return await _tDal.AddAsync(summary, description, assignee_id);
        }

        public async Task<IList<TasklifyTask>> GetAllAsync()
        {
            return await _tDal.GetAllAsync();
        }

        public async Task<TasklifyTask> GetByIdAsync(int id)
        {
            return await _tDal.GetByIdAsync(id);
        }

        public async Task<TasklifyTask> UpdateByIdAsync(int id, TasklifyTask task)
        {
            ValidateAssigneeId(task.Assignee_Id);
            return await _tDal.UpdateByIdAsync(id, task);
        }

        public async Task<TasklifyTask> PatchByIdAsync(int id, JsonPatchDocument<TasklifyTask> task)
        {
            var items = task.Operations.Where(t => t.path.Equals("/assignee_id"));
            if (items.Count() > 0)
            {
                var o = items.Single().value;
                if (o.GetType() == typeof(int))
                {
                    ValidateAssigneeId((int)o);
                }
                else
                {
                    throw new ValidationException("Assignee_id must be an integer.");
                }
            }
            

            return await _tDal.PatchByIdAsync(id, task);
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
           return await _tDal.RemoveByIdAsync(id);
        }

        void ValidateAssigneeId(int? assignee_id)
        {
            if (assignee_id != null && !_uDal.UserExists(assignee_id.Value))
            {
                throw new ValidationException($"User with id {assignee_id.Value} does not exist.");
            }
        }
    }
}