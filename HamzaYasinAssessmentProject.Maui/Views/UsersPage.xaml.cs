using HamzaYasinAssessmentProject.Shared.ViewModels;

namespace HamzaYasinAssessmentProject.Maui.Views;

public partial class UsersPage : ContentPage
{
    private readonly UsersViewModel _viewModel;

    public UsersPage(UsersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUsersCommand.ExecuteAsync(null);
    }

    private async void OnFilterToggled(object? sender, ToggledEventArgs e)
    {
        await _viewModel.LoadUsersCommand.ExecuteAsync(null);
    }
}