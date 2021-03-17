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
    [Route("api/v1.0/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Address> _pagingSupport;

        public AddressController(IAddressService service, IMapper mapper, IPagingSupport<Address> pagingSupport)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }

        /// <summary>
        /// Get a specific address type by address id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the address with the corresponding id</returns>
        /// <response code="200">Returns the address type with the specified id</response>
        /// <response code="404">No addresses found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressVM>> GetAddressById(int id)
        {
            Address addressSearch = await _service.GetByIdAsync(id);
            if (addressSearch == null)
            {
                return NotFound(addressSearch);   
            }
            var rtnAddress = _mapper.Map<AddressVM>(addressSearch);
            return Ok(rtnAddress);
        }

        /// <summary>
        /// Get all addresses
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
        /// <returns>All addresses</returns>
        /// <response code="200">Returns all addresses</response>
        /// <response code="404">No addresses found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<AddressVM>> GetAllAddresses([FromQuery] AddressSM model, int pageSize, int pageIndex)
        {
            IQueryable<Address> addressList = _service.GetAll();

            if (!string.IsNullOrEmpty(model.Location))
            {
                addressList = addressList.Where(s => s.Location.Contains(model.Location));
            }

            if (model.AccountId != 0)
            {
                addressList = addressList.Where(s => s.AccountId == model.AccountId);
            }

            if (!string.IsNullOrEmpty(model.Note))
            {
                addressList = addressList.Where(s => s.Note.Contains(model.Note));
            }

            if (!string.IsNullOrEmpty(model.LocationName))
            {
                addressList = addressList.Where(s => s.LocationName.Contains(model.LocationName));
            }

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(addressList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<AddressVM>();

            return Ok(pagedModel);
        }

        /// <summary>
        /// Create a new address
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "location": "Address location",    
        ///         "accountId": "AccountId that uses this address",    
        ///         "note": "Note about address",    
        ///         "locationName": "Name of address location",    
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new address</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressCM>> CreateAddress([FromBody] AddressCM model)
        {
            Address crtAddress = _mapper.Map<Address>(model);

            try
            {
                await _service.AddAsync(crtAddress);
                await _service.Save();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetAddressById", new { id = crtAddress.Id }, crtAddress);
        }

        /// <summary>
        /// Update address with specified id
        /// </summary>
        /// <param name="id">Address's id</param>
        /// <param name="model">Information applied to updated address</param>
        /// <response code="204">Update address successfully</response>
        /// <response code="400">Address's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutAddress(int id, [FromBody] AddressUM model)
        {
            // TODO: Implements return BadRequest if AccountId does not exist

            Address addressUpdated = await _service.GetByIdAsync(id);
            if (addressUpdated == null || id != model.Id)
            {
                return BadRequest();
            }

            try
            {
                addressUpdated.Id = model.Id;
                addressUpdated.Location = model.Location;
                addressUpdated.AccountId = model.AccountId;
                addressUpdated.Note = model.Note;
                addressUpdated.LocationName = model.LocationName;
                _service.Update(addressUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="id">Address's id</param>
        /// <response code="204">Delete address successfully</response>
        /// <response code="400">Address's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            Address dltAddress = await _service.GetByIdAsync(id);
            if (dltAddress == null)
            {
                return BadRequest();
            }

            try
            {
                _service.Delete(dltAddress);
                await _service.Save();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}
