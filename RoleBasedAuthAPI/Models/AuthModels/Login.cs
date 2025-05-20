using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthAPI.Models.AuthModels
{
    public class Login
    {
        [Required, MinLength(1)]
        public string Username { get; set; }

        [Required, MinLength(1)]
        public string Password { get; set; }

    }
}
