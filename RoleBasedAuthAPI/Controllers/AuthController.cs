using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthAPI.Models;
using RoleBasedAuthAPI.Models.AuthModels;
using RoleBasedAuthAPI.Services;

namespace RoleBasedAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] Register model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServices.RegisterAsync(model);
            if (result == "User created successfully!")
                return Ok(new { Message = result });
            return BadRequest(new { Message = result });
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authServices.LoginAsync(model);
            if (token == "Invalid credentials")
                return Unauthorized(new { Message = token });
            return Ok(new { Token = token });
        }
    }
}
