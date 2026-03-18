using HamzaYasinAssessmentProject.Server.Common;
using HamzaYasinAssessmentProject.Shared.Models;

namespace HamzaYasinAssessmentProject.Server.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetActiveUsersAsync();
        Task<ServiceResult<User>> UpdateUserStatusAsync(Guid id, bool active);
    }
}
