using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Extensions;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UsersController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = DefaultUsers();
        _mockUserService.Setup(s => s.GetAllAsync("Id", false)).ReturnsAsync(users);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _controller.List(null,UserListSortField.Id, 1, false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<ViewResult>()
          .Which.Model.Should().BeOfType<UserListViewModel>()
          .Which.PagedItems.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenServiceReturnsActiveUsers_ModelMustContainActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = new List<User>() { CreateUser(1, true) };
        _mockUserService.Setup(s => s.FilterByActiveAsync(true, "Id", false)).ReturnsAsync(users).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _controller.List(true, UserListSortField.Id, 1, false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<ViewResult>()
          .Which.Model.Should().BeOfType<UserListViewModel>()
          .Which.PagedItems.Should().BeEquivalentTo(users);
        _mockUserService.Verify();
    }


    [Fact]
    public async Task Create_WhenServiceCreateNewUser_ReturnsARedirectToList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var user = CreateUser(8);
        _mockUserService.Setup(s => s.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _controller.Create(user.ToUserItem());
        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().BeEquivalentTo(nameof(UsersController.List));
        _mockUserService.Verify();
            }

    [Fact]
    public async Task CreatePost_WhenModelInvalid_ReturnsSameView()
    {
        var itemViewModel = new UserListItemViewModel();
        _controller.ModelState.AddModelError("Forename", "Required");

        var result = await _controller.Create(itemViewModel);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeEquivalentTo(itemViewModel);
    }

    [Fact]
    public async Task Details_WhenUserExists_ReturnsView()
    {
        long userId = 1;
        var user = CreateUser(userId);
        _mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _controller.Details(userId);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserListItemViewModel>();
    }

    [Fact]
    public async Task Details_WhenUserNotFound_ReturnsNotFound()
    {
        long userId = 1;
        _mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var result = await _controller.Details(userId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task EditGet_WhenUserExists_ReturnsView()
    {
        var user = CreateUser(1);
        long userId = 1;
        _mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _controller.Edit(userId);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserListItemViewModel>();
    }

    [Fact]
    public async Task EditGet_WhenUserNotFound_ReturnsNotFound()
    {
        long userId = 1;
        _mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var result = await _controller.Edit(userId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_WhenCalled_DeletesUserAndRedirects()
    {
        long userId = 1;
        _mockUserService.Setup(s => s.DeleteAsync(userId)).Verifiable();

        var result = await _controller.Delete(userId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().BeEquivalentTo(nameof(UsersController.List));

        _mockUserService.Verify();
    }

    private User CreateUser(long id = 5, bool isActive = true, string forename = "Johnny", string surname = "User", string email = "juser@example.com", string dateOfBirth = "2025-2-15") => new()
    {
        Id = id,
        Forename = forename,
        Surname = surname,
        Email = email,
        IsActive = isActive,
        DateOfBirth = DateTime.Parse(dateOfBirth)
    };

    private List<User> DefaultUsers() => new()
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-1-11") },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-5-15") },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-6-11") },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = false, DateOfBirth = DateTime.Parse("2025-10-1") }
        };
}
