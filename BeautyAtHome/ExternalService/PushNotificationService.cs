using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ExternalService
{
    public interface IPushNotificationService
    {
        Task<string> SendMessage(string title, string body, string topic, string imageUrl);   
    }
    public class PushNotificationService : IPushNotificationService
    {
        public async Task<string> SendMessage(string title, string body, string topic, string imageUrl)
        {
            var dict = new Dictionary<String, String >();
            dict.Add("imageUrl", imageUrl);
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl = imageUrl
                },
                Topic = "/topics/" + topic,
                Data = dict,
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            return await messaging.SendAsync(message);
        }

            
        
    }
}
