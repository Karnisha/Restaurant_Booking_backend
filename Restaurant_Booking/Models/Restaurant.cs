using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Booking.Models
{
    public class Restaurant
    {
        [Key]
        public int Restaurant_Id { get; set; }

        public string? Restaurant_Name { get; set; }

        public string? Email_Id {  get; set; }

        public string? ContactNumber {  get; set; }


        public string? Password {  get; set; }

        public string? Location {  get; set; }

        public string? Type  { get; set; }

        public string? Cuisine {  get; set; }
        public int TotalTables {  get; set; }

        [DefaultValue("waiting for approval")]
        public string? Status {  get; set; }
        
        public string? Personal_Email { get; set; }
        [NotMapped]
        public IFormFile? RestaurantImage { get; set; }
        public string? UniqueFileName { get; set; }


    }
}
