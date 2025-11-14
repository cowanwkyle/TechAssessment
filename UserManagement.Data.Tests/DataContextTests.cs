using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public async Task GetAllAsync_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com",
            IsActive = true,
            DateOfBirth = DateTime.Parse("2000-01-01")
        };
        await context.CreateAsync(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetAllAsync_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var users = await context.GetAllAsync<User>();
        var entity = users.First();
        await context.DeleteAsync<User>(entity.Id);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
        result.Count.Should().Be(users.Count - 1);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsSeededUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(11);
    }

    [Fact]
    public async Task GetAsync_WithFilter_ReturnsOnlyMatching()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAsync<User>(user => user.IsActive);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().OnlyContain(user => user.IsActive);
        result.Count.Should().BePositive();
    }

    [Fact]
    public async Task GetByIDAsync_WithValidId_ReturnsExpectedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        const long testUserId = 1;

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetByIDAsync<User>(testUserId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Id.Should().Be(testUserId);
        result.Forename.Should().Be("Peter");
    }

    [Fact]
    public async Task GetByIDAsync_WithInvalidId_ThrowsNotFound()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        const long invalidTestUserId = 999;

        // Act: Invokes the method under test with the arranged parameters.
        var result = async () =>  await context.GetByIDAsync<User>(invalidTestUserId); 

        // Assert: Verifies that the action of the method under test behaves as expected.
        await result.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"User with id {invalidTestUserId} not found.");
    }

    [Fact]
    public async Task UpdateAsync_WhenEmailIsUpdated_EntityIncludesNewEmail()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        const long testUserId = 3;
        var entity = await context.GetByIDAsync<User>(testUserId);
        var newEmail = "tCastor@example.com";
        entity.Email = newEmail;

        // Act: Invokes the method under test with the arranged parameters.
        await context.UpdateAsync<User>(entity);
        var result = await context.GetByIDAsync<User>(testUserId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Email.Should().Be(newEmail);
    }

    private DataContext CreateContext() => new();
}
