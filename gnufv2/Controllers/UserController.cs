using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;
using gnufv2.Extensions;
using Microsoft.AspNetCore.Authorization;


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
                user_ids = user.UserId,
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
            
            if (!User.MatchesId(user_id.ToString()) && !User.IsInRole("Admin")) return Unauthorized();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }


        // 4.3.4 Update user (backend)
        [HttpPut("update/backend")]
        [Authorize]
        public async Task<ActionResult> UpdateUserProfileBackend([FromBody] Gnuf.Models.User.UserQueryParameters query)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.UserId == query.Id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!string.IsNullOrEmpty(query.CommunityIDs))
            {
                if (query.Action == "join")
                {
                    // Append community
                    user.CommunityIds = string.IsNullOrEmpty(user.CommunityIds)
                        ? query.CommunityIDs
                        : user.CommunityIds + "," + query.CommunityIDs;
                }
                else if (query.Action == "leave")
                {
                    // Remove community
                    var currentIds = user.CommunityIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
                    currentIds.Remove(query.CommunityIDs);

                    user.CommunityIds = string.Join(",", currentIds);
                }
            }

            await _context.SaveChangesAsync();
            return Ok("User updated successfully.");
        }


    }
}
