using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync(string sortField, bool isDesc);
    Task CreateAsync(User user);
    Task<User?> GetByIdAsync(long id);
    Task UpdateAsync(User user);
    Task DeleteAsync(long id);
    Task<List<User>> FilterByActiveAsync(bool isActive, string sortField, bool isDesc);
}
