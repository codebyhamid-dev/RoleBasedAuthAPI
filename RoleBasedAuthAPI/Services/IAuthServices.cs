using RoleBasedAuthAPI.Models;
using RoleBasedAuthAPI.Models.AuthModels;

namespace RoleBasedAuthAPI.Services
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(Register model);
        Task<string> LoginAsync(Login model);
    }
}
