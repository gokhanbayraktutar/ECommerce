using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public string? Email { get; set; }
        public string?Name { get; set; }

        public string? Lastname { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
