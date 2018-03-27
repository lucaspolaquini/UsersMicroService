using GeekBurger.Users.Binder;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using GeekBurger.Users.Services;
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
        private IServiceBus Bus { get; }

        public UserController(IDetector detector, IRestrictionsRepository restrictionsRepository, IServiceBus bus)
        {
            this.Detector = detector;
            this.RestrictionsRepository = restrictionsRepository;
            this.Bus = bus;
        }

        [HttpPost("")]
        public IActionResult Post([ModelBinder(typeof(ByteArrayModelBinder))]byte[] picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var faceStream = new MemoryStream(picture);

            //DO NOT await - make it an async call
            Detector.DetectAsync(faceStream);

            return Ok();
        }

        [HttpPost("{user}/foodrestrictions")]
        public async Task<IActionResult> Post(Guid user, [FromBody]FoodRestrictionsList restrictions)
        {
            if (restrictions?.Others?.Length > 0 || restrictions?.Restrictions?.Length > 0)
            {
                List<string> restrictionsList = new List<string>();

                if (restrictions?.Others?.Length > 0)
                    foreach (string item in restrictions?.Others)
                        RestrictionsRepository.Add(new Restriction() { UserId = user, Name = item, Other = true });

                if (restrictions?.Restrictions?.Length > 0)
                    foreach (string item in restrictions?.Restrictions)
                        RestrictionsRepository.Add(new Restriction() { UserId = user, Name = item, Other = false });

                RestrictionsRepository.Save();

                UserRetrievedMessage message = new UserRetrievedMessage
                {
                    UserId = user,
                    AreRestrictionsSet = true,
                    Restrictions = restrictions
                };

                await Bus.PostMessage(UserRetrievedMessage.DefaultTopic, message);

                return Ok();
            }

            return BadRequest("Submit at least one restriction");
        }
    }
}
