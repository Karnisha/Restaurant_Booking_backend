using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Booking.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }

        [NotMapped]
        public IFormFile? MenuImage { get; set; }
        public string? UniqueFileName { get; set; }


    }
}
