using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HamzaYasinAssessmentProject.Shared.Services;
using HamzaYasinAssessmentProject.Shared.Models;
using System.Collections.ObjectModel;

namespace HamzaYasinAssessmentProject.Shared.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        private readonly IUserApiService _userApiService;

        [ObservableProperty]
        private ObservableCollection<User> _users = [];

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _showActiveOnly;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool _hasError;

        public UsersViewModel(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }




        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            IsLoading = true;
            HasError = false;
            ErrorMessage = null;

            try
            {
                var users = ShowActiveOnly
                    ? await _userApiService.GetActiveUsersAsync()
                    : await _userApiService.GetAllUsersAsync();

                Users = new ObservableCollection<User>(users);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to load users: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task ToggleUserStatusAsync(User user)
        {
            var (success, errorMessage) = await _userApiService.UpdateUserStatusAsync(user.Id, !user.Active);

            if (!success)
            {
                ShowError(errorMessage ?? "An unexpected error occurred.");
                return;
            }

            await LoadUsersAsync();
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }

        [RelayCommand]
        private void DismissError()
        {
            ErrorMessage = null;
            HasError = false;
        }
    }
}