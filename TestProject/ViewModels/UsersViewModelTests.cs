using HamzaYasinAssessmentProject.Shared.Services;
using HamzaYasinAssessmentProject.Shared.ViewModels;
using HamzaYasinAssessmentProject.Shared.Models;
using NSubstitute;

namespace HamzaYasinAssessmentProject.Tests.ViewModels
{
    [TestFixture]
    public class UsersViewModelTests
    {
        private IUserApiService _userApiService;
        private UsersViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            // Use NSubstitute to create a fake version of IUserApiService
            // This means we're testing the ViewModel in isolation
            // without needing a real API or database
            _userApiService = Substitute.For<IUserApiService>();
            _viewModel = new UsersViewModel(_userApiService);
        }

        // --------------------------------------------------------
        // LoadUsersAsync
        // --------------------------------------------------------

        [Test]
        public async Task LoadUsersAsync_LoadsAllUsers_WhenShowActiveOnlyIsFalse()
        {
            // Arrange - set up the fake service to return a list of users
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@test.com", Active = true,  Roles = ["Admin"] },
                new User { Id = Guid.NewGuid(), Name = "Bob",   Email = "bob@test.com",   Active = false, Roles = ["User"] }
            };
            _userApiService.GetAllUsersAsync().Returns(users);
            _viewModel.ShowActiveOnly = false;

            // Act
            await _viewModel.LoadUsersCommand.ExecuteAsync(null);

            // Assert
            Assert.That(_viewModel.Users, Has.Count.EqualTo(2));
            await _userApiService.Received(1).GetAllUsersAsync();
            await _userApiService.DidNotReceive().GetActiveUsersAsync();
        }

        [Test]
        public async Task LoadUsersAsync_LoadsActiveUsers_WhenShowActiveOnlyIsTrue()
        {
            // Arrange
            var activeUsers = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@test.com", Active = true, Roles = ["User"] }
            };
            _userApiService.GetActiveUsersAsync().Returns(activeUsers);
            _viewModel.ShowActiveOnly = true;

            // Act
            await _viewModel.LoadUsersCommand.ExecuteAsync(null);

            // Assert
            Assert.That(_viewModel.Users, Has.Count.EqualTo(1));
            await _userApiService.Received(1).GetActiveUsersAsync();
            await _userApiService.DidNotReceive().GetAllUsersAsync();
        }

        [Test]
        public async Task LoadUsersAsync_SetsHasError_WhenServiceThrows()
        {
            // Arrange - make the fake service throw an exception
            _userApiService.GetAllUsersAsync().Returns(Task.FromException<List<User>>(new Exception("Network error")));

            // Act
            await _viewModel.LoadUsersCommand.ExecuteAsync(null);

            // Assert
            Assert.That(_viewModel.HasError, Is.True);
            Assert.That(_viewModel.ErrorMessage, Does.Contain("Network error"));
        }

        [Test]
        public async Task LoadUsersAsync_SetsIsLoading_WhileLoadingThenFalseAfter()
        {
            // Arrange
            _userApiService.GetAllUsersAsync().Returns([]);

            // Act
            await _viewModel.LoadUsersCommand.ExecuteAsync(null);

            // Assert - IsLoading should be false once complete
            Assert.That(_viewModel.IsLoading, Is.False);
        }

        // --------------------------------------------------------
        // ToggleUserStatusAsync
        // --------------------------------------------------------

        [Test]
        public async Task ToggleUserStatusAsync_SetsHasError_WhenServiceReturnsFalse()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@test.com", Active = true, Roles = ["User"] };
            _userApiService.UpdateUserStatusAsync(user.Id, false).Returns((false, "An unexpected error occurred."));

            // Act
            await _viewModel.ToggleUserStatusCommand.ExecuteAsync(user);

            // Assert
            Assert.That(_viewModel.HasError, Is.True);
            Assert.That(_viewModel.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
        }

        [Test]
        public async Task ToggleUserStatusAsync_ReloadsUsers_WhenSuccessful()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@test.com", Active = true, Roles = ["User"] };
            _userApiService.UpdateUserStatusAsync(user.Id, false).Returns((true, null));
            _userApiService.GetAllUsersAsync().Returns([]);

            // Act
            await _viewModel.ToggleUserStatusCommand.ExecuteAsync(user);

            // Assert - GetAllUsersAsync should be called again to refresh the list
            await _userApiService.Received(1).GetAllUsersAsync();
        }

        // --------------------------------------------------------
        // DismissError
        // --------------------------------------------------------

        [Test]
        public void DismissError_ClearsErrorMessageAndHasError()
        {
            // Arrange - manually set an error state
            _viewModel.HasError = true;
            _viewModel.ErrorMessage = "Something went wrong";

            // Act
            _viewModel.DismissErrorCommand.Execute(null);

            // Assert
            Assert.That(_viewModel.HasError, Is.False);
            Assert.That(_viewModel.ErrorMessage, Is.Null);
        }
    }
}