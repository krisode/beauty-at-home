using ApplicationCore.Services;
using AutoMapper;
using BeautyAtHome.Utils;
using BeautyAtHome.ViewModels;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public ServiceController(IBeautyServicesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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
        public ActionResult<ServiceVM> GetServiceById(int id)
        {

            var service = _service.GetById(id);

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
        public async Task<ActionResult<IEnumerable<ServiceVM>>> GetService([FromQuery] ServiceSM serviceModel, int pageSize, int pageIndex, bool sort)
        {
            var rtnList = _service.GetEnumAll();

            var testList = _service.GetQueryList(s => s.ServiceName.Contains("s"));
            
            if (serviceModel.Id.Length != 0)
            {
                for (int i = 0; i < serviceModel.Id.Length; i++)
                {
                    rtnList.Where(s => s.Id == serviceModel.Id[i]);
                }
            }

            if (!string.IsNullOrEmpty(serviceModel.ServiceName))
            {
                rtnList = rtnList.Where(s => s.ServiceName.Contains(serviceModel.ServiceName));
            } 

            if (serviceModel.CreatedAtMin != null)
            {
                rtnList = rtnList.Where(s => s.CreatedDate >= serviceModel.CreatedAtMin);
            }
            
            if (serviceModel.CreatedAtMax != null)
            {
                rtnList = rtnList.Where(s => s.CreatedDate <= serviceModel.CreatedAtMax);
            }
            
            if (serviceModel.UpdatedAtMin != null)
            {
                rtnList = rtnList.Where(s => s.UpdatedDate >= serviceModel.UpdatedAtMin);
            }
            
            if (serviceModel.UpdatedAtMax != null)
            {
                rtnList = rtnList.Where(s => s.UpdatedDate <= serviceModel.UpdatedAtMax);
            }

            if (serviceModel.LowerPrice > 0)
            {
                rtnList = rtnList.Where(s => s.Price >= serviceModel.LowerPrice);
            }

            if (serviceModel.UpperPrice > 0)
            {
                rtnList = rtnList.Where(s => s.Price <= serviceModel.UpperPrice);
            }

            if (serviceModel.LowerTime > 0)
            {
                rtnList = rtnList.Where(s => s.EstimateTime >= serviceModel.LowerTime);
            }

            if (serviceModel.UpperTime > 0)
            {
                rtnList = rtnList.Where(s => s.EstimateTime <= serviceModel.LowerTime);
            }

            
            int count = rtnList.Count();

            int totalPages = (int)Math.Ceiling(count / (double) pageSize);

            var items = rtnList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var previousPage = pageIndex > 1 ? "Yes" : "No";

            var nextPage = pageIndex < totalPages ? "Yes" : "No";

            var paginationMetadata = new
            {
                totalCount = count,
                pageSize = pageSize,
                currentPage = pageIndex,
                totalPages = totalPages,
                previousPage,
                nextPage
            };

            Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(rtnList);
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
        public async Task<ActionResult<int>> CountAll()
        {
            var lst = _service.GetEnumAll().Count();
            await _service.Save();
            return Ok(lst);
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

                _service.Add(crtService);
                await _service.Save();
                
            }
            catch (Exception)
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
            if (_service.GetById(id) == null || id != service.Id)
            {
                return BadRequest();
            }

            DateTime updDate = DateTime.Now;
            var updService = _mapper.Map<Service>(service);
            updService.UpdatedDate = updDate;
            try
            {
                _service.Update(updService);
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
            if (_service.GetById(id) == null)
            {
                return BadRequest();
            }

            DateTime updTime = DateTime.Now;
            string status = Constants.Status.DISABLED;

            var dltService = _service.GetById(id);
            dltService.UpdatedDate = updTime;
            dltService.Status = status;

            try
            {
                _service.Update(dltService);
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
