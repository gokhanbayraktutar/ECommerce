using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTO
{
    public class ProductSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }

        public string Picture { get; set; }
        public decimal Price { get; set; }
    }
}
