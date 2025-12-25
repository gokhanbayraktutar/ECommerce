using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string OrderStatus { get; set; } = "Sepette";

        public string? PaymentType { get; set; }

        public string? OrderNote { get; set; }
        public string? Address { get; set; }
        public string? UserEmail { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }

        public decimal? TotalPaymentPrice { get; set; }

        public string? OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }

        public User User { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
