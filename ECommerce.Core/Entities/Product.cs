using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // CartItem ilişkisi
        public ICollection<CartItem> CartItems { get; set; }
    }
}
