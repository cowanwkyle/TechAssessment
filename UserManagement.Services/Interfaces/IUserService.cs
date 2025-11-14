using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<List<User>> FilterByActiveAsync(bool isActive);
    Task<List<User>> GetAllAsync();
    Task CreateAsync(User user);
    Task<User?> GetByIdAsync(long id);
    Task UpdateAsync(User user);
    Task DeleteAsync(long id);
}
