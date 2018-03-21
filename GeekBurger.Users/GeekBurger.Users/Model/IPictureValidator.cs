using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Model
{
    interface IPictureValidator
    {
        void Validate(byte[] picture, ModelStateDictionary modelState);
    }
}
