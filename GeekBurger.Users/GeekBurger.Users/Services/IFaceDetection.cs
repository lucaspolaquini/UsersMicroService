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
        Task<Face> Detect(Stream picture);

        Task<(Guid userId, double Confidence)> FindSimilars(Face face);

        Task Save(Guid userID, Face face);
    }
}
