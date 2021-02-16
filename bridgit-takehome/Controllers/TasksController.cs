using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

using Tasklify.Interfaces;
using Tasklify.Contracts;

namespace Tasklify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksBLL _tBLL;
        
        public TasksController(ITasksBLL taskBLL)
        {
            _tBLL = taskBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tBLL.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _tBLL.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TasklifyTask task)
        {
            try
            {
                var tmpTask = await _tBLL.AddAsync(task.Summary, task.Description, task.Assignee_Id);
                return Ok(tmpTask);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] TasklifyTask task)
        {
            try 
            {
                var tmpTask = await _tBLL.UpdateByIdAsync(id, task);
                if (tmpTask is null)
                {
                    return StatusCode(403);
                }
                return base.Ok((object)tmpTask);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchById(int id, [FromBody] JsonPatchDocument<TasklifyTask> task)
        {
            try 
            {
                var tmpTask = await _tBLL.PatchByIdAsync(id, task);
                if (tmpTask is null)
                {
                    return StatusCode(403);
                }
                return Ok(tmpTask);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _tBLL.RemoveByIdAsync(id);
            if (!result)
            {
                return StatusCode(403);
            }
            return NoContent();
        }
    }
}
