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
    [Route("api/v1.0/bookings")]
    public class BookingController : ControllerBase
    {

        private readonly IBookingService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<Booking> _pagingSupport;

        public BookingController(IBookingService service, IMapper mapper, IPagingSupport<Booking> pagingSupport)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }

        /// <summary>
        /// Get a specific booking by bookingId
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the booking with the corresponding id</returns>
        /// <response code="200">Returns the booking with the specified id</response>
        /// <response code="404">No bookings found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        /*[SwaggerResponseExample(StatusCodes.Status200OK, typeof (ApplicationCore.DTOs.Booking))]*/
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<BookingVM>> GetBookingById(int id)
        {

            IQueryable<Booking> bookingList = _service.GetAll(s => s.BeautyArtistAccount, s => s.CustomerAccount, s => s.BookingDetails);
            var booking = bookingList.FirstOrDefault(_ => _.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var rtnBooking = _mapper.Map<BookingVM>(booking);

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

            return Ok(rtnBooking);
        }

        /// <summary>
        /// Create a new booking
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "bookingType": "string",
        ///         "status": "string",
        ///         "customerAccountId": 0,
        ///         "beautyArtistAccountId": 0,
        ///         "note": "string",
        ///         "endAddressId": 0,
        ///         "rateScore": 0,
        ///         "galleryId": 0,
        ///         "beginAddressId": 0,
        ///         "totalFee": 0,
        ///         "transportFee": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new booking</response>
        /// <response code="400">BookingDetail type's id or gallery's id does not exist</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingCM>> CreateBooking([FromBody] BookingCM serviceModel)
        {
            //TODO: Implements BookingDetail.GetById(int id) does not exist, return BadRequest()
            //TODO: Implements GaleryId.GetById(int id) does not exist, return BadRequest()

            DateTime createDate = DateTime.Now;
            DateTime updateDate = DateTime.Now;
            

            /*string bookingType = "";
            string status = "Active";
            int customerAccountId = 1;
            int beautyArtistAccountId = 1;
            string note = "";
            int endAddressId = 1;
            int galleryId = 1;
            int beginAddressId = 1;
            double totalFee = 10.5;
            double transportFee = 3.0;*/
            Booking crtBooking = _mapper.Map<Booking>(serviceModel);

            try
            {
                crtBooking.CreateDate = createDate;
                crtBooking.UpdateDate = updateDate;
                //crtFeedback.BookingDetail = bookingDetail;
                //crtFeedback.Gallery = gallery;

                await _service.AddAsync(crtBooking);
                await _service.Save();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetBookingById", new { id = crtBooking.Id }, crtBooking);
        }

        /// <summary>
        /// Get all bookings
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
        /// <returns>All bookings</returns>
        /// <response code="200">Returns all bookings</response>
        /// <response code="404">No bookings found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookingVM>> GetAllBooking([FromQuery] BookingSM model, int pageSize, int pageIndex)
        {
            IQueryable<Booking> bookingList = _service.GetAll(s => s.BeautyArtistAccount, s => s.CustomerAccount, s => s.BookingDetails);

   
            if (!string.IsNullOrEmpty(model.Status))
            {
                bookingList = bookingList.Where(s => s.Status.Contains(s.Status));
            }           
            if (!string.IsNullOrEmpty(model.Note))
            {
                bookingList = bookingList.Where(s => s.Note.Contains(s.Note));
            }

            if (model.CustomerAccountId > 0)
            {
                bookingList = bookingList.Where(s => s.CustomerAccountId == model.CustomerAccountId);
            }

            if (model.BeautyArtistAccountId > 0)
            {
                bookingList = bookingList.Where(s => s.BeautyArtistAccountId == model.BeautyArtistAccountId);
            }
        

            if (model.TotalFee > 0)
            {
                bookingList = bookingList.Where(s => s.TotalFee >= model.TotalFee); 
            }

            if (model.CreateDateMin.HasValue)
            {
                bookingList = bookingList.Where(s => s.CreateDate >= model.CreateDateMin);
            }

            if (model.CreateDateMin.HasValue)
            {
                bookingList = bookingList.Where(s => s.CreateDate <= model.CreateDateMax);
            }
            if (model.UpdateDateMin.HasValue)
            {
                bookingList = bookingList.Where(s => s.UpdateDate >= model.UpdateDateMin);
            }

            if (model.UpdateDateMax.HasValue)
            {
                bookingList = bookingList.Where(s => s.UpdateDate <= model.UpdateDateMax);
            }


            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(bookingList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<BookingVM>();

            return Ok(pagedModel);
        }

        /// <summary>
        /// Change the status of booking to disabled
        /// </summary>
        /// <param name="id">Booking's id</param>
        /// <response code="204">Update booking's status successfully</response>
        /// <response code="400">Booking's id does not exist</response>
        /// <response code="500">Failed to update</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            Booking bookingSaved = await _service.GetByIdAsync(id);
            if (bookingSaved == null)
            {
                return BadRequest();
            }

            bookingSaved.UpdateDate = DateTime.Now;
            bookingSaved.Status = Constants.Status.DISABLED;

            try
            {
                _service.Update(bookingSaved);
                await _service.Save();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        /// <summary>
        /// Update Booking with specified id
        /// </summary>
        /// <param name="id">Booking's id</param>
        /// <param name="booking">Information applied to updated Booking</param>
        /// <response code="204">Update Booking successfully</response>
        /// <response code="400">Booking's id does not exist or does not match with the id in parameter</response>
        /// <response code="500">Failed to update</response>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutBooking(int id, [FromBody] BookingUM booking)
        {
            Booking bookingUpdated = await _service.GetByIdAsync(id);
            if (bookingUpdated == null || id != booking.Id)
            {
                return BadRequest();
            }

            try
            {
                bookingUpdated.Id = booking.Id;
                bookingUpdated.Status = booking.Status;
                bookingUpdated.UpdateDate =  DateTime.Now;

                _service.Update(bookingUpdated);
                await _service.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }
    }
}
