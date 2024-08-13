using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Asp_Net_Good_idea.Context;

namespace Asp_Net_Good_idea.Controllers.ImageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        [HttpPost]


        public async Task<IActionResult> Upload()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest("Unsupported media type");
            }

            var form = await Request.ReadFormAsync();

            foreach (var file in form.Files)
            {
                var dataStream = file.OpenReadStream();
                var fileName = file.FileName.Trim('"');
                var filePath = Path.Combine("Path/To/Save", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await dataStream.CopyToAsync(fileStream);
                }
            }

            return Ok("File uploaded successfully");
        }


    }
}
