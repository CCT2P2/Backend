using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;

namespace Gnuf.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {

        private readonly string _imageUploadFolderPath;

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _imageUploadFolderPath = Path.Combine(webHostEnvironment.ContentRootPath, "uploads");

            if (!Directory.Exists(_imageUploadFolderPath))
            {
                Directory.CreateDirectory(_imageUploadFolderPath);
            }
        }

        private bool IsAllowedType(IFormFile file)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension);
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (!IsAllowedType(file))
            {
                return BadRequest("File is not allowed");
            }

            var uuid = Guid.NewGuid();
            var fileName = uuid + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(_imageUploadFolderPath, fileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}"});
        }
    }
}