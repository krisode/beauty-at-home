using ApplicationCore.Services;
using AutoMapper;
using BeautyAtHome.Utils;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyAtHome.Controllers
{
    [Route("api/v1.0/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Account> _pagingSupport;

        public AccountController(IAccountService accountService, IMapper mapper, IPagingSupport<Account> pagingSupport)
        {
            _accountService = accountService;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }


        /// <summary>
        /// Create a new account
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "displayName": "Name of customer",    
        ///         "phone": "Phone of customer",
        ///         "role": "Role of customer",
        ///         "status": "Status of account"
        ///         "isBeautyArtist": "Is Beauty Artist"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new account</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAccount([FromBody] AccountCM accountCM)
        {
            Account account = _mapper.Map<Account>(accountCM);
            Account createAccount = await _accountService.AddAsync(account);
            await _accountService.Save();
            return Created("/account", createAccount);
        }

        /// <summary>
        /// Update account with specified id
        /// </summary>
        /// <param name="id">Account's id</param>
        /// <param name="accountUM">Information applied to updated account</param>
        /// <response code="204">Update account successfully</response>
        /// <response code="400">Account's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutAccount(int id, [FromBody] AccountUM accountUM)
        {
            Account accountUpdated = await _accountService.GetByIdAsync(id);
            try
            {
                accountUpdated.DisplayName = accountUM.DisplayName;
                accountUpdated.Phone = accountUM.Phone;
                accountUpdated.Status = accountUM.Status;

                _accountService.Update(accountUpdated);
                await _accountService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(accountUpdated);
        }

        /// <summary>
        /// Change the status of account to disabled
        /// </summary>
        /// <param name="id">Account's id</param>
        /// <response code="204">Update account's status successfully</response>
        /// <response code="400">Account's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            Account accountSaved = await _accountService.GetByIdAsync(id);
            if (accountSaved == null)
            {
                return BadRequest();
            }

            try
            {
                accountSaved.Status = "INACTIVE";
                _accountService.Update(accountSaved);
                await _accountService.Save();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        /// <summary>
        /// Get a specific account by id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "Id" : "9"
        ///     }
        /// </remarks>
        /// <returns>Return the account with the corresponding id</returns>
        /// <response code="200">Returns the account with the specified id</response>
        /// <response code="404">No account found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAccountById(int id)
        {
            Account account = await _accountService.GetByIdAsync(id);
            AccountVM returnAccount = null;
            if (account != null)
            {
                returnAccount = _mapper.Map<AccountVM>(account);
                return Ok(returnAccount);
            }
            else
            {
                return NotFound(returnAccount);
            }
        }

        /// <summary>
        /// Get all account
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///         
        ///     }
        ///
        /// </remarks>
        /// <returns>All account</returns>
        /// <response code="200">Returns all account</response>
        /// <response code="404">No account found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<AccountVM>> GetAllAccount([FromQuery] int pageSize = 20, int pageIndex = 1)
        {
            IQueryable<Account> accountList = _accountService.GetAll().Where(a => a.Status.Equals("ACTIVE"));
            var pagedModel = _pagingSupport.From(accountList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<AccountVM>();

            return Ok(pagedModel);
        }

    }



}
