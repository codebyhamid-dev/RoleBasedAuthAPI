using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthAPI.Models
{
        public class Register
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public string Role { get; set; }
        }
    }

