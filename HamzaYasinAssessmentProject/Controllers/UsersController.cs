using HamzaYasinAssessmentProject.Server.Common;
using HamzaYasinAssessmentProject.Server.Services;
using HamzaYasinAssessmentProject.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HamzaYasinAssessmentProject.Server.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET /api/users/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }

        // PUT /api/users/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusDto dto)
        {
            var result = await _userService.UpdateUserStatusAsync(id, dto.Active);

            return result.Status switch
            {
                ServiceResultStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceResultStatus.Forbidden => StatusCode(403, result.ErrorMessage),
                _ => Ok(result.Data)
            };
        }

    }
}
