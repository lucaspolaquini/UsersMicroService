using GeekBurger.Users.Contract;
using GeekBurger.Users.Repository;
using GeekBurger.Users.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Model
{
    class Detector : IDetector
    {
        private readonly IRestrictionsRepository restrictionsRepository;
        private readonly IFaceDetection faceDetector;
        private readonly IServiceBus bus;

        public Detector(IFaceDetection faceDetector, IServiceBus bus, IRestrictionsRepository restrictionsRepository)
        {
            this.restrictionsRepository = restrictionsRepository;
            this.faceDetector = faceDetector;
            this.bus = bus;
        }

        public async Task DetectAsync(Stream picture)
        {
            try
            {
                var face = await faceDetector.Detect(picture);
                if (face != null)
                {
                    var result = await faceDetector.FindSimilars(face);

                    if (result?.Confidence <= 0.5)
                    {
                        Guid userID = Guid.NewGuid();
                        await faceDetector.Save(userID, face);

                        UserRetrievedMessage message = new UserRetrievedMessage();
                        message.UserId = userID;
                        message.AreRestrictionsSet = false;
                        await bus.PostMessage(UserRetrievedMessage.DefaultTopic, message);

                    }
                    else
                    {
                        var userId = (Guid)result?.userId;
                        UserRetrievedMessage message = new UserRetrievedMessage
                        {
                            UserId = userId,
                            AreRestrictionsSet = true,
                            Restrictions = new FoodRestrictionsList()
                        };
                        
                        var restrictions = restrictionsRepository.GetFoodRestrictionsByUser(userId);

                        message.Restrictions.Others = restrictions.Where(r => r.Other).Select(r => r.Name).ToArray();
                        message.Restrictions.Restrictions = restrictions.Where(r => !r.Other).Select(r => r.Name).ToArray();

                        await bus.PostMessage(UserRetrievedMessage.DefaultTopic, message);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Logging
            }
}
    }
}
