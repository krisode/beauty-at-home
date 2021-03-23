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
    [Route("api/v1.0/service-types")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<ServiceType> _pagingSupport;

        public ServiceTypeController(IServiceTypeService service, IMapper mapper, IPagingSupport<ServiceType> pagingSupport)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }


        /// <summary>
        /// Get a specific service type by service type id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the service type with the corresponding id</returns>
        /// <response code="200">Returns the service type with the specified id</response>
        /// <response code="404">No service types found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ServiceTypeVM> GetServiceTypeById(int id)
        {
            IQueryable<ServiceType> serviceTypeList = _service.GetAll(s => s.Services);
            ServiceType serviceTypeSearch = serviceTypeList.FirstOrDefault(s => s.Id == id);
            ServiceTypeVM rtnGallery = null;
            if (serviceTypeSearch != null)
            {
                rtnGallery = _mapper.Map<ServiceTypeVM>(serviceTypeSearch);
                return Ok(rtnGallery);
            }
            else
            {
                return NotFound(rtnGallery);
            }
        }

        /// <summary>
        /// Get all service types
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
        /// <returns>All service types</returns>
        /// <response code="200">Returns all service types</response>
        /// <response code="404">No service types found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ServiceTypeVM>> GetAllServiceTypes([FromQuery] ServiceTypeSM model, int pageSize, int pageIndex)
        {
            IQueryable<ServiceType> serviceTypeList = _service.GetAll(s => s.Services);

            if (!string.IsNullOrEmpty(model.Name))
            {
                serviceTypeList = serviceTypeList.Where(s => s.Name.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                serviceTypeList = serviceTypeList.Where(s => s.Description.Contains(model.Description));
            }

            /*if (model.GalleryId != 0)
            {
                // Check whether GalleryId exists, return Bad Request if does not exist

                if (_galleryService.GetByIdAsync(model.GalleryId) != null)
                {
                    imageList = imageList.Where(s => s.GalleryId == model.GalleryId);
                }
                else
                {
                    return BadRequest();
                }
            }*/

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(serviceTypeList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<ServiceTypeVM>();

            return Ok(pagedModel);
        }

        /// <summary>
        /// Create a new service type
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "name": "Service type's name",    
        ///         "description": "Service type's description",    
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new service type</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceTypeCM>> CreateServiceType([FromBody] ServiceTypeCM serviceTypeModel)
        {
            /*DateTime crtDate = DateTime.Now;
            DateTime updDate = DateTime.Now;*/

            ServiceType crtServiceType = _mapper.Map<ServiceType>(serviceTypeModel);

            try
            {
                /*crtService.CreatedDate = crtDate;
                crtService.UpdatedDate = updDate;*/

                await _service.AddAsync(crtServiceType);
                await _service.Save();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetServiceTypeById", new { id = crtServiceType.Id }, crtServiceType);
        }


        /// <summary>
        /// Update service type with specified id
        /// </summary>
        /// <param name="id">Service type's id</param>
        /// <param name="serviceType">Information applied to updated service type</param>
        /// <response code="204">Update service type successfully</response>
        /// <response code="400">Service type's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutServiceType(int id, [FromBody] ServiceTypeUM serviceType)
        {
            ServiceType serviceTypeUpdated = await _service.GetByIdAsync(id);
            if (serviceTypeUpdated == null || id != serviceType.Id)
            {
                return BadRequest();
            }

            try
            {
                _service.Update(serviceTypeUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete service type
        /// </summary>
        /// <param name="id">Service type's id</param>
        /// <response code="204">Delete service type successfully</response>
        /// <response code="400">Service type's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            ServiceType dltServiceType = await _service.GetByIdAsync(id);
            if (dltServiceType == null)
            {
                return BadRequest();
            }

            try
            {
                _service.Delete(dltServiceType);
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
