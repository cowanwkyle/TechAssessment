using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Data.Extensions;

namespace UserManagement.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required(ErrorMessage = "Forename is required.")]
    [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Forename can only contain letters, spaces, apostrophes or hyphens.")]
    [StringLength(50, ErrorMessage = "Forename must be under 50 characters.")]
    public string Forename { get; set; } = default!;
    [Required(ErrorMessage = "Surname is required.")]
    [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Surname can only contain letters, spaces, apostrophes or hyphens.")]
    [StringLength(50, ErrorMessage = "Surname must be under 50 characters.")]
    public string Surname { get; set; } = default!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    [CustomValidation(typeof(UserValidators), nameof(UserValidators.ValidateDateOfBirth))]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}
