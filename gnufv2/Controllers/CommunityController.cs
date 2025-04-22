using Microsoft.AspNetCore.Mvc;
using Gnuf.Models;
using Gnuf.Models.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommunityController : ControllerBase
    {
        private readonly GnufContext _context;

        public CommunityController(GnufContext context)
        {
            _context = context;
        }

        // 4.3.1 Create community
        [HttpPost("create")]
        public async Task<ActionResult> CreateCommunity([FromBody] CommunityStructure community)
        {
            // Only admins can create communities
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            if (_context.Community.Any(c => c.Name == community.Name))
            {
                return BadRequest("Community already exists");
            }

            _context.Community.Add(community);
            await _context.SaveChangesAsync();
            return Ok(new { community.CommunityID });
        }

        // 4.3.2 Get community details
        [HttpGet("details/{community_id}")]
        public async Task<ActionResult> GetCommunityDetails(int community_id)
        {
            var community = await _context.Community.FirstOrDefaultAsync(c => c.CommunityID == community_id);

            if (community == null)
            {
                return NotFound();
            }

            var response = new
            {
                id = community.CommunityID,
                name = community.Name,
                description = community.Description,
                Img_path = community.Img_path,
                MemberCount = community.MemberCount,
                // Returning CSV string directly
                Tags = community.Tags,  // Return as CSV string, e.g., "1,2,3"
                post_ids = community.PostID // Return as CSV string, e.g., "4,5,6"
            };

            return Ok(response);
        }

        // 4.3.3 Update community (user)
        [HttpPut("update/details/{community_id}")] //now there's no fecking errors, but also no fecking update, so 🤷
        public async Task<ActionResult> UpdateCommunityUser(int community_id, [FromBody] CommunityStructure update)
        {
            // Only admins can update communities
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var community = await _context.Community.FindAsync(community_id);
            if (community == null)
            {
                return NotFound();
            }
            community.Name = update.Name;
            community.Description = update.Description;
            community.Img_path = update.Img_path;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("update/backend")]
        [Authorize]
        public async Task<ActionResult> UpdateCommunityBackend([FromBody] commQueryParameters query)
        {
            var community = await _context.Community.FirstOrDefaultAsync(c => c.CommunityID == query.Id);

            if (community == null)
            {
                return NotFound("Community not found.");
            }

            if (query.MemberCount.HasValue)
            {
                community.MemberCount = query.MemberCount.Value;
            }

            if (!string.IsNullOrEmpty(query.Tags))
            {
                community.Tags = query.Tags;
            }

            if (!string.IsNullOrEmpty(query.PostID))
            {
                Console.WriteLine(query.PostID);
                community.PostID = string.IsNullOrEmpty(community.PostID) 
                    ? query.PostID 
                    : community.PostID + "," + query.PostID;
            }

            await _context.SaveChangesAsync();

            return Ok("Community updated successfully.");
        }



        // 4.3.5 Delete community
        [HttpDelete("remove/{community_id}")]
        public async Task<ActionResult> DeleteCommunity(int community_id)
        {
            // Only admins can delete communities
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var community = await _context.Community.FindAsync(community_id);
            if (community == null)
            {
                return NotFound();
            }
            // delete all posts in the community or move to id -1
            _context.Community.Remove(community);
            await _context.SaveChangesAsync();
            return Ok();


        }
        
        // 4.3.6 Get all communities
        [HttpGet("all")]
        public async Task<ActionResult<List<GetAllCommunitiesResponse>>> GetAllCommunities()
        {
            var communities = await _context.Community.ToListAsync();

            var response = communities.Select(c => new GetAllCommunitiesResponse
            {
                Names = c.Name,
                CommunityID = c.CommunityID,
                Description = c.Description
            }).ToList();

            return response;
        }
    }
}
