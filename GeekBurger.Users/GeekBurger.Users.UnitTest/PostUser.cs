using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GeekBurger.Users.Controllers;
using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GeekBurger.Users.UnitTest
{
    [TestClass]
    public class PostUser
    {
        UserController controller;

        [TestInitialize]
        public void Iniciar()
        {
            controller = new UserController(Mock.Of<IDetector>(), Mock.Of<IRestrictionsRepository>());
        }

        [TestMethod]
        public void VerifyOkReturn()
        {
            var parameterMock = new Mock<IFormFile>();
            parameterMock.Setup(p => p.Length).Returns(0);
            parameterMock.Setup(p => p.FileName).Returns("fototeste.jpg");
            parameterMock.Setup(p => p.OpenReadStream()).Returns(new MemoryStream());

            var result = controller.Post(new byte[0]);

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void VerifyBadRequest()
        {
            int maxSizePlusOne = 4 * 1024 * 1024 + 1;

            var result = controller.Post(new byte[maxSizePlusOne]);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
