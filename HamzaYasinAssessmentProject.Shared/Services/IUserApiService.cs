using HamzaYasinAssessmentProject.Shared.Models;

namespace HamzaYasinAssessmentProject.Shared.Services
{
    public interface IUserApiService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetActiveUsersAsync();
        Task<(bool Success, string? ErrorMessage)> UpdateUserStatusAsync(Guid id, bool active);
    }
}