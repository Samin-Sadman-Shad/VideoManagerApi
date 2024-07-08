using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using VideoManagerApi.Models;
using VideoManagerApi.Repositories;

namespace VideoManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController:ControllerBase
    {
        ILogger<RegisterController> _logger;
        IUserRepository _userRepository;

        public RegisterController(ILogger<RegisterController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]User user)
        {
            if(!ModelState.IsValid) 
            { 
                return BadRequest();
            }
            var registered = await _userRepository.CreateUser(user);
            if(registered is null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(Register), new { id = user.Id},  registered);
        }
    }
}
