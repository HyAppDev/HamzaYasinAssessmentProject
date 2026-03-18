using HamzaYasinAssessmentProject.Maui.Services;
using HamzaYasinAssessmentProject.Maui.Views;
using Microsoft.Extensions.Logging;
using HamzaYasinAssessmentProject.Shared.ViewModels;
using HamzaYasinAssessmentProject.Shared.Services;


namespace HamzaYasinAssessmentProject.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IUserApiService, UserApiService>();
            builder.Services.AddSingleton<UsersViewModel>();
            builder.Services.AddSingleton<UsersPage>();

            builder.Services.AddHttpClient<IUserApiService, UserApiService>(client =>
            {
                // Use your local API URL — update port to match yours
                client.BaseAddress = new Uri("https://localhost:7138/");
            });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
