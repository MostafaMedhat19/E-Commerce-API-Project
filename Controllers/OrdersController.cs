using Holistic_Mission.DTOs.OrderDto;
using Holistic_Mission.Repository.OrderRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Holistic_Mission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {   
        public OrdersController(IOrderRepo orderRepo)
        {
             _orderRepo = orderRepo;
        }
        private readonly IOrderRepo _orderRepo;

        [HttpGet("GetOrders")]
        public ActionResult<List<OrderResponserDTo>> GetOrders()
        {
           
           
             var orders = _orderRepo.GetOrders(new OrderResponserDTo());
            if (orders == null) { 
                return NotFound();  
            
            }

            return Ok(orders);
            
           
        }

        [HttpPost("AddOrder")]
        public ActionResult AddOrder([FromBody] OrderResponserDTo orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order data is null");
            }

            
             _orderRepo.AddOrder(orderDto);
            return Created(); 
            
           
        }

      
        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int id, [FromBody] OrderRequstDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order data is null");
            }

            _orderRepo.UpdateOrder(orderDto, id);
             return Accepted(); 
            
         
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var result = _orderRepo.DeleteOrder(id);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }

}

