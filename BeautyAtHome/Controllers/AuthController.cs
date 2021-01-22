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

                Account accountCreated = _accountService.GetEnumList(acc => acc.Email == authCM.Email).First();

                if (accountCreated == null)
                {
                    accountCreated = new Account()
                    {
                        Name = authCM.DisplayName,
                        Email = authCM.Email,
                        Role = Constants.Role.ADMIN
                    };
                    _accountService.Add(accountCreated);
                }
                var uid = accountCreated.Id;
                var additionalClaims = new Dictionary<string, object>()
                {
                    { "role", accountCreated.Role},
                };

                string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid.ToString(), additionalClaims);

                response = new AuthVM()
                {
                    AccessToken = customToken,
                    DisplayName = accountCreated.Name,
                    Email = accountCreated.Email,
                    Phone = accountCreated.Phone,
                    Addresses = accountCreated.Addresses
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
