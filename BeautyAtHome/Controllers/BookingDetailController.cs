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
    [Route("api/v1.0/booking-details")]
    public class BookingDetailController : ControllerBase
    {

        private readonly IBookingDetailService _service;
        private readonly IMapper _mapper;
        private readonly IPagingSupport<BookingDetail> _pagingSupport;

        public BookingDetailController(IBookingDetailService service, IMapper mapper, IPagingSupport<BookingDetail> pagingSupport)
        {
            _service = service;
            _mapper = mapper;
            _pagingSupport = pagingSupport;
        }

        /// <summary> 
        /// Get a specific bookingDetail by bookingDetail ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET {
        ///         "id" : "1"
        ///     }
        /// </remarks>
        /// <returns>Return the bookingDetail with the corresponding id</returns>
        /// <response code="200">Returns the bookingDetail with the specified id</response>
        /// <response code="404">No bookingDetails found with the specified id</response>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        /*[SwaggerResponseExample(StatusCodes.Status200OK, typeof (ApplicationCore.DTOs.Booking))]*/
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public  ActionResult<BookingDetailVM> GetBookingDetailById(int id)
        {

            IQueryable<BookingDetail> bookingDetailQuery = _service.GetAll(_ => _.FeedBack.Gallery.Images);
            BookingDetail bookingDetail = bookingDetailQuery.FirstOrDefault(s => s.Id == id);


            if (bookingDetail == null)
            {
                return NotFound();
            }

            var rtnBookingDetail = _mapper.Map<BookingDetailVM>(bookingDetail);

            return Ok(rtnBookingDetail);
        }

        /// <summary>
        /// Create a new bookingDetail
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "quantity": 0,
        ///         "id": 0,
        ///         "bookingId": 0,
        ///         "serviceId": 0,
        ///         "servicePrice": 0,
        ///         "serviceName": "string"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created new bookingDetail</response>
        /// <response code="400">Booking type's id or gallery's id does not exist</response>
        /// <response code="500">Failed to save request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDetailCM>> CreateBooking([FromBody] BookingDetailCM serviceModel)
        {
            //TODO: Implements Booking.GetById(int id) does not exist, return BadRequest()

            

            BookingDetail crtBookingDetail = _mapper.Map<BookingDetail>(serviceModel);

            try
            {
                //crtBookingDetail.Quantity = quantity;
                //crtBookingDetail.BookingId = bookingId;
                //crtBookingDetail.ServiceId = serviceId;
                //crtBookingDetail.ServiceName = serviceName;
                //crtBookingDetail.ServicePrice = servicePrice;


                await _service.AddAsync(crtBookingDetail);
                await _service.Save();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetBookingDetailById", new { id = crtBookingDetail.Id }, crtBookingDetail);
        }

        /// <summary>
        /// Get all bookingDetails
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
        /// <returns>All bookingDetails</returns>
        /// <response code="200">Returns all bookingDetails</response>
        /// <response code="404">No bookingDetails found</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookingDetailVM>> GetAllBookingDetail([FromQuery] BookingDetailSM model, int pageSize, int pageIndex)
        {
            IQueryable<BookingDetail> bookingDetailList = _service.GetAll(s => s.Booking, s => s.Service, s => s.FeedBack);

            if (!string.IsNullOrEmpty(model.ServiceName))
            {
                bookingDetailList = bookingDetailList.Where(s => s.ServiceName.Contains(s.ServiceName));
            }

            if (model.BookingId > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.BookingId == model.BookingId);
            }
            if(model.Id > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.Id == model.Id);
            }

            if (model.ServiceId > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.ServiceId == model.ServiceId);
            }

            if (model.QuantityMin > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.Quantity >= model.QuantityMin);
            }
            if (model.QuantityMax > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.Quantity <= model.QuantityMax);
            }

            if (model.ServicePriceMin > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.ServicePrice >= model.ServicePriceMin);
            }

            if (model.ServicePriceMax > 0)
            {
                bookingDetailList = bookingDetailList.Where(s => s.ServicePrice <= model.ServicePriceMax); 
            }


            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var pagedModel = _pagingSupport.From(bookingDetailList)
                .GetRange(pageIndex, pageSize, s => s.Id)
                .Paginate<BookingDetailVM>();

            return Ok(pagedModel);
        }
    }
}
