using AiDrivenMedicalPlatformAPIs.Models;
using MedicalProj.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCheckController : ControllerBase
    {
        private readonly MedicalDbContext Context;

        public UserCheckController(MedicalDbContext _context)
        {
            Context = _context;
        }

        [AllowAnonymous]
        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmail([FromBody] CheckEmailRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email))
            {
                return BadRequest(new { message = "Email is required!" });
            }

            var email = request.Email.ToLowerInvariant();

            var emailExists = await Context.Users.AnyAsync(u => u.Email.ToLower() == email);

            return Ok(new
            {
                isRegistered = emailExists
            });
        }
    }
}
