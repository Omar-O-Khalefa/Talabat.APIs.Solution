using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        //private readonly UserManager<AppUser> _userManger;

        public OrdersController(IOrderService orderService,IMapper mapper/*,UserManager<AppUser> _userManger*/)
        {
            _orderService = orderService;
            _mapper = mapper;
            //_userManger = _userManger;
        }
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse),StatusCodes.Status400BadRequest)]
        [HttpPost] // POST : api/Orders

        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var Addresss = _mapper.Map<AddressDto, Core.Entities.Order_Aggregate.Address>(orderDto.ShippingAddress);
            //var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            //var user = await _userManger.FindByEmailAsync(email);
            var order =  await _orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, Addresss);

            if(order is null)
            {
                return BadRequest(new APIResponse(400));
            }
            return Ok(order);   
        }

        [HttpGet] //GET : api/Orders?email=omar@gmail.com
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser(string email)
        {
            var orders = await _orderService.GetOrderForUserAsync(email);

            return Ok(orders);
        }
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]  // GET : api/Orders/1?email=omar@gmail.com
        public async Task<ActionResult<Order>> GetOrderForUser( string email,int id )
        {
            var order = await _orderService.GetOrderByIdForAsync( email, id);
            if(order is null)
            {
                return NotFound(new APIResponse(404));
            }
            return Ok(order);
        }
    }
}
