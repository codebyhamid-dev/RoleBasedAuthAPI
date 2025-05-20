using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RoleBasedAuthAPI.Models;
using RoleBasedAuthAPI.Models.AuthModels;

namespace RoleBasedAuthAPI.Services
{
    public class AuthServices:IAuthServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthServices(UserManager<IdentityUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(Login model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
               new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var expireTime = int.Parse(_configuration["Jwt:ExpireTime"]);
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.UtcNow.AddMinutes(expireTime),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "Invalid credentials";
        }

        public async Task<string> RegisterAsync(Register model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return "User already exists!";
            IdentityUser user = new IdentityUser()
            {
                UserName = model.Username,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return "User creation failed! Please check user details and try again.";
            //Assign Role
            if (!await _userManager.IsInRoleAsync(user, model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            return "User created successfully!";
        }
    }
}
