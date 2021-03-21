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
        private readonly IFeedBackService _feedbackService;

        public ServiceController(IBeautyServicesService service, IMapper mapper, IPagingSupport<Service> pagingSupport, IFeedBackService feedbackService)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
            _feedbackService = feedbackService;
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
        /// <response code="404">No services found with the specified id</response>
        [HttpGet]
        [Route("api/v1.0/services/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ServicePagingSM> GetServiceById(int id)
        {
            IQueryable<Service> serviceList = _service.GetAll(s => s.ServiceType, s => s.Gallery, s => s.Account, s => s.Gallery.Images, s => s.BookingDetails);
            Service serviceSearch = serviceList.FirstOrDefault(s => s.Id == id);
            ServicePagingSM rtnService = null;
            
            if (serviceSearch != null)
            {
                rtnService = _mapper.Map<ServicePagingSM>(serviceSearch);
                return Ok(rtnService);
            } else
            {
                return NotFound(rtnService);
            } 
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
        public ActionResult<IEnumerable<ServiceVM>> GetAllService([FromQuery] ServiceSM model, int pageSize, int pageIndex)
        {
            IQueryable<Service> serviceList = _service.GetAll(s => s.ServiceType, s => s.Gallery, s => s.Account);
            if (!string.IsNullOrEmpty(model.Description))
            {
                serviceList = serviceList.Where(s => s.Description.Contains(model.Description));
            }

            if (!string.IsNullOrEmpty(model.ServiceName))
            {
                serviceList = serviceList.Where(s => s.ServiceName.Contains(model.ServiceName));
            }

            if (model.CreatedAtMin.HasValue)
            {
                serviceList = serviceList.Where(s => s.CreatedDate >= model.CreatedAtMin);
            }

            if (model.CreatedAtMax.HasValue)
            {
                serviceList = serviceList.Where(s => s.CreatedDate <= model.CreatedAtMax);
            }

            if (model.UpdatedAtMin.HasValue)
            {
                serviceList = serviceList.Where(s => s.UpdatedDate >= model.UpdatedAtMin);
            }

            if (model.UpdatedAtMax.HasValue)
            {
                serviceList = serviceList.Where(s => s.UpdatedDate <= model.UpdatedAtMax);
            }

            if (model.LowerPrice > 0)
            {
                serviceList = serviceList.Where(s => s.Price >= model.LowerPrice);
            }

            if (model.UpperPrice > 0)
            {
                serviceList = serviceList.Where(s => s.Price <= model.UpperPrice);
            }

            if (model.LowerTime > 0)
            {
                serviceList = serviceList.Where(s => s.EstimateTime >= model.LowerTime);
            }

            if (model.UpperTime > 0)
            {
                serviceList = serviceList.Where(s => s.EstimateTime <= model.UpperTime);
            }
            
            if (model.Status == true)
            {
                serviceList = serviceList.Where(s => s.Status.Equals("Active"));
            }

            /*if (model.IsServiceCombo == true)
            {
                serviceList = serviceList.Where(s => s.IsServiceCombo == true);
            }*/

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(serviceList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<ServicePagingSM>();

            return Ok(pagedModel);
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
            //TODO: Implements ServiceTypeId.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements GaleryId.GetById(int id) does not exist, return BadRequest()

            DateTime crtDate = DateTime.Now;
            DateTime updDate = DateTime.Now;
            string status = Constants.Status.ACTIVE;
            int accountId = 3;
            int serviceTypeId = 1;
            int galleryId = 1;


            Service crtService = _mapper.Map<Service>(serviceModel);

            try
            {
                crtService.CreatedDate = crtDate;
                crtService.UpdatedDate = updDate;
                crtService.Status = status;
                crtService.AccountId = accountId;
                crtService.ServiceTypeId = serviceTypeId;
                crtService.GalleryId = galleryId;

                await _service.AddAsync(crtService);
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
            Service serviceUpdated = await _service.GetByIdAsync(id);
            if (serviceUpdated == null || id != service.Id)
            {
                return BadRequest();
            }

            int accountId = 3;
            int serviceTypeId = 1;
            int galleryId = 1;
            
            try
            {
                serviceUpdated.Id = service.Id;
                serviceUpdated.Description = service.Description;
                serviceUpdated.ServiceName = service.ServiceName;
                serviceUpdated.Price = service.Price;
                serviceUpdated.EstimateTime = service.EstimateTime;
                serviceUpdated.Status = service.Status;
                serviceUpdated.UpdatedDate = DateTime.Now;
                serviceUpdated.AccountId = accountId;
                serviceUpdated.ServiceTypeId = serviceTypeId;
                serviceUpdated.GalleryId = galleryId;
                _service.Update(serviceUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e); 
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
