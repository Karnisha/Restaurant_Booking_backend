using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Booking.Data;
using Restaurant_Booking.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Restaurant_BookingDbContext _context;

        public LoginController(Restaurant_BookingDbContext context)
        {
            _context = context;
        }

        //[HttpPost("signup")]
        //public async Task<IActionResult> SignUp(Customer customer)
        //{
        //    if (await _context.Customer.AnyAsync(c => c.Customer_Name == customer.Customer_Name))
        //    {
        //        return Conflict("Username already exists");
        //    }

        //    _context.Customer.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}
        [HttpPost("adminlogin")]
        public async Task<IActionResult> Admin_login(Admin admin)
        {
            if (await _context.Admin.AnyAsync(c => c.Admin_Email == admin.Admin_Email))
            {
                return Conflict("Username already exists");
            }

            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            return Ok();
        }
        //[HttpPost("Restaurant_SignUp")]

        //public async Task<IActionResult> Restaurant_SignUp(Restaurant restaurant)
        //{
        //    if (await _context.Restaurant.AnyAsync(c => c.Email_Id == restaurant.Email_Id))
        //    {
        //        return Conflict("Email Id already exists");
        //    }

        //    _context.Restaurant.Add(restaurant);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Customer_Email == model.Email && c.Password == model.Password);
            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.Admin_Email == model.Email && a.Password == model.Password);
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(r => r.Email_Id == model.Email && r.Password == model.Password);

            if (customer != null)
            {
                // Customer login
                // You might return a JWT token here for authentication
                return Ok("Customer login successful");
            }
            else if (admin != null)
            {
                // Admin login
                // You might return a JWT token here for authentication
                return Ok("Admin login successful");
            }
            else if (restaurant != null)
            {
                // Restaurant login
                // You might return a JWT token here for authentication
                return Ok("Restaurant login successful");
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }
    }
}