using ApplicationCore.Services;
using BeautyAtHome.Utils;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            try
            {
                await auth.VerifyIdTokenAsync(authCM.AccessToken);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

            AuthVM response;
            try
            {
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

                string customToken = await auth.CreateCustomTokenAsync(uid.ToString(), additionalClaims);

                response = new AuthVM()
                {
                    Uid = accountCreated.Id,
                    DisplayName = accountCreated.DisplayName,
                    Role = accountCreated.Role,
                    AccessToken = customToken,
                    ExpiresIn = Constants.EXPIRES_IN_AN_HOUR
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
