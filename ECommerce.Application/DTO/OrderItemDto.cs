using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTO
{
    public class OrderItemDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string Picture { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
