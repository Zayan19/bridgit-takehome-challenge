using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

using Tasklify.Contracts;

namespace Tasklify.Interfaces
{
    public interface ITasksBLL
    {
        Task<IList<TasklifyTask>> GetAllAsync();
        Task<TasklifyTask> AddAsync(string summary, string description, int? assignee_id);
        Task<bool> RemoveByIdAsync(int id);
        Task<TasklifyTask> GetByIdAsync(int id);
        Task<TasklifyTask> UpdateByIdAsync(int id, TasklifyTask user);
        Task<TasklifyTask> PatchByIdAsync(int id, JsonPatchDocument<TasklifyTask> task);
    }
}