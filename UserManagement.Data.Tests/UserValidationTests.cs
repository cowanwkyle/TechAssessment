using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data.Tests;
public class UserValidationTests
{
    private const string REQUIRED_ERROR = "required";
    private const string INVALID_CHARACTERS_ERROR = "can only contain letters";
    private const string INVALID_ERROR = "invalid";
    private const string FUTURE_DATE_ERROR = "cannot be in the future";
    private const string MAX_LENGTH_ERROR = "must be under";

    public void ShouldHaveMemberError(User user, string memberName, string errorMessage)
    {
        var results = Validate(user);
        var firstResultMessage = results.First().ErrorMessage ?? "";
        bool hasMemberErrorO = firstResultMessage.Contains(errorMessage, StringComparison.OrdinalIgnoreCase);
        bool hasMemberErrorI = firstResultMessage.Contains(errorMessage, StringComparison.InvariantCultureIgnoreCase);
        bool hasMemberErrorC = firstResultMessage.Contains(errorMessage, StringComparison.CurrentCultureIgnoreCase);

        results.Should().Contain(r =>
        r.MemberNames.Contains(memberName) &&
        r.ErrorMessage != null &&
        r.ErrorMessage.Contains(errorMessage,StringComparison.OrdinalIgnoreCase));
    }

    private List<ValidationResult> Validate(User user)
    {
        var context = new ValidationContext(user);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(user, context, results, true);
        return results;
    }

    private User CreateValidUser() =>
            new User
            {
                Forename = "Kyle",
                Surname = "Cowan",
                Email = "KyleCowan@gmail.com",
                DateOfBirth = DateTime.Parse("2025-01-01"),
                IsActive = true
            };

    [Fact]
    public void User_WithValidUser_ShouldPassValidation()
    {
        var user = CreateValidUser();

        var result = Validate(user);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Forename_WhenMissing_ShouldFail(string? name)
    {
        var user = CreateValidUser();
        user.Forename = name ?? default!;

        ShouldHaveMemberError( user, nameof(User.Forename), REQUIRED_ERROR);
    }

    [Theory]
    [InlineData("Kyle123")]
    [InlineData("Kyle!")]
    [InlineData("Ky1e")]
    public void Forename_WhenContainsInvalidCharacters_ShouldFail(string invalidName)
    {
        var user = CreateValidUser();
        user.Forename = invalidName;

        ShouldHaveMemberError(user, nameof(User.Forename), INVALID_CHARACTERS_ERROR);
    }

    [Fact]
    public void Forename_WhenTooLong_ShouldFail()
    {
        var user = CreateValidUser();
        user.Forename = new string('K', 51);

        ShouldHaveMemberError(user, nameof(User.Forename), MAX_LENGTH_ERROR);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Surname_WhenMissing_ShouldFail(string? name)
    {
        var user = CreateValidUser();
        user.Surname = name ?? default!;

        ShouldHaveMemberError(user, nameof(User.Surname), REQUIRED_ERROR);
    }

    [Theory]
    [InlineData("Cowan123")]
    [InlineData("Cowan!")]
    [InlineData("C0wan")]
    public void Surname_WhenContainsInvalidCharacters_ShouldFail(string invalidName)
    {
        var user = CreateValidUser();
        user.Surname = invalidName;

        ShouldHaveMemberError(user, nameof(User.Surname), INVALID_CHARACTERS_ERROR);
    }

    [Fact]
    public void Surname_WhenTooLong_ShouldFail()
    {
        var user = CreateValidUser();
        user.Surname = new string('C', 51);

        ShouldHaveMemberError(user, nameof(User.Surname), MAX_LENGTH_ERROR);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Email_WhenMissing_ShouldFail(string? email)
    {
        var user = CreateValidUser();
        user.Email = email ?? default!;

        ShouldHaveMemberError(user, nameof(User.Email), REQUIRED_ERROR);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("kyle@")]
    [InlineData("@example.com")]
    [InlineData("kyle@@example.com")]
    public void Email_WhenInvalid_ShouldFail(string email)
    {
        var user = CreateValidUser();
        user.Email = email;

        ShouldHaveMemberError(user, nameof(User.Email), INVALID_ERROR);
    }

    [Fact]
    public void DateOfBirth_WhenMissing_ShouldFail()
    {
        var user = CreateValidUser();
        user.DateOfBirth = default;

        ShouldHaveMemberError(user, nameof(User.DateOfBirth), REQUIRED_ERROR);
    }

    [Fact]
    public void DateOfBirth_WhenInFuture_ShouldFail()
    {
        var user = CreateValidUser();
        user.DateOfBirth = DateTime.Today.AddDays(1); ;

        ShouldHaveMemberError(user, nameof(User.DateOfBirth), FUTURE_DATE_ERROR);
    }
}
