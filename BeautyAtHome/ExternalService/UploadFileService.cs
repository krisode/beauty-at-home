using BeautyAtHome.Utils;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BeautyAtHome.Utils.Constants;

namespace BeautyAtHome.ExternalService
{
    public interface IUploadFileService
    {
        Task<string> UploadFile(IFormFile file, string token, string bucket, string directory);
    }
    public class UploadFileService : IUploadFileService
    {
        private readonly IConfiguration _configuration;
        
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public UploadFileService(IConfiguration configuration, IJwtTokenProvider jwtTokenProvider)
        {
            _configuration = configuration;
            
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<string> UploadFile(IFormFile file, string token, string bucket, string directory)
        {
            if(token == null)
            {
                throw new Exception("Failed to authenticate user!");
            }
            string uid = await _jwtTokenProvider.GetPayloadFromToken(token, TokenClaims.UID);

            var customToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);

            var task = new FirebaseStorage(
                _configuration["Firebase:Bucket"],
                new FirebaseStorageOptions()
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(customToken)
                });
            string fileExtension = Path.GetExtension(file.FileName);
            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + "." + fileExtension;
            return await task.Child(bucket)
                .Child(directory)
                .Child(fileExtension)
                .PutAsync(file.OpenReadStream());
        }
    }
}
