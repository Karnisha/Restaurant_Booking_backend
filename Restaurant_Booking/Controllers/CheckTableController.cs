using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Booking.Data;
using Restaurant_Booking.DTO;

namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckTableController : ControllerBase
    {
        private readonly Restaurant_BookingDbContext _context;

        public CheckTableController(Restaurant_BookingDbContext context)
        {
            _context = context;
        }

        // GET: api/customer_details
        //[Route("GetNoOfTable/{date}/{time}")]
        [HttpPost]
        public IActionResult GetNoOfTable([FromBody]GetTableDto getTable)
        {
            var totaltable = _context.Restaurant.Where(x => x.Restaurant_Id == 1).FirstOrDefault().TotalTables;
            var nooftable=_context.Reservation.Where(x=>x.Date==getTable.Date).Where(j=>j.Time==getTable.Time).Sum(s=>s.NoOfTables);
            var remainingtable = totaltable - nooftable;
            return Ok(remainingtable);

        }
    }
}
