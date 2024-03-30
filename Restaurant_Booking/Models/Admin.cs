using System.ComponentModel.DataAnnotations;

namespace Restaurant_Booking.Models
{
    public class Admin
    {
        [Key]
        public int Admin_Id { get; set; }
        public string? Admin_Email { get; set; }

        public string? Password { get; set; }
    }
}
