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
        public ActionResult<ServiceViewModelVM> GetServiceById(int id)
        {

            var service = _service.GetById(id);

            if (service == null)
            {
                return NotFound();
            }

            var rtnService = _mapper.Map<ServiceViewModelVM>(service);

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
        public async Task<ActionResult<IEnumerable<ServiceViewModelVM>>> GetService([FromQuery] ServiceViewModelSM serviceModel, int pageSize, int pageIndex, bool sort)
        {
            var rtnList = _service.GetEnumAll();


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

            IQueryable<Service> queryList = null;

            PagingSupport<Service> pagingList = new PagingSupport<Service>(queryList);
            PagingViewModel<Service> pagingViewModel = pagingList
                .GetRange(pageIndex, pageSize, s => s.Id)
                .ToPagingViewModel();

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

        /*/// <summary>
        /// Create a new service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "id": 1,
        ///         "description": "TODO something",
        ///         "serviceName": "Service name",
        ///         "createdDate": "2021-01-22 00:00:00",
        ///         "updatedDate": "2021-01-22 00:00:00",
        ///         "price": 50,
        ///         "estimateTime": 30,
        ///         "salonId": 1,
        ///         "categoryId": 1,
        ///         "status": "Active"
        ///         "isServiceCombo": "True",
        ///         "galleryId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new service</response>
        [HttpPost]
        [Route("api/v1.0/services")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        *//*[ProducesResponseType(StatusCodes.)]*//*
        public async Task<ActionResult<ServiceViewModelCM>> CreateService([FromBody] ServiceViewModelCM serviceModel)
        {

            *//*if (ServiceExists(service.Id) != null)
            {
                return BadRequest(service);
            }*/

        /*_service.Add(service);*/

        /*if ()

        Service service = _mapper.Map<Service>(serviceModel);*//*


        DateTime crtDate = DateTime.Now;


        await _service.Save();
        return CreatedAtAction("GetServiceById", new { id = service.Id }, service);*//*
    }*/

        /*/// <summary>
        /// Update service with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        [HttpPut]
        [Route("api/v1.0/services/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> PutService(long id, [FromBody] Service service)
        {
            var dltService = ServiceExists(id);
            if (dltService == null && dltService.Id != service.Id)
            {
                return BadRequest(service);
            }

            _service.Update(service);
            await _service.Save();
            return NoContent();
        }

        /// <summary>
        /// Delete a service by specified id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("api/v1.0/services/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteService(long id)
        {
            if (ServiceExists(id) == null)
            {
                return NotFound();
            }

            _service.Delete(s => s.Id == id);
            await _service.Save();
            return NoContent();
        }*/


        /*private Service ServiceExists(long id)
        {
            var service = _service.GetById(id);
            return service != null ? service : null; 
        }*/


    }
}
