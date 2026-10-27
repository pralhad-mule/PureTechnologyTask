
namespace PureTechnologyTask.Models
{
    public class OrderItem
    {
        public string FoodName { get; set; }

        public string ItemType { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool Selected { get; set; }
    }
}
