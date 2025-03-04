using Holistic_Mission.DTOs.OrderDto;

namespace Holistic_Mission.Repository.OrderRepo
{
    public interface IOrderRepo
    {
       public List<OrderResponserDTo> GetOrders(OrderResponserDTo orderDto);
        public void AddOrder(OrderResponserDTo orderDto); 
       public void UpdateOrder (OrderRequstDto orderDto,int id);
       public bool DeleteOrder(int id);

    }
}
