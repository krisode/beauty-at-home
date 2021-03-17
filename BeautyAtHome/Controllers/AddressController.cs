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
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Address> _pagingSupport;

        public AddressController(IAddressService addressService, IMapper mapper, IPagingSupport<Address> pagingSupport)
        {
            _addressService = addressService;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }

        /// <summary>
        /// Create a new address
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "accountId": "Id of Account",
        ///         "location": "Address location of the place",    
        ///         "locationName": "The name of the location by account"
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
        public async Task<ActionResult> CreateAddress ([FromBody] AddressCM addressCM)
        {
            Address address = _mapper.Map<Address>(addressCM);
            Address createAddress = await _addressService.AddAsync(address);
            await _addressService.Save();
            return Created("/addresses", createAddress);
        }


        /// <summary>
        /// Update address with specified id
        /// </summary>
        /// <param name="id">Address's id</param>
        /// <param name="addressUM">Information applied to updated address</param>
        /// <response code="204">Update address successfully</response>
        /// <response code="400">Address's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutAddress(int id, [FromBody] AddressUM addressUM)
        {

            Address addressUpdated = await _addressService.GetByIdAsync(id);
            try
            {
                addressUpdated.Location = addressUM.Location;
                addressUpdated.Note = addressUM.Note;
                addressUpdated.LocationName = addressUM.LocationName;

                _addressService.Update(addressUpdated);
                await _addressService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(addressUpdated);
        }



        /// <summary>
        /// Change the status of address to disabled
        /// </summary>
        /// <param name="id">Address's id</param>
        /// <response code="204">Update address's status successfully</response>
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
            Address addressSaved = await _addressService.GetByIdAsync(id);
            if (addressSaved == null)
            {
                return BadRequest();
            }

            try
            {
                _addressService.Delete(addressSaved);
                await _addressService.Save();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }



        /// <summary>
        /// Get a specific address by id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "Id" : "7"
        ///     }
        /// </remarks>
        /// <returns>Return the address with the corresponding id</returns>
        /// <response code="200">Returns the address with the specified id</response>
        /// <response code="404">No address found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAddressById(int id)
        {
            Address address = await _addressService.GetByIdAsync(id);
            AddressVM returnAddress = null;
            if (address != null)
            {
                returnAddress = _mapper.Map<AddressVM>(address);
                return Ok(returnAddress);
            }
            else
            {
                return NotFound(returnAddress);
            }
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
        public ActionResult<IEnumerable<AddressVM>> GetAllAddress([FromQuery] int pageSize = 20, int pageIndex = 1)
        {
            IQueryable<Address> addressList = _addressService.GetAll();
            var pagedModel = _pagingSupport.From(addressList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<AddressVM>();

            return Ok(pagedModel);
        }
    }
}
