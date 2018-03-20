﻿using GeekBurger.Users.Contract;
using GeekBurger.Users.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GeekBurger.Users.Model
{
    class Detector : IDetector
    {
        private readonly IFaceDetection faceDetector;
        private readonly IServiceBus bus;

        public Detector(IFaceDetection faceDetector, IServiceBus bus)
        {
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

                    if (result.Confidence <= 0.5)
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
                        UserRetrievedMessage message = new UserRetrievedMessage();
                        message.UserId = result.userId;
                        message.AreRestrictionsSet = true;
                        //TODO: Get restrictions
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
