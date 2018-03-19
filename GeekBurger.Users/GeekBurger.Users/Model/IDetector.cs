using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Model
{
    public interface IDetector
    {
        Task DetectAsync(Stream picture);
    }
}
