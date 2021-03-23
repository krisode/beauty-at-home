﻿using ApplicationCore.Services;
using AutoMapper;
using BeautyAtHome.ExternalService;
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
    [Route("api/v1.0/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IFeedBackService _feedBackService;
        private readonly IImageService _imageService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Account> _pagingSupport;

        public AccountController(IAccountService accountService, IFeedBackService feedBackService, IMapper mapper, IPagingSupport<Account> pagingSupport, IImageService imageService, IUploadFileService uploadFileService)
        {
            _accountService = accountService;
            _feedBackService = feedBackService;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
            _imageService = imageService;
            _uploadFileService = uploadFileService;
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
        /// <response code="404">No accounts found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAccountById(int id, [FromQuery] bool withRateScore)
        {
            IQueryable<Account> accountList = _accountService.GetAll( s => s.Services, s => s.Gallery.Images, _ => _.Addresses);
            Account account = accountList.FirstOrDefault(s => s.Id == id);
            AccountVM returnAccount = null;
            if (account != null)
            {
                returnAccount = _mapper.Map<AccountVM>(account);
                if (withRateScore)
                {
                    var rating = _feedBackService.GetRateScoreByAccount(returnAccount.Id);
                    returnAccount.RateScore = rating[0];
                    returnAccount.TotalFeedback = (int) rating[1];
                }
                return Ok(returnAccount);
            }
            else
            {
                return NotFound(returnAccount);
            }
        }

        /// <summary>
        /// Get all accounts
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
        /// <returns>All accounts</returns>
        /// <response code="200">Returns all accounts</response>
        /// <response code="404">No account found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<AccountVM>> GetAllAccount([FromQuery] AccountSM account, int pageSize, int pageIndex, bool withRateScore)
        {
            try
            {
                IQueryable<Account> accountList = _accountService.GetAll(s => s.Services, s => s.Gallery.Images, _ => _.Addresses);

                if (!string.IsNullOrEmpty(account.Email))
                {
                    accountList = accountList.Where(s => s.Email.Contains(account.Email));
                }
                if (!string.IsNullOrEmpty(account.DisplayName))
                {
                    accountList = accountList.Where(s => s.DisplayName.Contains(account.DisplayName));
                }
                if (!string.IsNullOrEmpty(account.Phone))
                {
                    accountList = accountList.Where(s => s.Phone.Contains(account.Phone));
                }
                if (!string.IsNullOrEmpty(account.Role))
                {
                    accountList = accountList.Where(s => s.Role.Contains(account.Role));
                }
                if (!string.IsNullOrEmpty(account.Status))
                {
                    accountList = accountList.Where(s => s.Status.Contains(account.Status));
                }

                if (account.GalleryId != 0)
                {
                    accountList = accountList.Where(s => s.GalleryId == account.GalleryId);
                }


                if (account.IsBeautyArtist != false)
                {
                    accountList = accountList.Where(s => s.IsBeautyArtist == true);
                }

                if (pageSize == 0)
                {
                    pageSize = 20;
                }

                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }
                

                var pagedModel = _pagingSupport.From(accountList)
                    .GetRange(pageIndex, pageSize, s => s.Id)
                    .Paginate<AccountVM>();
                if (withRateScore)
                {
                    pagedModel.Content = pagedModel.Content.AsEnumerable().Select<AccountVM, AccountVM>(_ => {
                        var rating = _feedBackService.GetRateScoreByAccount(_.Id);
                        _.RateScore = rating[0];
                        _.TotalFeedback = (int) rating[1];
                        return _;
                    }).AsQueryable();
                }


                return Ok(pagedModel);
            }
            catch (Exception e)
            {

                return NotFound(e);
            }
            
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
            try
            {
                await _accountService.AddAsync(account);
                await _accountService.Save();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Account createAccount = await _accountService.AddAsync(account);
            await _accountService.Save();
            return CreatedAtAction("GetAccountById", createAccount);
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
        public async Task<ActionResult> PutAccount([FromForm] AccountUM accountUM)
        {
            /*Account accountUpdated = await _accountService.GetByIdAsync(id);*/
            var accountList = _accountService.GetAll(_ => _.Gallery.Images);
            Account account = accountList.FirstOrDefault(_ => _.Id == accountUM.Id);

            ImageUM imageUM = new ImageUM();
            imageUM.Id = account.Gallery.Images.First().Id;
            imageUM.Description = "Hình " + account.DisplayName;
            imageUM.ImageUrl = await _uploadFileService.UploadFile("123456798", accountUM.File, "service", "service-detail");
            imageUM.GalleryId = (int) account.GalleryId;

            Image image = _mapper.Map<Image>(imageUM);
            _imageService.Update(image);

            try
            {
                account.DisplayName = accountUM.DisplayName;
                account.Phone = accountUM.Phone;
                account.Status = accountUM.Status;

                _accountService.Update(account);
                await _accountService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(account);
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
                accountSaved.Status = Constants.Status.DISABLED;
                _accountService.Update(accountSaved);
                await _accountService.Save();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}
