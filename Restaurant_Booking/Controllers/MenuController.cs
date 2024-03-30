using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Restaurant_Booking.Data;
using Restaurant_Booking.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
       
        private readonly Restaurant_BookingDbContext _menudetails;

        private readonly IWebHostEnvironment _environment;

        private readonly IConfiguration _configuration;





        public MenuController(Restaurant_BookingDbContext menudetails, IWebHostEnvironment environment, IConfiguration configuration)

        {

            _menudetails = menudetails;

            _environment = environment;

            _configuration = configuration;

        }
        [HttpGet]
      
        public IActionResult GetAllCart()
        {
            var carts = _menudetails.Menu.ToList();

            var cartList = new List<object>();

            foreach (var menu in carts)
            {
                var cartData = new
                {
                 
                    MenuID = menu.MenuID,
                    Name = menu.Name,
                    Price = menu.Price,
                    MenuImage = menu.MenuImage,
                    UniqueFileName = menu.UniqueFileName,
                    imageUrl = String.Format("{0}://{1}{2}/wwwroot/images/{3}", Request.Scheme, Request.Host, Request.PathBase, menu.UniqueFileName)
                };

                cartList.Add(cartData);
            }

            return Ok(cartList);
        }

        // GET: api/menu

        [HttpGet("{Item_Id}")]

        public async Task<Menu> GetById(int id)

        {

            return await _menudetails.Menu.FindAsync(id);

        }

        [HttpGet("{id}/Image")]

        public IActionResult GetImage(int id)

        {

            var request = _menudetails.Menu.Find(id);

            if (request == null)

            {

                return NotFound(); // User not found 

            }



            // Construct the full path to the image file 

            var imagePath = Path.Combine(_environment.WebRootPath, "images", request.UniqueFileName);



            // Check if the image file exists 

            if (!System.IO.File.Exists(imagePath))

            {

                return NotFound(); // Image file not found 

            }



            // Serve the image file 

            return PhysicalFile(imagePath, "image/jpeg");

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
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{menu.MenuImage.FileName}";



            // Save the image to a designated folder (e.g., wwwroot/images) 
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);



            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await menu.MenuImage.CopyToAsync(stream);
            }



            // Store the file path in the database 
            menu.UniqueFileName = uniqueFileName;
            Menu food = new()
            {
                MenuID = menu.MenuID,
                Name = menu.Name,
                Price = menu.Price,
                MenuImage = menu.MenuImage,
                UniqueFileName = menu.UniqueFileName,



            };


            
            _menudetails.Menu.Add(food);
            await _menudetails.SaveChangesAsync();



            return Ok();



        }

        // PUT: api/menu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Menu menu)
        {
            if (id != menu.MenuID)
            {
                return BadRequest();
            }

            _menudetails.Entry(menu).State = EntityState.Modified;

            try
            {
                await _menudetails.SaveChangesAsync();
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



            var mobdetails = _menudetails.Menu.Find(id);



            if (mobdetails == null)



            {



                return NotFound(); // PetAccessory not found 



            }



            _menudetails.Menu.Remove(mobdetails);



            await _menudetails.SaveChangesAsync();



            return NoContent(); // Successfully deleted 



        }
        private bool MenuExists(int id)
        {
            return _menudetails.Menu.Any(e => e.MenuID == id);
        }
    }
}