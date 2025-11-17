using System;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Extensions;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;
    private const int PageSize = 10;

    [HttpGet]
    public async Task<IActionResult> List(bool? isActive, UserListSortField? sortField, int? pageNumber, bool sortOrder = false)
    {
        var model = new UserListViewModel
        {
            IsActive = isActive,
            SortOrder = sortOrder,
            SortField = sortField ?? UserListSortField.Id
        };

        string sortfieldName = model.SortField.ToString()?? UserListSortField.Id.ToString();
        List<User> users = !isActive.HasValue
            ? await _userService.GetAllAsync(sortfieldName, sortOrder)
            : await _userService.FilterByActiveAsync(isActive.Value, sortfieldName, sortOrder);
        
        model.PagedItems = await PagedList<UserListItemViewModel>.CreateAsync(users.ToUserItems(), pageNumber ?? 1, PageSize);
        return View(model);
    }

    [HttpGet("create")]
    public IActionResult Create() =>
        View(new UserListItemViewModel { IsActive = true, DateOfBirth = DateTime.Now });

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = model.ToUser();

        await _userService.CreateAsync(user);
        return RedirectToAction(nameof(List),model.IsActive);
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        var userItem = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(userItem);
    }

    [HttpGet("edit")]
    public async Task<IActionResult> Edit(long id, string? returnUrl = null)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        var userItem = user.ToUserItem();
        ViewData["ReturnUrl"] = returnUrl;
        return View(userItem);
    }

    [HttpPost("edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, UserListItemViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        user.UpdateFrom(model);
        await _userService.UpdateAsync(user);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(List));
    }

    [HttpPost("delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction("List");
    }
}
