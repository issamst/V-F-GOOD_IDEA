
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

using Asp_Net_Good_idea.Entity;
using AutoMapper;
using Asp_Net_Good_idea.Dto.Jwt;
using Asp_Net_Good_idea.Dto.UserDto;
using Asp_Net_Good_idea.Dto.Email;
using Asp_Net_Good_idea.Helpers.Email;
using Asp_Net_Good_idea.Helpers.UserHelpers;
using Asp_Net_Good_idea.Models.UserModel;
//using Asp_Net_Good_idea.UtilityService.EmailService;
using Asp_Net_Good_idea.Models.EmailModel;
using System.IO.Ports;



namespace Asp_Net_Good_idea.Service.MultimediaService
{

    public class MultimediaService
    {
        public async Task<string> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
