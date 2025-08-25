// OrdersController.cs
using CrsSoft.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrsSoft.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ICartCookieService cartCookieService;

        public OrderController(IOrderService orderService, ICartCookieService cartCookieService)
        {
            this.orderService = orderService;
            this.cartCookieService = cartCookieService;
        }


        [Authorize]
        [HttpPost("createfromcart")]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { success = false, message = "User not authenticated" });
                }

                var cartIdStr = cartCookieService.GetOrCreate(HttpContext);
                if (!Guid.TryParse(cartIdStr, out Guid cartId))
                {
                    return BadRequest(new { success = false, message = "Invalid cart ID" });
                }

                var order = await orderService.CreateOrderFromCart(cartId, userId);
                return Ok(new { success = true, orderId = order.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("orderDetails")] 
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { success = false, message = "User not authenticated" });
                }

                var orders = await orderService.GetOrderDetails(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}