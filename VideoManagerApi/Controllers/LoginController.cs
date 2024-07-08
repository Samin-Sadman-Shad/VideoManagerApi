using Microsoft.AspNetCore.Mvc;
using VideoManagerApi.Repositories;
using VideoManagerApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VideoManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController:ControllerBase
    {
        ILogger<LoginController> _logger;
        IUserRepository _userRepository;
        IConfiguration _config;

        public LoginController(ILogger<LoginController> logger, IUserRepository userRepository, IConfiguration config)
        {
            _logger = logger;
            _userRepository = userRepository;
            _config = config;

        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            IActionResult response = Unauthorized();
            var authenticated = await AuthenticateUser(user);
            if(authenticated != null)
            {
                var jwtoken = CreateJwt(user);
                response = Ok(new {access_token = jwtoken});
            }
            return response;

        }

        public async Task<User> AuthenticateUser(User loginData)
        {
            var user = await _userRepository.GetUser(loginData.Id);
            return user;
        }

        public string CreateJwt(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256
                );
            var token = new JwtSecurityToken(signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
