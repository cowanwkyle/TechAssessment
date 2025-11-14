using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Data.Extensions;
public static class UserValidators
{
    public static ValidationResult? ValidateDateOfBirth(DateTime? date, ValidationContext context)
    {
        if (!date.HasValue)
        {
            return ValidationResult.Success;
        }
        if (date.Value == default)
        {
            return new ValidationResult("Date of birth is required.", [context.MemberName ?? string.Empty]);
        }

        if (date.Value > DateTime.Today)
        {
            return new ValidationResult("Date of birth cannot be in the future.", [context.MemberName ?? string.Empty]);
        }

        return ValidationResult.Success;
    }
}

