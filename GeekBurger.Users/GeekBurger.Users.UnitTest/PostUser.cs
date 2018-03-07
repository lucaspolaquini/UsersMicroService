using FluentAssertions;
using GeekBurger.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeekBurger.Users.UnitTest
{
    [TestClass]
    public class PostUser
    {
        [TestMethod]
        public void VerifyOkReturn()
        {
            UserController controller = new UserController();
            var parameter = new byte[0];

            var result = controller.Post(parameter);

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void VerifyBadRequest()
        {
            UserController controller = new UserController();
            var parameter = new byte[4*1024*1024 + 1];

            var result = controller.Post(parameter);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
