using HamzaYasinAssessmentProject.Server.Common;
using HamzaYasinAssessmentProject.Server.Data;
using HamzaYasinAssessmentProject.Shared.Models;
using HamzaYasinAssessmentProject.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace HamzaYasinAssessmentProject.TestProject.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private AppDbContext _context;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            // Use a fresh in-memory database for each test
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userService = new UserService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        // --------------------------------------------------------
        // GetAllUsersAsync
        // --------------------------------------------------------

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            _context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@test.com", Active = true, Roles = ["Admin"] },
                new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@test.com", Active = false, Roles = ["User"] }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        // --------------------------------------------------------
        // GetActiveUsersAsync
        // --------------------------------------------------------

        [Test]
        public async Task GetActiveUsersAsync_ReturnsOnlyActiveUsers()
        {
            // Arrange
            _context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@test.com", Active = true, Roles = ["User"] },
                new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@test.com", Active = false, Roles = ["User"] }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetActiveUsersAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Alice"));
        }

        [Test]
        public async Task GetActiveUsersAsync_ReturnUsersSortedByName()
        {
            // Arrange
            _context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Name = "Charlie", Email = "charlie@test.com", Active = true, Roles = ["User"] },
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@test.com", Active = true, Roles = ["User"] },
                new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@test.com", Active = true, Roles = ["User"] }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetActiveUsersAsync();

            // Assert
            Assert.That(result[0].Name, Is.EqualTo("Alice"));
            Assert.That(result[1].Name, Is.EqualTo("Bob"));
            Assert.That(result[2].Name, Is.EqualTo("Charlie"));
        }

        // --------------------------------------------------------
        // UpdateUserStatusAsync
        // --------------------------------------------------------

        [Test]
        public async Task UpdateUserStatusAsync_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Act
            var result = await _userService.UpdateUserStatusAsync(Guid.NewGuid(), false);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceResultStatus.NotFound));
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task UpdateUserStatusAsync_ReturnsForbidden_WhenDisablingAdminUser()
        {
            // Arrange
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Alice",
                Email = "alice@test.com",
                Active = true,
                Roles = ["Admin"]
            };
            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.UpdateUserStatusAsync(adminUser.Id, false);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceResultStatus.Forbidden));
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task UpdateUserStatusAsync_DisablesUser_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Bob",
                Email = "bob@test.com",
                Active = true,
                Roles = ["User"]
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.UpdateUserStatusAsync(user.Id, false);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceResultStatus.Ok));
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data!.Active, Is.False);
        }

        [Test]
        public async Task UpdateUserStatusAsync_EnablesUser_WhenUserIsDisabled()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Bob",
                Email = "bob@test.com",
                Active = false,
                Roles = ["User"]
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.UpdateUserStatusAsync(user.Id, true);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceResultStatus.Ok));
            Assert.That(result.Data!.Active, Is.True);
        }
    }
}