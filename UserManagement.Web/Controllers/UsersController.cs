using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<IActionResult> List(bool? isActive)
    {
        IEnumerable<User> users;
        if (!isActive.HasValue)
        {
            users = await _userService.GetAllAsync();
        }
        else
        {
            users = await _userService.FilterByActiveAsync(isActive.Value);
        }

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }
    //[HttpGet]
    //public async Task<IActionResult> List()
    //{
    //    var users = await _userService.GetAllAsync();
    //    var items = users.Select(p => new UserListItemViewModel
    //    {
    //        Id = p.Id,
    //        Forename = p.Forename,
    //        Surname = p.Surname,
    //        Email = p.Email,
    //        IsActive = p.IsActive
    //    });

    //    var model = new UserListViewModel
    //    {
    //        Items = items.ToList()
    //    };

    //    return View(model);
    //}
    //[HttpGet("/[action]/{isActive?}")]
    //public IActionResult ActiveFilteredList(bool isActive = false)
    //{
    //    var items = _userService.FilterByActive(isActive).Select(p => new UserListItemViewModel
    //    {
    //        Id = p.Id,
    //        Forename = p.Forename,
    //        Surname = p.Surname,
    //        Email = p.Email,
    //        IsActive = p.IsActive
    //    });

    //    var model = new UserListViewModel
    //    {
    //        Items = items.ToList()
    //    };

    //    return View("List",model);
    //}
}
