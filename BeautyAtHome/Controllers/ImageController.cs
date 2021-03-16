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
    [Route("api/v1.0/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _service;
        private readonly IGalleryService _galleryService;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Image> _pagingSupport;

        public ImageController(IImageService service, IGalleryService galleryService, IMapper mapper, IPagingSupport<Image> pagingSupport)
        {
            _service = service;
            _galleryService = galleryService;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }

        /// <summary>
        /// Get a specific image by image id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the image with the corresponding id</returns>
        /// <response code="200">Returns the image with the specified id</response>
        /// <response code="404">No images found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ImageVM> GetImageById(int id)
        {
            IQueryable<Image> imageList = _service.GetAll(s => s.Gallery);
            Image imageSearch = imageList.FirstOrDefault(s => s.Id == id);
            ImageVM rtnGallery = null;
            if (imageSearch != null)
            {
                rtnGallery = _mapper.Map<ImageVM>(imageSearch);
                return Ok(rtnGallery);
            }
            else
            {
                return NotFound(rtnGallery);
            }
        }

        /// <summary>
        /// Get all images
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
        /// <returns>All images</returns>
        /// <response code="200">Returns all images</response>
        /// <response code="400">GalleryId does not exist</response>
        /// <response code="404">No images found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ImageVM>> GetAllGallery([FromQuery] ImageSM model, int pageSize, int pageIndex)
        {
            IQueryable<Image> imageList = _service.GetAll(s => s.Gallery);

            if (!string.IsNullOrEmpty(model.ImageUrl))
            {
                imageList = imageList.Where(s => s.ImageUrl.Contains(model.ImageUrl));
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                imageList = imageList.Where(s => s.Description.Contains(model.Description));
            }

            if (model.GalleryId != 0)
            {
                // Check whether GalleryId exists, return Bad Request if does not exist

                if (_galleryService.GetByIdAsync(model.GalleryId) != null)
                {
                    imageList = imageList.Where(s => s.GalleryId == model.GalleryId);
                } else
                {
                    return BadRequest();
                }                   
            }

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(imageList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<ImageVM>();

            return Ok(pagedModel);
        }

        /// <summary>
        /// Create a new image
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "imageUrl": "Image's url",    
        ///         "description": "Image's description",    
        ///         "galleryId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new image</response>
        /// <response code="400">GalleryId does not exist</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ImageCM>> CreateImage([FromBody] ImageCM imageModel)
        {
            /*DateTime crtDate = DateTime.Now;
            DateTime updDate = DateTime.Now;*/

            Image crtImage = _mapper.Map<Image>(imageModel);

            try
            {
                /*crtService.CreatedDate = crtDate;
                crtService.UpdatedDate = updDate;*/

                await _service.AddAsync(crtImage);
                await _service.Save();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetImageById", new { id = crtImage.Id }, crtImage);
        }

        /// <summary>
        /// Update image with specified id
        /// </summary>
        /// <param name="id">Image's id</param>
        /// <param name="image">Information applied to updated image</param>
        /// <response code="204">Update image successfully</response>
        /// <response code="400">Image's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutImage(int id, [FromBody] ImageUM image)
        {
            Image imageUpdated = await _service.GetByIdAsync(id);
            if (imageUpdated == null || id != image.Id)
            {
                return BadRequest();
            }

            /*int accountId = 3;
            int serviceTypeId = 1;
            int galleryId = 1;*/

            try
            {
                imageUpdated.Id = image.Id;
                imageUpdated.ImageUrl = image.ImageUrl;
                imageUpdated.Description = image.Description;
                imageUpdated.GalleryId = image.GalleryId;
                _service.Update(imageUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="id">Image's id</param>
        /// <response code="204">Delete imgage successfully</response>
        /// <response code="400">Image's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            Image dltImage = await _service.GetByIdAsync(id);
            if (dltImage == null)
            {
                return BadRequest();
            }

            /*gallerySaved.UpdatedDate = DateTime.Now;
            gallerySaved.Status = Constants.Status.DISABLED;*/

            try
            {
                _service.Delete(dltImage);
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
