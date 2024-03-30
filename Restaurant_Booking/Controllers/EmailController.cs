
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Restaurant_Booking.Models;


namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _adcontext;
        public EmailController(IEmailService adcontext)
        {
            _adcontext = adcontext;
        }


        [HttpPost("SendingEmail")]
        public async Task<IActionResult> Add([FromForm] Restaurant restaurantDetails)
        {
            Restaurant center = new Restaurant()
            {
                Restaurant_Id = restaurantDetails.Restaurant_Id,
                Restaurant_Name = restaurantDetails.Restaurant_Name,
                Password = restaurantDetails.Password,
                Personal_Email = restaurantDetails.Personal_Email,

            };

            await SendEmailToCenterAsync(restaurantDetails);
            return CreatedAtAction(nameof(Add), restaurantDetails);

        }


        private async Task SendEmailToCenterAsync(Restaurant restaurantDetails)
        {
            try
            {

                var centerEmail = restaurantDetails.Personal_Email; // Replace with actual admin email address
                var subject = $"Your Center Email and Password";
                var body = $"Your Email id is: {restaurantDetails.Email_Id} Password: {restaurantDetails.Password}";

                await _adcontext.SendEmailAsync(centerEmail, subject, body);
                // Log success or handle any exceptions
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return;
            }
        }
    }
}
// Define an interface for your email service
public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

// Implement your email service (e.g., using SmtpClient)
public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (var client = new SmtpClient("smtp-mail.outlook.com"))
        {
            client.Port = 587;
            client.Credentials = new NetworkCredential("karnishak8@gmail.com", "Karnisha@08");
            client.EnableSsl = true;

            var message = new MailMessage
            {
                From = new MailAddress("karnishak8@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }
    }
}







