using ApplicationCore.Services;
using BeautyAtHome.ExternalService;
using BeautyAtHome.Utils;
using BeautyAtHome.ViewModels;
using Firebase.Auth;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BeautyAtHome.Utils.Constants;

namespace BeautyAtHome.Controllers
{
    [Route("api/v1.0/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUploadFileService _uploadFileService;
        private readonly IPushNotificationService _pushNotificationService;

        public AuthController(IAccountService accountService, IJwtTokenProvider jwtTokenProvider, IUploadFileService uploadFileService, IPushNotificationService pushNotificationService)
        {
            _accountService = accountService;
            _jwtTokenProvider = jwtTokenProvider;
            _uploadFileService = uploadFileService;
            _pushNotificationService = pushNotificationService;
        }

        [HttpPost("upload-image-flutter")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> uploadImageToFlutter([FromForm] IFormFile file)
        {
            try
            {
                string idToken = Request.Headers[HeaderClaims.FIREBASE_AUTH];
                string fileUrl = await _uploadFileService.UploadFile(idToken, file, "service", "service-detail");
                return Ok(fileUrl);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        [HttpGet("test-push")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> testPush()
        {
            var task = await _pushNotificationService.SendMessage("Hello Trang", "Bạn đã nhận được đơn hàng.\nMakeup - Làm tóc xoăn tự nhiên", "Randomizer", @"https://paulaschoice.vn/wp-content/uploads/2019/08/nguyen-tac-make-up.jpg");
            return Ok(task);
        }



        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> LoginAccount([FromBody] AuthCM authCM)
        {
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            string email;
            try
            {
                var token  = await auth.VerifyIdTokenAsync(authCM.IdToken);
                email = (string) token.Claims[TokenClaims.EMAIL];
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            AuthVM response;
            try
            {
                Account accountCreated = _accountService.GetByEmail(email);
                
                if (accountCreated == null)
                {
                    accountCreated = new Account()
                    {
                        DisplayName = authCM.DisplayName,
                        Email = email,
                        Role = Constants.Role.ADMIN,
                        Status = Constants.AccountStatus.ACTIVE
                    };
                    await _accountService.AddAsync(accountCreated);
                    await _accountService.Save();
                }
                var uid = accountCreated.Id;

                string accessToken = await _jwtTokenProvider.GenerateToken(accountCreated);

                response = new AuthVM()
                {
                    Uid = accountCreated.Id,
                    DisplayName = accountCreated.DisplayName,
                    Role = accountCreated.Role,
                    AccessToken = accessToken,
                    ExpiresIn = Constants.EXPIRES_IN_DAY
                };
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(e.Message);
            }
            return Ok(response);
        }
    }
}
