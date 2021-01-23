using ApplicationCore.Services;
using BeautyAtHome.Utils;
using BeautyAtHome.ViewModels;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeautyAtHome.Controllers
{
    [Route("api/v1.0/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> LoginAccount([FromBody] AuthCM authCM)
        {
            AuthVM response = null;
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            try
            {
                var tokenResponse = await auth.VerifyIdTokenAsync(authCM.AccessToken);

                Account accountCreated = _accountService.GetByEmail(authCM.Email);

                if (accountCreated == null)
                {
                    accountCreated = new Account()
                    {
                        DisplayName = authCM.DisplayName,
                        Email = authCM.Email,
                        Role = Constants.Role.ADMIN,
                        Status = Constants.AccountStatus.ACTIVE
                    };
                    _accountService.Add(accountCreated);
                    await _accountService.Save();
                }
                var uid = accountCreated.Id;
                var additionalClaims = new Dictionary<string, object>()
                {
                    { "role", accountCreated.Role},
                };

                string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid.ToString(), additionalClaims);

                response = new AuthVM()
                {
                    Id = accountCreated.Id,
                    AccessToken = customToken,
                    DisplayName = accountCreated.DisplayName
                };
            }
            catch (FirebaseException)
            {
                return Unauthorized();
            }
            return Ok(response);
        }
    }
}
