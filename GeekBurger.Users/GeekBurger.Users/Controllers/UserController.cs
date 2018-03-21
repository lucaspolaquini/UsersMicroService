using GeekBurger.Users.Binder;
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
        private IDetector Detector { get; }
        private IRestrictionsRepository RestrictionsRepository { get; }

        public UserController(IDetector detector, IRestrictionsRepository restrictionsRepository)
        {
            this.Detector = detector;
            this.RestrictionsRepository = restrictionsRepository;
        }

        [HttpPost("")]
        public IActionResult Post([ModelBinder(typeof(ByteArrayModelBinder))]byte[] picture)
        {
            var faceStream = new MemoryStream(picture);

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
