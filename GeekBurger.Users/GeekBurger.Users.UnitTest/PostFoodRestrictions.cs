using FluentAssertions;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Controllers;
using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using GeekBurger.Users.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace GeekBurger.Users.UnitTest
{
    [TestClass]
    public class PostFoodRestrictions
    {
        UserController controller;

        [TestInitialize]
        public void Iniciar()
        {
            controller = new UserController(Mock.Of<IDetector>(), Mock.Of<IRestrictionsRepository>(), Mock.Of<IServiceBus>());
        }

        [TestMethod]
        public void VerifyOkReturn()
        {
            FoodRestrictionsList foodRestrictionsList = new FoodRestrictionsList
            {
                Restrictions = new string[1]
            };
            foodRestrictionsList.Restrictions[0] = "Lactose";

            var result = controller.Post(Guid.NewGuid(), foodRestrictionsList).Result;

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void VerifyNullBadRequest()
        {
            FoodRestrictionsList foodRestrictionsList = null;

            var result = controller.Post(Guid.NewGuid(), foodRestrictionsList).Result;

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void VerifyNullListBadRequest()
        {
            FoodRestrictionsList foodRestrictionsList = new FoodRestrictionsList();

            var result = controller.Post(Guid.NewGuid(), foodRestrictionsList).Result;

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void VerifyEmptyListBadRequest()
        {
            FoodRestrictionsList foodRestrictionsList = new FoodRestrictionsList
            {
                Others = new string[0],
                Restrictions = new string[0]
            };

            var result = controller.Post(Guid.NewGuid(), foodRestrictionsList).Result;

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
