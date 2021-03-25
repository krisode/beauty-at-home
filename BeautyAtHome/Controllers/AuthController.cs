using ApplicationCore.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthController(IAccountService accountService, IJwtTokenProvider jwtTokenProvider, IUploadFileService uploadFileService, IPushNotificationService pushNotificationService, IMapper mapper)
        {
            _accountService = accountService;
            _jwtTokenProvider = jwtTokenProvider;
            _uploadFileService = uploadFileService;
            _pushNotificationService = pushNotificationService;
            _mapper = mapper;
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
            //var task = await _pushNotificationService.SendMessage("Đơn của bạn đã được chấp nhận", "Makeup - Làm tóc xoăn tự nhiên", "Randomizer", @"https://techkalzen.com/wp-content/uploads/2020/02/tron-bo-nhung-hinh-anh-dep-buon-mang-tam-trang-suy-tu-1.jpg");
            return Ok("test");
        }



        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> LoginAccount([FromBody] AuthCM authCM)
        {
            if (string.IsNullOrEmpty(authCM.LoginType))
            {
                return BadRequest();
            }

            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            string email;
            try
            {
                var token = await auth.VerifyIdTokenAsync(authCM.IdToken);
                email = (string)token.Claims[TokenClaims.EMAIL];
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            AuthVM response;
            try
            {
                //string email = "maiquynhanh@gmail.com";
                Account accountCreated = _accountService.GetByEmail(email);

                if (accountCreated == null)
                {
                    Gallery gallery = null;
                    if (authCM.Avatar != null)
                    {
                        gallery = new Gallery()
                        {
                            Name = authCM.LoginType + "_GALLERY",
                            Description = "GALLERY OF " + authCM.LoginType
                        };
                        Image img = new Image()
                        {
                            Description = "IMAGES OF " + authCM.LoginType,
                            ImageUrl = authCM.Avatar
                        };
                        gallery.Images = new List<Image>();
                        gallery.Images.Add(img);
                    }

                    accountCreated = new Account()
                    {
                        DisplayName = authCM.DisplayName,
                        Email = email,
                        Role = Constants.Role.ADMIN,
                        Status = Constants.AccountStatus.ACTIVE,
                        Gallery = gallery
                    };
                    await _accountService.AddAsync(accountCreated);
                    await _accountService.Save();
                }

                string role = accountCreated.Role;
                if (role != authCM.LoginType)
                {
                    return BadRequest();
                }
                var uid = accountCreated.Id;
                string accessToken = await _jwtTokenProvider.GenerateToken(accountCreated);

                response = new AuthVM()
                {
                    Uid = accountCreated.Id,
                    DisplayName = accountCreated.DisplayName,
                    Role = accountCreated.Role,
                    AccessToken = accessToken,
                    ExpiresIn = Constants.EXPIRES_IN_DAY,
                    Gallery = _mapper.Map<GalleryVM>(accountCreated.Gallery)
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
