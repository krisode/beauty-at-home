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

        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RegisterAccount([FromForm] AccountFormDataCM account)
        {
            if (string.IsNullOrEmpty(account.Email))
            {
                return BadRequest();
            }

            Account accountCreated = _accountService.GetByEmail(account.Email);
            if(accountCreated != null)
            {
                return BadRequest("Email exists");
            }

            accountCreated = _mapper.Map<Account>(account);
            List<Image> listImgAvatar = new List<Image>();
            List<Image> listImgCertificates = new List<Image>();
            if (account.ImagesAvatar != null)
            {
                foreach (IFormFile file in account.ImagesAvatar)
                {
                    string avatar = await _uploadFileService.UploadFile("token", file, "customer", "avatar");
                    Image image = new Image();
                    image.ImageUrl = avatar;
                    image.Description = "customer-avatar";
                    listImgAvatar.Add(image);
                }
            }
            if(account.ImagesCertificates != null)
            {
                foreach (IFormFile file in account.ImagesAvatar)
                {
                    string certificate = await _uploadFileService.UploadFile("token", file, "customer", "certificates");
                    Image image = new Image();
                    image.ImageUrl = certificate;
                    image.Description = "customer-certificates";
                    listImgAvatar.Add(image);
                }
            }
            listImgAvatar.AddRange(listImgCertificates);
            accountCreated.Gallery = new Gallery()
            {
                Images = listImgAvatar,
                Description = accountCreated.DisplayName + "_Info",
                Name = "Ảnh cá nhân"
            };

            accountCreated.Status = "NEW";
            accountCreated.Role = "WORKER";
            accountCreated.IsBeautyArtist = true;
            
            await _accountService.AddAsync(accountCreated);
            await _accountService.Save();
            
            return Ok("Đăng ký tài khoản thành công");
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
                        Gallery = gallery,
                        IsBeautyArtist = true
                    };
                    await _accountService.AddAsync(accountCreated);
                    await _accountService.Save();
                }
                else
                {
                    if (!accountCreated.Status.Equals("ACTIVE"))
                    {
                        return BadRequest();
                    }
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
                    Email = accountCreated.Email,
                    Phone = accountCreated.Phone,
                    Role = accountCreated.Role,
                    AccessToken = accessToken,
                    ExpiresIn = Constants.EXPIRES_IN_DAY,
                    Gallery = _mapper.Map<GalleryVM>(accountCreated.Gallery)
                };
                if (accountCreated.Role.Equals("ADMIN"))
                {
                    response.Addresses = accountCreated.Addresses;
                }
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
