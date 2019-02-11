using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

using Tasklify.Interfaces;
using Tasklify.Contracts;

namespace Tasklify.DAL
{
    public class DemoTasksDAL : ITasksDAL
    {
        int _current_id = 0;
        IDictionary<int, TasklifyTask> _tasks = new ConcurrentDictionary<int, TasklifyTask>();

        public async Task<TasklifyTask> AddAsync(string summary, string description)
        {
            _current_id += 1;
            var tmpTask = new TasklifyTask(_current_id, summary, description);
            _tasks.Add(tmpTask.Id, tmpTask);

            return await Task.FromResult(tmpTask);
        }

        public async Task<IList<TasklifyTask>> GetAllAsync()
        {
            return await Task.FromResult(_tasks.Values.ToList());
        }

        public async Task<TasklifyTask> GetByIdAsync(int id)
        {
            return await Task.Run(() => {
                return _tasks[id];
            });
        }

        public async Task<TasklifyTask> UpdateByIdAsync(int id, TasklifyTask task)
        {
            return await Task.Run(() => {
                _tasks[id].Summary = task.Summary;
                _tasks[id].Description = task.Description;
                return _tasks[id];
            });
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            return await Task.Run(() => {
                try
                {
                    _tasks.Remove(id);
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