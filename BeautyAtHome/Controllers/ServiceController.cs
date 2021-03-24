using ApplicationCore.Services;
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

    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IBeautyServicesService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Service> _pagingSupport;
        private readonly IFeedBackService _feedbackService;
        private readonly IGalleryService _galleryService;
        private readonly IImageService _imageService;
        private readonly IUploadFileService _uploadFileService;

        public ServiceController(IBeautyServicesService service, IMapper mapper, IPagingSupport<Service> pagingSupport, IFeedBackService feedbackService, IGalleryService galleryService, IImageService imageService, IUploadFileService uploadFileService)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
            _feedbackService = feedbackService;
            _galleryService = galleryService;
            _imageService = imageService;
            _uploadFileService = uploadFileService;
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

        public ActionResult<ServicePagingSM> GetServiceById(int id, [FromQuery] bool withRateScore)
        {
            IQueryable<Service> serviceList = _service.GetAll(s => s.ServiceType, s => s.Gallery, s => s.Account, s => s.Gallery.Images, s => s.BookingDetails);
            Service serviceSearch = serviceList.FirstOrDefault(s => s.Id == id);
            ServicePagingSM rtnService = null;
            
            if (serviceSearch != null)
            {
                rtnService = _mapper.Map<ServicePagingSM>(serviceSearch);
                if (withRateScore)
                {
                    var rating = _feedbackService.GetRateScoreByService(rtnService.Id);
                    rtnService.RateScore = rating[0];
                    rtnService.TotalFeedback = (int)rating[1];
                }
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
        public ActionResult<IEnumerable<ServiceVM>> GetAllService([FromQuery] ServiceSM model, int pageSize, int pageIndex, bool withRateScore, string searchQuery)
        {
            IQueryable<Service> serviceList = _service.GetAll(s => s.ServiceType, s => s.Gallery.Images, s => s.Account.Gallery.Images);
            IQueryable<Service> serviceBasedOnSearchQuery;
            IQueryable<Service> result = null;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var serviceByServiceName = serviceList.Where(_ => _.ServiceName.Contains(searchQuery));
                var serviceByProviderName = serviceList.Where(_ => _.Account.DisplayName.Contains(searchQuery));
                var serviceByServiceTypeName = serviceList.Where(_ => _.ServiceType.Name.Contains(searchQuery));

                serviceBasedOnSearchQuery = Queryable.Concat(serviceByServiceName, serviceByProviderName);
                result = Queryable.Concat(serviceBasedOnSearchQuery, serviceByServiceTypeName);
            }
            


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
            
            if (model.AccountId != 0)
            {
                serviceList = serviceList.Where(s => s.AccountId == model.AccountId);
            }

            if (model.ServiceTypeId != 0)
            {
                serviceList = serviceList.Where(s => s.ServiceTypeId == model.ServiceTypeId);
            }

            if (model.Status == true)
            {
                serviceList = serviceList.Where(s => s.Status.Equals("Active"));
            }

            if (model.GalleryId != 0)
            {
                serviceList = serviceList.Where(s => s.GalleryId == model.GalleryId);
            }

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            Paged<ServicePagingSM> pagedModel = null;
            if(string.IsNullOrEmpty(searchQuery))
            {
                pagedModel = _pagingSupport.From(serviceList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<ServicePagingSM>();
            } else
            {
                pagedModel = _pagingSupport.From(result.Distinct())
                .GetRange(pageIndex, pageSize, s => s.UpdatedDate)
                .Paginate<ServicePagingSM>();
            }

            if (withRateScore)
            {
                pagedModel.Content = pagedModel.Content.AsEnumerable().Select<ServicePagingSM, ServicePagingSM>(_ => {
                    var rating = _feedbackService.GetRateScoreByService(_.Id);
                    _.RateScore = rating[0];
                    _.TotalFeedback = (int) rating[1];
                    return _;
                }).AsQueryable();
            }

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
        public async Task<ActionResult<ServiceCM>> CreateService([FromForm] ServiceCM serviceModel)
        {
            //TODO: Implements AccountService.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements ServiceTypeId.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements GaleryId.GetById(int id) does not exist, return BadRequest()

            
            DateTime crtDate = DateTime.Now;
            DateTime updDate = DateTime.Now;
            string status = Constants.Status.ACTIVE;
            
            // Initialize gallery
            GalleryCM galleryCM = new GalleryCM();
            galleryCM.Name = "Hình " + serviceModel.ServiceName.ToLower();
            galleryCM.Description = "Hình " + serviceModel.ServiceName.ToLower(); ;

            // Add new gallery
            Gallery gallery = _mapper.Map<Gallery>(galleryCM);
            Gallery crtGallery = await _galleryService.AddAsync(gallery);
            await _galleryService.Save();

            ImageCM imageCM = new ImageCM();
            imageCM.Description = "Hình " + serviceModel.ServiceName;
            imageCM.GalleryId = crtGallery.Id;
            imageCM.ImageUrl = await _uploadFileService.UploadFile("123456798", serviceModel.File, "service", "service-detail");

            Image image = _mapper.Map<Image>(imageCM);
            await _imageService.AddAsync(image);

            Service crtService = _mapper.Map<Service>(serviceModel);

            try
            {
                crtService.CreatedDate = crtDate;
                crtService.UpdatedDate = updDate;
                crtService.Status = status;
                crtService.AccountId = serviceModel.AccountId;
                crtService.ServiceTypeId = serviceModel.ServiceTypeId;
                crtService.GalleryId = crtGallery.Id;

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
        public async Task<ActionResult> PutService([FromForm] ServiceUM service)
        {
            var serviceList = _service.GetAll(_ => _.Gallery.Images);

            Service serviceUpdated = serviceList.FirstOrDefault(_ => _.Id == service.Id);

            Image imageUpdated = serviceUpdated.Gallery.Images.First();
            imageUpdated.Description = "Hình " + service.ServiceName.ToLower();
            imageUpdated.ImageUrl = await _uploadFileService.UploadFile("123456798", service.File, "service", "service-detail");
            imageUpdated.GalleryId = (int) service.GalleryId;

            _imageService.Update(imageUpdated);
            await _imageService.Save();

            try
            {
                serviceUpdated.Id = service.Id;
                serviceUpdated.Description = service.Description;
                serviceUpdated.ServiceName = service.ServiceName;
                serviceUpdated.Summary = service.Summary;
                serviceUpdated.Price = service.Price;
                serviceUpdated.EstimateTime = service.EstimateTime;
                serviceUpdated.Status = service.Status;
                serviceUpdated.UpdatedDate = DateTime.Now;
                serviceUpdated.AccountId = (int) service.AccountId;
                serviceUpdated.ServiceTypeId = (int) service.ServiceTypeId;
                serviceUpdated.GalleryId = (int) service.GalleryId;
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
