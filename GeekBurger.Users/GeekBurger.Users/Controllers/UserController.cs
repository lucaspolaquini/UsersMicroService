using GeekBurger.Users.Contract;
using GeekBurger.Users.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeekBurger.Users.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private static readonly int MaxImageSize = 4 * 1024 * 1024;//4MB
        private static readonly Regex fileTypeRegex = new Regex(@"\.(jpg|png|jpeg|bmp)");

        private IDetector Detector { get; }

        public UserController(IDetector detector)
        {
            this.Detector = detector;
        }


        [HttpPost("")]
        public IActionResult Post(IFormFile facePicture)
        {
            if(facePicture == null)
            {
                return BadRequest("Please send a image file with name facePicture");
            }

            if (!fileTypeRegex.IsMatch(facePicture.FileName))
            {
                return BadRequest("File type not supported");
            }

            if (facePicture.Length > MaxImageSize)
            {
                return BadRequest("Image too big");
            }

            var faceStream = facePicture.OpenReadStream();

            //DO NOT await - make it an async call
            Detector.DetectAsync(faceStream);

            return Ok();
        }

        [HttpPost("{user}/foodrestrictions")]
        public IActionResult Post(FoodRestrictionsList restrictions)
        {
            if (restrictions?.Others?.Length > 0 || restrictions?.Restrictions?.Length > 0)
            {
                return Ok();
            }

            return BadRequest("Submit at least one restriction");
        }
    }
}
