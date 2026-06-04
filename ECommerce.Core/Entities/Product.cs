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
        public string ProductCode { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public int? Stock { get; set; }


        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<ProductPicture> ProductPictures { get; set; } = new List<ProductPicture>();
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
