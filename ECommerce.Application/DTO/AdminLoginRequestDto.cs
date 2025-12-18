using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTO
{
    public class AdminLoginRequestDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
