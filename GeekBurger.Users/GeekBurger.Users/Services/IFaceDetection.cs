using GeekBurger.Users.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Services
{
    interface IFaceDetection
    {
        Task<FacePicture> Detect(Stream picture);

        Task<(Guid userId, double Confidence)?> FindSimilars(FacePicture face);

        Task Save(Guid userID, FacePicture face);
    }
}
