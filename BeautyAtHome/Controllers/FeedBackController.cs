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
    [Route("api/v1.0/feedbacks")]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<FeedBack> _pagingSupport;
        private readonly IGalleryService _galleryService;
        private readonly IImageService _imageService;
        private readonly IUploadFileService _uploadFileService;

        public FeedBackController(IFeedBackService service, IMapper mapper, IPagingSupport<FeedBack> pagingSupport, IGalleryService galleryService, IImageService imageService, IUploadFileService uploadFileService)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
            _galleryService = galleryService;
            _imageService = imageService;
            _uploadFileService = uploadFileService;
        }

        /// <summary>
        /// Get a specific feedback by feedback id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the feedback with the corresponding id</returns>
        /// <response code="200">Returns the feedback with the specified id</response>
        /// <response code="404">No feedbacks found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        /*[SwaggerResponseExample(StatusCodes.Status200OK, typeof (ApplicationCore.DTOs.FeedBack))]*/
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<FeedBackVM>> GetFeedBackById(int id)
        {

            var feedback = await _service.GetByIdAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            var rtnFeedBack = _mapper.Map<FeedBackVM>(feedback);

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

            return Ok(rtnFeedBack);
        }

        /// <summary>
        /// Create a new feedback
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "rateScore": "TODO something",
        ///         "bookingDetailId": "10",     
        ///         "galleryId": 50,
        ///         "FeedbackContent": "Nice",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new feedback</response>
        /// <response code="400">BookingDetail type's id or gallery's id does not exist</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FeedBackCM>> CreateFeedback([FromForm] FeedBackCM feedbackModel)
        {
            //TODO: Implements BookingDetail.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements GaleryId.GetById(int id) does not exist, return BadRequest()

            if (feedbackModel.File != null)
            {
                GalleryCM galleryCM = new GalleryCM();
                galleryCM.Name = "Hình feedback cho dịch vụ trong đơn " + feedbackModel.BookingDetailId;
                galleryCM.Description = "Hình feedback cho dịch vụ trong đơn " + feedbackModel.BookingDetailId;

                Gallery gallery = _mapper.Map<Gallery>(galleryCM);
                Gallery crtGallery = await _galleryService.AddAsync(gallery);
                await _galleryService.Save();
                
                feedbackModel.GalleryId = crtGallery.Id;

                ImageCM imageCM = new ImageCM();
                imageCM.Description = "Hình feedback trong đơn " + feedbackModel.BookingDetailId;
                imageCM.GalleryId = crtGallery.Id;
                imageCM.ImageUrl = await _uploadFileService.UploadFile("123456798", feedbackModel.File, "service", "service-detail");

                Image image = _mapper.Map<Image>(imageCM);
                await _imageService.AddAsync(image);
            }
            
            FeedBack crtFeedback = _mapper.Map<FeedBack>(feedbackModel);

            try
            {
                crtFeedback.CreateDate = DateTime.Now;

                await _service.AddAsync(crtFeedback);
                await _service.Save();

            }
            catch (Exception e)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError);
                NotFound(e);
            }

            return CreatedAtAction("GetFeedBackById", new { id = crtFeedback.Id }, crtFeedback);
        }

        /// <summary>
        /// Update feedback with specified id
        /// </summary>
        /// <param name="id">FeedBack's id</param>
        /// <param name="feedBack">Information applied to updated feedback</param>
        /// <response code="204">Update feedback successfully</response>
        /// <response code="400">BookingDetailId's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutFeedBack(int id, [FromBody] FeedBackUM feedBack)
        {
            FeedBack feedBachUpdated = await _service.GetByIdAsync(id);
            if (feedBachUpdated == null || id != feedBack.Id)
            {
                return BadRequest();
            }

            int galleryId = 1;
            int bookingDetailID = 1;

            try
            {
                feedBachUpdated.Id = feedBack.Id;
                /*feedBachUpdated.RateScore = feedBack.RateScore;*/
                feedBachUpdated.BookingDetailId = bookingDetailID;
                feedBachUpdated.GalleryId = galleryId;
                feedBachUpdated.FeedbackContent = feedBack.FeedbackContent;
                feedBachUpdated.CreateDate = DateTime.Now;

                _service.Update(feedBachUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        /// <summary>
        /// Get all feedbacks
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
        /// <returns>All feedbacks</returns>
        /// <response code="200">Returns all feedbacks</response>
        /// <response code="404">No feedbacks found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FeedBackVM>> GetAllFeedBack([FromQuery] FeedBackSM model, int pageSize, int pageIndex)
        {
            IQueryable<FeedBack> feedBackList = _service.GetAll(s => s.BookingDetail.Booking.CustomerAccount.Gallery.Images, s => s.Gallery.Images);

            if (!string.IsNullOrEmpty(model.FeedbackContent))
            {
                feedBackList = feedBackList.Where(s => s.FeedbackContent.Contains(s.FeedbackContent));
            }


            if (model.CreateDateAtMin.HasValue)
            {
                feedBackList = feedBackList.Where(s => s.CreateDate >= model.CreateDateAtMin);
            }

            if (model.CreateDateAtMax.HasValue)
            {
                feedBackList = feedBackList.Where(s => s.CreateDate <= model.CreateDateAtMax);
            }

            if (model.RateScoreMin > 0)
            {
                feedBackList = feedBackList.Where(s => s.RateScore >= model.RateScoreMin);
            }
            if (model.RateScoreMax > 0)
            {
                feedBackList = feedBackList.Where(s => s.RateScore >= model.RateScoreMax);
            }

            if (model.BookingDetailId > 0)
            {
                feedBackList = feedBackList.Where(s => s.BookingDetailId == model.BookingDetailId);
            }

            if (model.GalleryId > 0)
            {
                feedBackList = feedBackList.Where(s => s.GalleryId == model.GalleryId);
            }

            if (model.ServiceId > 0)
            {
                feedBackList = feedBackList.Where(s => s.BookingDetail.ServiceId == model.ServiceId);                
            }

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            if (model.WorkerId > 0)
            {
                feedBackList = feedBackList.Where(_ => _.BookingDetail.Booking.BeautyArtistAccountId == model.WorkerId);
            }

            var pagedModel = _pagingSupport.From(feedBackList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<FeedBackVM>();

            return Ok(pagedModel);
        }

    }
}
