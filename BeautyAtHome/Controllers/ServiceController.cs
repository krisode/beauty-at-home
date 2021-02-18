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

    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IBeautyServicesService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Service> _pagingSupport;

        public ServiceController(IBeautyServicesService service, IMapper mapper, IPagingSupport<Service> pagingSupport)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }



        /// <summary>
        /// Get a specific service by service id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the service with the corresponding id</returns>
        /// <response code="200">Returns the service with the specified id</response>
        /*/// <response code="404">No services found with the specified id</response>*/
        [HttpGet]
        [Route("api/v1.0/services/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        /*[SwaggerResponseExample(StatusCodes.Status200OK, typeof (ApplicationCore.DTOs.Service))]*/
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ServiceVM>> GetServiceById(int id)
        {

            var service = await _service.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            var rtnService = _mapper.Map<ServiceVM>(service);

            /*try
            {
                bool result = await _service.Save();
                if (!result)
                {
                    throw new DbUpdateConcurrencyException();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }*/

            return Ok(rtnService);
        }

        /// <summary>
        /// Get all services 
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
        /// <returns>All services</returns>
        /// <response code="200">Returns all services</response>
        /// <response code="404">No services found</response>
        [HttpGet]
        [Route("api/v1.0/services")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceVM>>> GetService([FromQuery] ServiceSM serviceModel, int pageSize, int pageIndex)
        {
            var serviceList = _service.GetAll(s => s.ServiceType, s => s.Gallery, s => s.Account);

            var pagedModel = _pagingSupport.From(serviceList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<ServicePagingSM>();

            return Ok(pagedModel);
        }

        /// <summary>
        /// Count all services 
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
        /// <returns>Number of services in the whole system</returns>
        /// <response code="200">Returns amount of services</response>
        /// <response code="404">No services found</response>
        [HttpGet]
        [Route("api/v1.0/services/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CountAll([FromQuery] ServiceSM serviceModel)
        {
            var serviceList = _service.GetAll(s => s.ServiceType, s => s.Gallery, s => s.Account).Count();
            return Ok(serviceList);
        }

        /// <summary>
        /// Create a new service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "description": "TODO something",
        ///         "serviceName": "Service name",     
        ///         "price": 50,
        ///         "estimateTime": 30,
        ///         "salonId": 1,
        ///         "categoryId": 1,
        ///         "isServiceCombo": "True",
        ///         "galleryId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new service</response>
        /// <response code="400">Account's id or service type's id or gallery's id does not exist</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Route("api/v1.0/services")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceCM>> CreateService([FromBody] ServiceCM serviceModel)
        {
            //TODO: Implements AccountService.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements CategoryId.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements GaleryId.GetById(int id) does not exist, return BadRequest()

            DateTime crtDate = DateTime.Now;
            DateTime updDate = DateTime.Now;
            string status = Constants.Status.ENABLED;
            var crtService = _mapper.Map<Service>(serviceModel);

            try
            {
                crtService.CreatedDate = crtDate;
                crtService.UpdatedDate = updDate;
                crtService.Status = status;

                await _service.AddAsync(crtService);
                await _service.Save();
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetServiceById", new { id = crtService.Id }, crtService);
        }

        /// <summary>
        /// Update service with specified id
        /// </summary>
        /// <param name="id">Service's id</param>
        /// <param name="service">Information applied to updated service</param>
        /// <response code="204">Update service successfully</response>
        /// <response code="400">Service's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("api/v1.0/services/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutService(int id, [FromBody] ServiceUM service)
        {
            Service serviceSaved = await _service.GetByIdAsync(id);
            if (serviceSaved == null || id != service.Id)
            {
                return BadRequest();
            }

            serviceSaved.UpdatedDate = DateTime.Now;
            try
            {
                _service.Update(serviceSaved);
                await _service.Save();
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        /// <summary>
        /// Change the status of service to disabled
        /// </summary>
        /// <param name="id">Service's id</param>
        /// <response code="204">Update service's status successfully</response>
        /// <response code="400">Service's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("api/v1.0/services/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteService(int id)
        {
            Service serviceSaved = await _service.GetByIdAsync(id);
            if (serviceSaved == null)
            {
                return BadRequest();
            }

            serviceSaved.UpdatedDate = DateTime.Now;
            serviceSaved.Status = Constants.Status.DISABLED;

            try
            {
                _service.Update(serviceSaved);
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
