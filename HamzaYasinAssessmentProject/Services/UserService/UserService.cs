using HamzaYasinAssessmentProject.Server.Common;
using HamzaYasinAssessmentProject.Server.Data;
using HamzaYasinAssessmentProject.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HamzaYasinAssessmentProject.Server.Services
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Active)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {

            return await _context.Users.ToListAsync();
        }

        public async Task<ServiceResult<User>> UpdateUserStatusAsync(Guid id, bool active)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
                return ServiceResult<User>.NotFound($"User with ID {id} was not found.");

            if (!active && user.Roles.Contains("Admin"))
                return ServiceResult<User>.Forbidden("Admin users cannot be disabled.");

            user.Active = active;
            await _context.SaveChangesAsync();

            return ServiceResult<User>.Ok(user);
        }
    }
}
