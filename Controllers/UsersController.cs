using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(await _dal.GetUserByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] TasklifyUser user)
        {
            var tmpUser = await _dal.AddUserAsync(user.Email, user.Name);
            return Ok(tmpUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] TasklifyUser user)
        {
            await _dal.UpdateUserByIdAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _dal.RemoveUserByIdAsync(id);
            return NoContent();
        }
    }
}
