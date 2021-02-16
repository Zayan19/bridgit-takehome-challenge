using System.Threading.Tasks;
using System.Collections.Generic;

using Tasklify.Contracts;

namespace Tasklify.Interfaces
{
    public interface ITasksDAL
    {
        Task<IList<TasklifyTask>> GetAllAsync();
        Task<TasklifyTask> AddAsync(string summary, string description);
        Task<bool> RemoveByIdAsync(int id);
        Task<TasklifyTask> GetByIdAsync(int id);
        Task<TasklifyTask> UpdateByIdAsync(int id, TasklifyTask user);
    }
}