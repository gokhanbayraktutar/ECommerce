using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<Category> ChildCategories { get; set; } = new List<Category>();

        public ICollection<Product>? Products { get; set; }
    }
}
