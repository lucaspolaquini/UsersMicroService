using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Entities
{
    public class FacePicture
    {
        public Guid FaceId { get; set; }
        public Stream ImageStream { get; set; }
    }
}
