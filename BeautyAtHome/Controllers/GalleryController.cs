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
    [Route("api/v1.0/galleries")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Gallery> _pagingSupport;
        public GalleryController(IGalleryService service, IMapper mapper, IPagingSupport<Gallery> pagingSupport)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }

        /// <summary>
        /// Get a specific gallery by gallery id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the gallery with the corresponding id</returns>
        /// <response code="200">Returns the gallery with the specified id</response>
        /// <response code="404">No galleries found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GalleryVM> GetGalleryById(int id)
        {
            IQueryable<Gallery> galleryList = _service.GetAll(s => s.Images);
            Gallery gallerySearch = galleryList.FirstOrDefault(s => s.Id == id);
            GalleryVM rtnGallery = null;
            if (gallerySearch != null)
            {
                rtnGallery = _mapper.Map<GalleryVM>(gallerySearch);
                return Ok(rtnGallery);
            }
            else
            {
                return NotFound(rtnGallery);
            }
        }

        /// <summary>
        /// Get all galleries
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
        /// <returns>All galleries</returns>
        /// <response code="200">Returns all galleries</response>
        /// <response code="404">No galleries found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<GalleryVM>> GetAllGallery([FromQuery] GallerySM model, int pageSize, int pageIndex)
        {
            IQueryable<Gallery> galleryList = _service.GetAll(s => s.Images);

            if (!string.IsNullOrEmpty(model.Name))
            {
                galleryList = galleryList.Where(s => s.Description.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                galleryList = galleryList.Where(s => s.Description.Contains(model.Description));
            }

           /* if (model.DefaultImageId != 0)
            {
                galleryList = galleryList.Where(s => s.DefaultImageId == model.DefaultImageId);
            }*/

            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(galleryList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<GalleryVM>();

            return Ok(pagedModel);
        }

        /// <summary>
        /// Create a new gallery
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "name": "Gallery's name",
        ///         "description": "Gallery's description",    
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new gallery</response>
        /// <response code="400">Default Image Id does not exist</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GalleryCM>> CreateGallery([FromBody] GalleryCM galleryModel)
        {
            /*DateTime crtDate = DateTime.Now;
            DateTime updDate = DateTime.Now;*/

            Gallery crtGallery = _mapper.Map<Gallery>(galleryModel);

            try
            {
                /*crtService.CreatedDate = crtDate;
                crtService.UpdatedDate = updDate;*/
                /*crtGallery.DefaultImageId = defaultImageId;*/

                await _service.AddAsync(crtGallery);
                await _service.Save();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetGalleryById", new { id = crtGallery.Id }, crtGallery);
        }


        /// <summary>
        /// Update gallery with specified id
        /// </summary>
        /// <param name="id">Gallery's id</param>
        /// <param name="gallery">Information applied to updated gallery</param>
        /// <response code="204">Update gallery successfully</response>
        /// <response code="400">Gallery's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutService(int id, [FromBody] GalleryUM gallery)
        {
            Gallery galleryUpdated = await _service.GetByIdAsync(id);
            if (galleryUpdated == null || id != gallery.Id)
            {
                return BadRequest();
            }

            /*int accountId = 3;
            int serviceTypeId = 1;
            int galleryId = 1;*/

            try
            {
                galleryUpdated.Id = gallery.Id;
                galleryUpdated.Name = gallery.Name;
                galleryUpdated.Description = gallery.Description;
                /*galleryUpdated.DefaultImageId = gallery.DefaultImageId;*/
                /*serviceUpdated.Price = service.Price;
                serviceUpdated.EstimateTime = service.EstimateTime;
                serviceUpdated.Status = service.Status;
                serviceUpdated.UpdatedDate = DateTime.Now;
                serviceUpdated.AccountId = accountId;
                serviceUpdated.ServiceTypeId = serviceTypeId;
                serviceUpdated.GalleryId = galleryId;*/
                _service.Update(galleryUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }


        /// <summary>
        /// Change the status of gallery to disabled
        /// </summary>
        /// <param name="id">Gallery's id</param>
        /// <response code="204">Update gallery's status successfully</response>
        /// <response code="400">gallery's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteGallery(int id)
        {
            Gallery gallerySaved = await _service.GetByIdAsync(id);
            if (gallerySaved == null)
            {
                return BadRequest();
            }

            /*gallerySaved.UpdatedDate = DateTime.Now;
            gallerySaved.Status = Constants.Status.DISABLED;*/

            try
            {
                _service.Update(gallerySaved);
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
