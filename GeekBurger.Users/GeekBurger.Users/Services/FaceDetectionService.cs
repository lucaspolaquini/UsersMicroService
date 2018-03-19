using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Users.Entities;

namespace GeekBurger.Users.Services
{
    public class FaceDetectionService : IFaceDetection
    {
        public Task<Face> Detect(Stream picture)
        {
            throw new NotImplementedException();
        }

        public Task<(Guid userId, double Confidence)> FindSimilars(Face face)
        {
            throw new NotImplementedException();
        }

        public Task Save(Guid userID, Face face)
        {
            throw new NotImplementedException();
        }
    }
}
