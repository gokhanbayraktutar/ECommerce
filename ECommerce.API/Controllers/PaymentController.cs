using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Interfaces;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public PaymentController(
        ICartService cartService,
        IUnitOfWork unitOfWork,
        IConfiguration config)
    {
        _cartService = cartService;
        _unitOfWork = unitOfWork;
        _config = config;
    }

    private int GetUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    private async Task<(string Email, string Phone, string FullName)> GetUserInfoFromDbAsync()
    {
        var userId = GetUserId();
        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        var fullName = $"{user.Name} {user.Lastname}";

        return (user.Email, user.Phone, fullName);
    }


    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] PaymentRequestDto paymentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cart = await _cartService.GetCartByUserIdAsync(GetUserId());
        if (cart == null || !cart.CartItems.Any())
            return BadRequest("Cart is empty");

        var options = new Options
        {
            ApiKey = _config["Iyzipay:ApiKey"],
            SecretKey = _config["Iyzipay:SecretKey"],
            BaseUrl = _config["Iyzipay:BaseUrl"]
        };

        var totalPrice = cart.TotalPaymentPrice ?? 0m;

        var request = new CreatePaymentRequest
        {
            Locale = Locale.TR.ToString(),
            ConversationId = Guid.NewGuid().ToString(),
            Price = totalPrice.ToString("0.##", CultureInfo.InvariantCulture),
            PaidPrice = totalPrice.ToString("0.##", CultureInfo.InvariantCulture),
            Currency = Currency.TRY.ToString(),
            Installment = 1,
            BasketId = cart.Id.ToString(),
            PaymentChannel = PaymentChannel.WEB.ToString(),
            PaymentGroup = PaymentGroup.PRODUCT.ToString(),

            PaymentCard = new PaymentCard
            {
                CardHolderName = paymentDto.PaymentCardHolderName,
                CardNumber = paymentDto.CardNumber,
                ExpireMonth = paymentDto.ExpireMonth,
                ExpireYear = paymentDto.ExpireYear,
                Cvc = paymentDto.Cvc,
                RegisterCard = 0
            },

            Buyer = new Buyer
            {
                Id = GetUserId().ToString(),
                Name = "John",
                Surname = "Doe",
                Email = paymentDto.BuyerEmail,
                IdentityNumber = "11111111111",
                RegistrationAddress = "Address",
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34000"
            },

            ShippingAddress = new Address
            {
                ContactName = "John Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Shipping Address"
            },

            BillingAddress = new Address
            {
                ContactName = "John Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Billing Address"
            },

            BasketItems = cart.CartItems.Select(ci => new BasketItem
            {
                Id = ci.Id.ToString(),
                Name = ci.Product.Name,
                //Category1 = ci.Product.CategoryId.ToString(),
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = (ci.TotalPrice ?? 0m)
                    .ToString("0.##", CultureInfo.InvariantCulture)
            }).ToList()
        };

        Payment payment = await Payment.Create(request, options);

        if (payment != null &&
            payment.Status?.Equals("success", StringComparison.OrdinalIgnoreCase) == true)
        {
            cart.OrderStatus = "Sipariş Alındı";
            cart.OrderDate = DateTime.Now;
            cart.OrderNo = $"ORD-{Guid.NewGuid().ToString("N")[..10].ToUpper()}";
            cart.PaymentType = "Kredi Kartı";
            var userInfo = await GetUserInfoFromDbAsync();
            cart.UserEmail = userInfo.Email;
            cart.Phone = userInfo.Phone;
            cart.FullName = userInfo.FullName;
            cart.Address = paymentDto.Address;
            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.CommitAsync();

            return Ok(new
            {
                cartId = cart.Id,
                orderNo = cart.OrderNo,
                totalPrice = cart.TotalPaymentPrice,
                paymentStatus = payment.Status
            });
        }

        return BadRequest(payment?.ErrorMessage ?? "Payment failed");
    }

}
