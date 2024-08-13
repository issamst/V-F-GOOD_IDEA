//using Microsoft.AspNetCore.Http;
//using Asp_Net_Good_idea.Service.MultimediaService;

//using Microsoft.AspNetCore.Mvc;
//using System.IO;
//using System.Threading.Tasks;

//namespace Asp_Net_Good_idea.Controllers.MultimediaCapture
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class MultimediaController : ControllerBase
//    {

//        [HttpPost("upload")]
//        public async Task<IActionResult> UploadMedia([FromForm] IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest("No file uploaded.");
//            }

//            var filePath = Path.Combine("Uploads", file.FileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            return Ok(new { filePath });
//        }
//    }

//}


// CameraController.csusing Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Context;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Controllers.MultimediaCapture
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultimediaCapture : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor injection to inject AppDbContext
        public MultimediaCapture(AppDbContext context)
        {
            _context = context;
        }

        // Action method to handle POST requests for capturing images
        [HttpPost("Capture")]
        public async Task<IActionResult> Capture()
        {
            try
            {
                // Accessing the uploaded image from the request
                var imageFile = Request.Form.Files[0]; // Assuming you're uploading a single image

                // Check if an image was uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Get the file name
                    var fileName = Path.GetFileName(imageFile.FileName);

                    // Define the file path where the image will be saved
                    var filePath = Path.Combine("CapturedImages", fileName);

                    // Save the image to the specified file path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Save image information to the database
                    // You need to define this logic based on your database schema
                    // Example: _context.CapturedImages.Add(new CapturedImage { FileName = fileName, FilePath = filePath });
                    await _context.SaveChangesAsync();

                    // Return a success response
                    return Ok("Image captured and saved successfully.");
                }
                else
                {
                    // Return a bad request response if no image was uploaded
                    return BadRequest("No image uploaded.");
                }
            }
            catch (Exception ex)
            {
                // Return a server error response if an exception occurred
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

