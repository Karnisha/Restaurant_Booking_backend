namespace Restaurant_Booking.DTO
{
    public class Booking
    {
        public string? Date { get; set; }

        public string? Time { get; set; }
        public int Customer_Id { get; set; }
        public int Customer_Email {  get; set; }
        public int Restaurant_Id { get; set; }

        public int NoOfTables { get; set; }

    }
}
