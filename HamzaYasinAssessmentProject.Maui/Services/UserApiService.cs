using HamzaYasinAssessmentProject.Shared.Models;
using HamzaYasinAssessmentProject.Shared.Services;
using HamzaYasinAssessmentProject.Shared.ViewModels;
using System.Net;
using System.Net.Http.Json;

namespace HamzaYasinAssessmentProject.Maui.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<User>>("api/users") ?? [];
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<User>>("api/users/active") ?? [];
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateUserStatusAsync(Guid id, bool active)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/users/{id}/status", new { Active = active });

                if (response.IsSuccessStatusCode)
                    return (true, null);

                // Read the error message from the API response body
                var message = await response.Content.ReadAsStringAsync();

                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound => (false, "User not found."),
                    HttpStatusCode.Forbidden => (false, message),
                    _ => (false, "An unexpected error occurred.")
                };
            }
            catch (Exception ex)
            {
                return (false, $"Could not connect to the server: {ex.Message}");
            }
        }
    }
}