using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthAPI.Dtos
{
    public class EmployeeDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
