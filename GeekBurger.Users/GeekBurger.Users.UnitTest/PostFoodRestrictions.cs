using FluentAssertions;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeekBurger.Users.UnitTest
{
    [TestClass]
    public class PostFoodRestrictions
    {
        [TestMethod]
        public void VerifyOkReturn()
        {
            UserController controller = new UserController();
            FoodRestrictionsList foodRestrictionsList = new FoodRestrictionsList
            {
                Restrictions = new string[1]
            };
            foodRestrictionsList.Restrictions[0] = "Lactose";

            var result = controller.Post(foodRestrictionsList);

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void VerifyNullBadRequest()
        {
            UserController controller = new UserController();
            FoodRestrictionsList foodRestrictionsList = null;

            var result = controller.Post(foodRestrictionsList);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void VerifyNullListBadRequest()
        {
            UserController controller = new UserController();
            FoodRestrictionsList foodRestrictionsList = new FoodRestrictionsList();

            var result = controller.Post(foodRestrictionsList);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void VerifyEmptyListBadRequest()
        {
            UserController controller = new UserController();
            FoodRestrictionsList foodRestrictionsList = new FoodRestrictionsList
            {
                Others = new string[0],
                Restrictions = new string[0]
            };

            var result = controller.Post(foodRestrictionsList);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
