using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UserManagement.Data.Extensions;
using UserManagement.Web.Extensions;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public bool? IsActive { get; set; }
    public UserListSortField SortField { get; set; } = UserListSortField.Id;
    public bool SortOrder { get; set; } = false;

    public PagedList<UserListItemViewModel> PagedItems { get; set; } = new();

}

public class UserListItemViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Forename is required.")]
    [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Forename can only contain letters, spaces, apostrophes or hyphens.")]
    [StringLength(50, ErrorMessage = "Forename must be under 50 characters.")]
    public string? Forename { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Surname can only contain letters, spaces, apostrophes or hyphens.")]
    [StringLength(50, ErrorMessage = "Surname must be under 50 characters.")]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
    [DisplayName("Account Active")]
    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date), DisplayName("Date of Birth")]
    [CustomValidation(typeof(UserValidators), nameof(UserValidators.ValidateDateOfBirth))]
    public DateTime? DateOfBirth { get; set; }
}

public enum UserListSortField
{
    Id,
    Forename,
    Surname,
    Email,
    DateOfBirth,
    IsActive
}

