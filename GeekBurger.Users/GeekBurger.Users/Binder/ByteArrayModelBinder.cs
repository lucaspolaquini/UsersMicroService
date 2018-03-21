using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Binder
{
    public class ByteArrayModelBinder : IModelBinder
    {
        private readonly IPictureValidator validator;

        public ByteArrayModelBinder()
        {
            this.validator = new PictureValidator();
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(byte[]))
            {
                long size = bindingContext.HttpContext.Request.ContentLength.GetValueOrDefault(0);

                byte[] body = new byte[size];

                for (int i = 0; i < size; i++)
                {
                    body[i] = (byte)bindingContext.HttpContext.Request.Body.ReadByte();
                }

                validator.Validate(body, bindingContext.ModelState);

                if (bindingContext.ModelState.IsValid)
                {
                    bindingContext.Result = ModelBindingResult.Success(body);

                }
            }
        }
    }
}
