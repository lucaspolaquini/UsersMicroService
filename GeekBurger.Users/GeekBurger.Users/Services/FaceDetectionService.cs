using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Users.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;

namespace GeekBurger.Users.Services
{
    public class FaceDetectionService : IFaceDetection
    {
        public static FaceServiceClient FaceServiceClient;
        public static IConfiguration Configuration;
        public static string FaceListId;

        public FaceDetectionService(IConfiguration configuration)
        {
            Configuration = configuration;

            FaceListId = Configuration.GetSection("faceApi")["FaceListId"];

            FaceServiceClient = new FaceServiceClient(Configuration.GetSection("faceApi")["FaceAPIKey"], "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/");

            UpsertFaceListAndCheckIfContainsFace();
        }

        public async Task<FacePicture> Detect(Stream picture)
        {
            try
            {
                var faces = await FaceServiceClient.DetectAsync(picture);
                var faceReturned = faces.FirstOrDefault();

                if (faceReturned != null)
                {
                    FacePicture face = new FacePicture
                    {
                        FaceId = faceReturned.FaceId,
                        ImageStream = picture
                    };

                    return face;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(Guid userId, double Confidence)?> FindSimilars(FacePicture face)
        {
            try
            {
                var similarFaces = await FaceServiceClient.FindSimilarAsync(face.FaceId, FaceListId.ToString(), 5);

                var similarFace = similarFaces.FirstOrDefault();

                return (similarFace.PersistedFaceId, similarFace.Confidence);
            }
            catch (Exception ex)
            {
                return (face.FaceId, 0);
            }
        }

        public async Task Save(Guid userID, FacePicture face)
        {
            try
            {
                face.ImageStream.Seek(0, SeekOrigin.Begin);
                await FaceServiceClient.AddFaceToFaceListAsync(FaceListId.ToString(), face.ImageStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Face not included in Face List!");
            }
        }

        private void UpsertFaceListAndCheckIfContainsFace()
        {
            var faceLists = FaceServiceClient.ListFaceListsAsync().Result;
            var faceList = faceLists.FirstOrDefault(_ => _.FaceListId == FaceListId);

            if (faceList == null)
            {
                FaceServiceClient.CreateFaceListAsync(FaceListId, "GeekBurgerFaces", null).Wait();
            }
        }
    }
}
