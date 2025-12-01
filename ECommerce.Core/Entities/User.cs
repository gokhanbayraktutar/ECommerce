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

        // Bir kullanıcının tek sepeti olur
        public Cart? Cart { get; set; } 
    }
}
