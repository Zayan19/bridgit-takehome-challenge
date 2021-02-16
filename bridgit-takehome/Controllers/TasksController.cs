using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Tasklify.Interfaces;
using Tasklify.Contracts;

namespace Tasklify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUsersDAL _uDal;
        private readonly ITasksDAL _tDal;
        
        public TasksController(IUsersDAL userDal, ITasksDAL taskDal)
        {
            _uDal = userDal;
            _tDal = taskDal;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tDal.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _tDal.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TasklifyTask task)
        {
            var tmpTask = await _tDal.AddAsync(task.Summary, task.Description);
            return Ok(tmpTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] TasklifyTask task)
        {
            if (string.IsNullOrEmpty(task.Summary))
            {
                return BadRequest("Summary cannot be empty.");
            }

            return Ok(await _tDal.UpdateByIdAsync(id, task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _tDal.RemoveByIdAsync(id);
            return NoContent();
        }
    }
}
