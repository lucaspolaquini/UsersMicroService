using GeekBurger.Users.Contract;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Users.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private int MaxImageSize = 4 * 1024 * 1024;//4MB

        [HttpPost("")]
        public IActionResult Post([FromBody]byte[] userFace)
        {
            if (userFace.Length > MaxImageSize)
            {
                return BadRequest("Image too big");
            }

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
