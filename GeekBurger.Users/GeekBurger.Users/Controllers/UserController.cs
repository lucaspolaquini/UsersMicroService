using GeekBurger.Users.Contract;
using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IRestrictionsRepository RestrictionsRepository { get; }

        public UserController(IDetector detector, IRestrictionsRepository restrictionsRepository)
        {
            this.Detector = detector;
            this.RestrictionsRepository = restrictionsRepository;
        }

        [HttpPost("")]
        public IActionResult Post()
        {
            if (Request?.ContentLength == 0)
            {
                return BadRequest("Please send a image file with name facePicture");
            }

            if (Request?.ContentLength > MaxImageSize)
            {
                return BadRequest("Image too big");
            }
            
            var faceStream = new MemoryStream();
            Request.Body.CopyTo(faceStream);
            faceStream.Seek(0, SeekOrigin.Begin);
            //var faceStream = facePicture.OpenReadStream();

            //DO NOT await - make it an async call
            Detector.DetectAsync(faceStream);

            return Ok();
        }

        [HttpPost("{user}/foodrestrictions")]
        public IActionResult Post(Guid user, FoodRestrictionsList restrictions)
        {
            if (restrictions?.Others?.Length > 0 || restrictions?.Restrictions?.Length > 0)
            {
                //TODO: Verificar se restrição já existe
                List<string> restrictionsList = new List<string>();

                if (restrictions?.Others?.Length > 0)
                    foreach (string item in restrictions?.Others)
                        RestrictionsRepository.Add(new Restriction() { UserId = user, Name = item, Other = true });

                if (restrictions?.Restrictions?.Length > 0)
                    foreach (string item in restrictions?.Restrictions)
                        RestrictionsRepository.Add(new Restriction() { UserId = user, Name = item, Other = false });

                RestrictionsRepository.Save();

                return Ok();
            }

            return BadRequest("Submit at least one restriction");
        }
    }
}
