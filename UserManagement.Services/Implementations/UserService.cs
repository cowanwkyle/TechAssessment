using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive)
        => await _dataAccess.Get<User>(user => user.IsActive == isActive).ToListAsync();

    public IEnumerable<User> FilterByActive(bool isActive)
    {
        var users =_dataAccess.Get<User>(user => user.IsActive == isActive);
        return users;
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    /// <summary>
    /// Asynchronous API to return all users
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync()
        => await _dataAccess.GetAll<User>().ToListAsync();
}
