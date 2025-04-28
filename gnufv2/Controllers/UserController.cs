using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Gnuf.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly GnufContext _context;

        public UserController(GnufContext context)
        {
            _context = context;
        }

        // 4.2.1 Get User Profile
        [HttpGet("profile/{user_id}")]
        public async Task<ActionResult> GetUserProfile(int user_id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user_id);
            if (user == null)
            {
                return NotFound();
            }

            var result = new
            {
                id = user.UserId,
                email = user.Email,
                username = user.Username,
                img_path = user.ImagePath,
                post_ids = user.PostIds,
                community_ids = user.CommunityIds,
                tags = user.Tags,
                admin = user.IsAdmin,
                display_name = user.DisplayName,
                description = user.Description,
            };

            return Ok(result);
        }

        // 4.2.2 Delete User Account
        [HttpDelete("remove/{user_id}")]
        public async Task<ActionResult> DeleteUser(int user_id)
        {
            var user = await _context.Users.FindAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // 4.2.3 Update User Profile (user)
        [HttpPut("update/user/{user_id}")]
        public async Task<ActionResult> UpdateUserProfile(int user_id, [FromBody] UserStructure update)
        {
            var user = await _context.Users.FindAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(update.ImagePath))
                user.ImagePath = update.ImagePath;

            if (!string.IsNullOrWhiteSpace(update.Password))
                user.Password = update.Password;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // 4.2.4 Update User Profile (backend)
        [HttpPut("update/backend/{user_id}")]
        public async Task<ActionResult> UpdateUserBackend(int user_id, [FromBody] UserStructure update)
        {
            var user = await _context.Users.FindAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            user.PostIds = update.PostIds ?? "";
            user.CommunityIds = update.CommunityIds ?? "";
            user.Tags = update.Tags ?? "";

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
