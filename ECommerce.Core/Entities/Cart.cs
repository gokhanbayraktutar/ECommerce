using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        // User ilişkisi (1-1)
        public int UserId { get; set; }
        public User User { get; set; }

        // Sepet içeriği
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
