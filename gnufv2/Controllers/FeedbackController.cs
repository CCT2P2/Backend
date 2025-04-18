
// Controllers/FeedbackController.cs
using Microsoft.AspNetCore.Mvc;
using Gnuf.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly GnufContext _context;

    public FeedbackController(GnufContext context)
    {
        _context = context;
    }

    // POST: api/feedback/submit
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackStructure feedback)
    {
        if (feedback.Rating < 1 || feedback.Rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5.");
        }

        // Optionally set the timestamp here if it's not client-provided
        if (feedback.Timestamp == 0)
        {
            feedback.Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        _context.Feedback.Add(feedback);
        await _context.SaveChangesAsync();

        return Ok(new { feedback.Id });
    }

    // GET: api/feedback/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllFeedback()
    {
        var feedbackList = await _context.Feedback.ToListAsync();
        return Ok(feedbackList);
    }
}
