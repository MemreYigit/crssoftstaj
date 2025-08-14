using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CrsSoft.Interfaces;

namespace CrsSoft.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

    }
}
