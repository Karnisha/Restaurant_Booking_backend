using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Booking.Data;
using Restaurant_Booking.DTO;

namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GettAllRestaurantController : ControllerBase
    {
        private readonly Restaurant_BookingDbContext _restaurantdetails;

        private readonly IWebHostEnvironment _environment;

        private readonly IConfiguration _configuration;





        public GettAllRestaurantController(Restaurant_BookingDbContext restaurantdetails, IWebHostEnvironment environment, IConfiguration configuration)

        {

            _restaurantdetails = restaurantdetails;

            _environment = environment;

            _configuration = configuration;

        }
        [HttpPost]

        public IActionResult GetAllCart([FromBody] GetTableDto getTableDto)
        {
            var carts = _restaurantdetails.Restaurant.ToList();

            var cartList = new List<object>();

            foreach (var menu in carts)
            {
                var cartData = new
                {

                    Restaurant_Id = menu.Restaurant_Id,
                    Restaurant_Name = menu.Restaurant_Name,
                    Email_Id = menu.Email_Id,
                    ContactNumber = menu.ContactNumber,
                    Location = menu.Location,
                    Type = menu.Type,
                    Cuisine = menu.Cuisine,
                    TotalTables = menu.TotalTables - _restaurantdetails.Reservation.Where(x => x.Date == getTableDto.Date).Where(j => j.Time == getTableDto.Time).Sum(s => s.NoOfTables),
                    Status = menu.Status,
                    Personal_Email = menu.Personal_Email,
                    UniqueFileName = menu.UniqueFileName,
                    imageUrl = String.Format("{0}://{1}{2}/wwwroot/images/{3}", Request.Scheme, Request.Host, Request.PathBase, menu.UniqueFileName)
                };

                cartList.Add(cartData);
            }

            return Ok(cartList);
        }
    }
}
