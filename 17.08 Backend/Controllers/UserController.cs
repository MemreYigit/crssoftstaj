using CrsSoft.Data;
using CrsSoft.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CrsSoft.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {   
        private readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var _users = await _dataContext.Users.ToListAsync();

            return Ok(_users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var _user = await _dataContext.Users.FindAsync(id);
            if (_user == null)
            {
                return NotFound("User not found.");     //BadRequest
            }
            return Ok(_user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var _user = await _dataContext.Users.FindAsync(id);
            if (_user == null) { return NotFound("User Not Found."); }

            _dataContext.Remove(_user);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Users.ToListAsync());
        }
    }
}
