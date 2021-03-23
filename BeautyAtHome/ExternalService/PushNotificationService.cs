using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.ExternalService
{
    public interface IPushNotificationService
    {
        Task<string> SendMessage(string title, string body, string topic, Dictionary<String, String> additionalDatas);   
    }
    public class PushNotificationService : IPushNotificationService
    {
        public async Task<string> SendMessage(string title, string body, string topic, Dictionary<String, String> additionalDatas)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl = "https://png.pngtree.com/element_our/20190530/ourlarge/pngtree-520-couple-avatar-boy-avatar-little-dinosaur-cartoon-cute-image_1263411.jpg",
                },
                Topic = "/topics/" + topic,
                Data = additionalDatas,
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            return await messaging.SendAsync(message);
        }

            
        
    }
}
