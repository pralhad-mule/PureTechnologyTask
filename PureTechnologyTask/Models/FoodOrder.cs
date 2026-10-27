
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PureTechnologyTask.Models
{
    public class FoodOrder
    {
        public int OrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string customerName { get; set; }

        public string MenuType { get; set; }

        public List<string> SelectedItems { get; set; }

        public List<int> ItemQuantities { get; set; }

        public decimal TotalPrice { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
