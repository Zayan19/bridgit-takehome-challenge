using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using System;

using Tasklify.Interfaces;
using Tasklify.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

namespace Tasklify.DAL
{
    public class DemoTasksDAL : ITasksDAL
    {
        int _current_id = 0;
        IDictionary<int, TasklifyTask> _tasks = new ConcurrentDictionary<int, TasklifyTask>();

        public async Task<TasklifyTask> AddAsync(string summary, string description, int? assignee_id = null)
        {
            ValidateTask(summary, description, assignee_id);
            _current_id += 1;
            var tmpTask = new TasklifyTask(_current_id, summary, description, assignee_id);
            _tasks.Add(tmpTask.Id, tmpTask);

            return await Task.FromResult(tmpTask);
        }

        void ValidateTask(string summary, string description, int? assignee_id = null)
        {
            if (String.IsNullOrWhiteSpace(summary))
            {
                throw new ValidationException("Task summary cannot be empty or all whitespace.");
            }
            if (summary.Length > 100)
            {
                throw new ValidationException("Task summary should be less than or equal to 100 characters.");
            }
            if (description.Length > 500)
            {
                throw new ValidationException("Task description should be less than or equal to 500 characters.");
            }
        }

        public async Task<IList<TasklifyTask>> GetAllAsync()
        {
            return await Task.FromResult(_tasks.Values.ToList());
        }

        public async Task<TasklifyTask> GetByIdAsync(int id)
        {
            return await Task.Run(() => {
                if (!_tasks.ContainsKey(id))
                {
                    return null;
                }
                return _tasks[id];
            });
        }

        public async Task<TasklifyTask> UpdateByIdAsync(int id, TasklifyTask task)
        {
            return await Task.Run(() => {
                if (!_tasks.ContainsKey(id))
                {
                    return null;
                }
                ValidateTask(task.Summary, task.Description);
                _tasks[id].Summary = task.Summary;
                _tasks[id].Description = task.Description;
                _tasks[id].Assignee_Id = task.Assignee_Id;
                return _tasks[id];
            });
        }

        public async Task<TasklifyTask> PatchByIdAsync(int id, JsonPatchDocument<TasklifyTask> task)
        {
            return await Task.Run(() => {
                if (!_tasks.ContainsKey(id))
                {
                    return null;
                }
                var oldTask = _tasks[id];

                // Need to clone the task and apply the patched changes prior to validation, otherwise the original model might get into a corrupted state
                var clonedTask = oldTask.Clone();
                task.ApplyTo(clonedTask);
                ValidateTask(clonedTask.Summary, clonedTask.Description);

                _tasks[id] = clonedTask;
                return _tasks[id];
            });
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            return await Task.Run(() => {
                try
                {
                    return _tasks.Remove(id);
                }
                catch (System.Exception)
                {
                    return false;
                }
            });
        }
    }
}