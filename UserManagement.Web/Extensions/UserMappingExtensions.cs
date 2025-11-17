using UserManagement.Models;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Extensions;

public static class UserMappingExtensions
{
    public static User ToUser(this UserListItemViewModel userItem) =>
        new User
        {
            Forename = userItem.Forename ?? string.Empty,
            Surname = userItem.Surname ?? string.Empty,
            Email = userItem.Email ?? string.Empty,
            IsActive = userItem.IsActive,
            DateOfBirth = userItem.DateOfBirth ?? System.DateTime.Today
        };

    public static UserListItemViewModel ToUserItem(this User user) =>
        new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

    public static List<UserListItemViewModel> ToUserItems(this List<User> user) =>
        user.ConvertAll(u => u.ToUserItem());

    public static void UpdateFrom(this User user, UserListItemViewModel userItem)
    {
        user.Forename = userItem.Forename ?? string.Empty;
        user.Surname = userItem.Surname ?? string.Empty;
        user.Email = userItem.Email ?? string.Empty;
        user.IsActive = userItem.IsActive;
        user.DateOfBirth = userItem.DateOfBirth ?? System.DateTime.Today;
    }
}
