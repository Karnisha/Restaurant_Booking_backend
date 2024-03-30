using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Restaurant_Booking.Data;
using Restaurant_Booking.DTO;
using Restaurant_Booking.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Restaurant_DetailsController : ControllerBase
    {

        private readonly Restaurant_BookingDbContext _restaurantdetails;

        private readonly IWebHostEnvironment _environment;

        private readonly IConfiguration _configuration;
        public Restaurant_DetailsController(Restaurant_BookingDbContext restaurantdetails, IWebHostEnvironment environment, IConfiguration configuration)

        {

            _restaurantdetails = restaurantdetails;

            _environment = environment;

            _configuration = configuration;

        }


        // GET: api/menu

        //[HttpGet("Restaurant/{restaurant_Id}")]

        //public async Task<Restaurant> GetById(int restaurant_Id)

        //{

        //    return await _restaurantdetails.Restaurant.FindAsync(restaurant_Id);

        //}

        [HttpGet("Restaurant/{restaurant_Id}")]
        public async Task<ActionResult<Restaurant>> GetById(int restaurant_Id)
        {
            var restaurant = await _restaurantdetails.Restaurant.FindAsync(restaurant_Id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }


        [HttpGet("{id}/Image")]

        public IActionResult GetImage(int id)

        {

            var request = _restaurantdetails.Restaurant.Find(id);

            if (request == null)

            {

                return NotFound(); // User not found 

            }
           // Construct the full path to the image file 

            var imagePath = Path.Combine(_environment.WebRootPath, "images", request.UniqueFileName); // Check if the image file exists 

            if (!System.IO.File.Exists(imagePath))

            {

                return NotFound(); // Image file not found 

            }



            // Serve the image file 

            return PhysicalFile(imagePath, "images/jpeg");

        }

        //// GET: api/menu/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Menu>> GetMenu(int id)
        //{
        //    var menu = await _menudetails.Menu.FindAsync(id);

        //    if (menu == null)
        //    {
        //        return NotFound();
        //    }

        //    return menu;
        //}


        // POST: api/menu
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostMenu(Restaurant restaurant)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{restaurant.RestaurantImage.FileName}";



            // Save the image to a designated folder (e.g., wwwroot/images) 
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);



            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await restaurant.RestaurantImage.CopyToAsync(stream);
            }



            // Store the file path in the database 
            restaurant.UniqueFileName = uniqueFileName;
            Restaurant food = new()
            {
                   Restaurant_Id = restaurant.Restaurant_Id,
                    Restaurant_Name = restaurant.Restaurant_Name,
                    Email_Id = restaurant.Email_Id,
                    ContactNumber = restaurant.ContactNumber,
                    Password = restaurant.Password,
                    Location = restaurant.Location,
                    Type = restaurant.Type,
                    Cuisine = restaurant.Cuisine,
                    TotalTables = restaurant.TotalTables,
                    Status = restaurant.Status,
                    Personal_Email = restaurant.Personal_Email,
                    UniqueFileName = restaurant.UniqueFileName,
               


            };



            _restaurantdetails.Restaurant.Add(food);
            await _restaurantdetails.SaveChangesAsync();



            return Ok();



        }

        // PUT: api/menu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Restaurant menu)
        {
            if (id != menu.Restaurant_Id)
            {
                return BadRequest();
            }

            _restaurantdetails.Entry(menu).State = EntityState.Modified;

            try
            {
                await _restaurantdetails.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/menu/5
        [HttpDelete("{id}")]



        public async Task<IActionResult> DeleteItemDetails(int id)



        {
            var mobdetails = _restaurantdetails.Restaurant.Find(id);
              if (mobdetails == null)



            { 

                return NotFound(); // PetAccessory not found 

            }
            _restaurantdetails.Restaurant.Remove(mobdetails);
             await _restaurantdetails.SaveChangesAsync();
             return NoContent(); // Successfully deleted
             }

        private bool MenuExists(int id)
        {
            return _restaurantdetails.Restaurant.Any(e => e.Restaurant_Id == id);
        }
    }
}
