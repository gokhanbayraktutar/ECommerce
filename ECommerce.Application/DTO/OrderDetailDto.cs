using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTO
{
    public class OrderDetailDto
    {
        public int CartId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public string PaymentType { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }

}
