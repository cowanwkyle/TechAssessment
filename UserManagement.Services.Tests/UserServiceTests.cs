using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private readonly Mock<IDataContext> _mockDataContext;
    private readonly IUserService _service;
    public UserServiceTests()
    {
        _mockDataContext = new Mock<IDataContext>();
        _service = new UserService(_mockDataContext.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = DefaultUsers();
        _mockDataContext.Setup(s => s.GetAsync<User>(null,"Id", false)).ReturnsAsync(users);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _service.GetAllAsync("Id", false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
        _mockDataContext.Verify();
    }

    [Fact]
    public async Task FilterByActiveAsync_WhenActive_ReturnsOnlyActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = new List<User>() { CreateUser(1, true) };
        _mockDataContext.Setup(s => s.GetAsync<User>(It.IsAny<Expression<Func<User, bool>>>(), "Id", false)).ReturnsAsync(users).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _service.FilterByActiveAsync(true,"Id", false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
        _mockDataContext.Verify(data => data.GetAsync<User>(It.IsAny<Expression<Func<User, bool>>>(), "Id", false), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ReturnsUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        long userId = 8;
        var user = CreateUser(userId);
        _mockDataContext.Setup(s => s.GetByIDAsync<User>(userId)).ReturnsAsync(user).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _service.GetByIdAsync(userId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(user);
        _mockDataContext.Verify();
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        long userId = 15;
        _mockDataContext.Setup(s => s.GetByIDAsync<User>(userId)).ThrowsAsync(new ArgumentException($"User with id {userId} not found."));

        // Act: Invokes the method under test with the arranged parameters.
        var result = async () => await _service.GetByIdAsync(userId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        await result.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"User with id {userId} not found.");
    }

    [Fact]
    public async Task CreateAsync_WhenNewEntityAdded_CallsContext()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var user = CreateUser();
        _mockDataContext.Setup(s => s.CreateAsync<User>(user)).Returns(Task.CompletedTask).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        await _service.CreateAsync(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _mockDataContext.Verify(data => data.CreateAsync<User>(user), Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_ShouldCallContextUpdate()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var user = CreateUser();
        _mockDataContext.Setup(s => s.UpdateAsync<User>(user)).Returns(Task.CompletedTask).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        await _service.UpdateAsync(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _mockDataContext.Verify(data => data.UpdateAsync<User>(user), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallContextDelete()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        long userId = 8;
        _mockDataContext.Setup(s => s.DeleteAsync<User>(userId)).Returns(Task.CompletedTask).Verifiable();

        // Act: Invokes the method under test with the arranged parameters.
        await _service.DeleteAsync(userId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _mockDataContext.Verify(data => data.DeleteAsync<User>(userId), Times.Once);
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
