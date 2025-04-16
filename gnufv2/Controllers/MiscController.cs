using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gnuf.Models;

namespace Gnuf.Controllers
{
    [ApiController]
    [Route("api/misc")]
    public class MiscController : ControllerBase
    {
        
        string pathToSave = "/home/slindau/image/";
        
        [HttpPost("upload/image")]
        public IActionResult Upload()
        {
            if (!Request.Form.Files.Any())
                return Ok();
            
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);
            
            var uploadedFiles = new List<object>();
            
            foreach(IFormFile file in Request.Form.Files)
            {
                
                Guid myuuid = Guid.NewGuid();
                string FileName = myuuid.ToString() + ".png";
                
                string fullPath = Path.Combine(pathToSave, FileName);            
                using FileStream stream = new(fullPath, FileMode.Create);     
                
                file.CopyTo(stream);
                
                uploadedFiles.Add(new
                {
                    OriginalName = file.FileName,
                    SavedName = FileName,
                    Size = file.Length
                });
            }

            return Ok(new
            {
                files = uploadedFiles
            });
        }
    }
}