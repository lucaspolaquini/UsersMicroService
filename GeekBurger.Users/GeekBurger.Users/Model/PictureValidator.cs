using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GeekBurger.Users.Model
{
    public class PictureValidator : IPictureValidator
    {
        private static readonly int MaxImageSize = 4 * 1024 * 1024;//4MB

        public void Validate(byte[] picture, ModelStateDictionary modelState)
        {
            if (picture.Length == 0)
            {
                modelState.AddModelError("", "Please send some image!");
            }

            if (picture.Length > MaxImageSize)
            {
                modelState.AddModelError("", $"Image too big! Max size: {MaxImageSize} bytes");
            }
        }
    }
}
