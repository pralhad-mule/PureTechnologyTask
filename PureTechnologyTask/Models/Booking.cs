
namespace PureTechnologyTask.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string custimername { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string roomtype { get; set; }

        public string CheckInDate { get; set; }

        public string CheckOutDate { get; set; }

        public int Guests { get; set; }

        public decimal Price_Per_Nigh { get; set; }

        public int TotalPrice { get; set; }
    }
}
