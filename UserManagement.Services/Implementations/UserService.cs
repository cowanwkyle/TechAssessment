using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;

    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public async Task CreateAsync(User user)
    {
        await _dataAccess.CreateAsync(user);
    }

    public async Task DeleteAsync(long id)
    {
        await _dataAccess.DeleteAsync<User>(id);
    }

    public async Task<List<User>> FilterByActiveAsync(bool isActive, string sortField, bool isDesc)
        => await _dataAccess.GetAsync<User>(user => user.IsActive == isActive, sortField, isDesc);

    public async Task<List<User>> GetAllAsync(string sortField, bool isDesc)
        => await _dataAccess.GetAsync<User>(null, sortField, isDesc);

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _dataAccess.GetByIDAsync<User>(id);
    }

    public async Task UpdateAsync(User user)
    {
        await _dataAccess.UpdateAsync(user);
    }
}
