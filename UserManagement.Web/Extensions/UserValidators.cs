//using System;
//using System.ComponentModel.DataAnnotations;

//namespace UserManagement.Web.Extensions;

//public static class UserValidators
//{
//    public static ValidationResult? ValidateDateOfBirth(DateTime? date, ValidationContext context)
//    {
//        if (!date.HasValue)
//        {
//            return ValidationResult.Success;
//        }

//        if (date.Value > DateTime.Today)
//        {
//            return new ValidationResult("Date of birth cannot be in the future.");
//        }

//        return ValidationResult.Success;
//    }
//}
