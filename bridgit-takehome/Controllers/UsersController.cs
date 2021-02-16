using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

using Tasklify.Interfaces;
using Tasklify.Contracts;

namespace Tasklify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersDAL _dal;
        
        public UsersController(IUsersDAL userDal)
        {
            _dal = userDal;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dal.GetUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _dal.GetUserByIdAsync(id);
            if (user is null)
            {
                return StatusCode(403);
            }
            return Ok(user);
        }

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
         
            var tmpUser = await _dal.GetUserByEmailAsync(email);
            if (tmpUser is null)
            {
                return StatusCode(403);
            }
            return Ok(tmpUser);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] TasklifyUser user)
        {
            try 
            {
                var tmpUser = await _dal.AddUserAsync(user.Email, user.Name);
                return Ok(tmpUser);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] TasklifyUser user)
        {
            try 
            {
                var tmpUser = await _dal.UpdateUserByIdAsync(id, user);
                if (tmpUser is null)
                {
                    return StatusCode(403);
                }
                return Ok(tmpUser);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _dal.RemoveUserByIdAsync(id);
            if (!result)
            {
                return StatusCode(403);
            }
            return NoContent();
        }
    }
}
