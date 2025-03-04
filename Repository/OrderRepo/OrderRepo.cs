using Holistic_Mission.Data;
using Holistic_Mission.DTOs.CustomerDto;
using Holistic_Mission.DTOs.OrderDto;
using Holistic_Mission.DTOs.ProductDto;
using Holistic_Mission.DTOs.ShoppinCartDTO;
using Holistic_Mission.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Holistic_Mission.Repository.OrderRepo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;
        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
      

        public void AddOrder(OrderResponserDTo orderDto)
        {
            var customer = _context.customers.FirstOrDefault(p => p.Id == orderDto.CustomerforOrderDto.Id);
            customer.Name = customer.Name;
            customer.phone = customer.phone;
            customer.Email = customer.Email;
            _context.customers.Update(customer);
            Order order = new Order()
            {
                TotalPrice = orderDto.TotalPrice,
                Products = orderDto.ProductRequstDto.Select(p => new Product
                {

                    Name = p.Name,
                    stockQuantity = p.stockQuantity,
                    Description = p.Description,

                    
                }).ToList(),
                
                customer = customer,

            };
            _context.orders.Add(order);
            _context.SaveChanges();
        }

        public bool DeleteOrder(int id)
        {
           var order = _context.orders.Include(x=>x.Products).Include(x=> x.customer).ThenInclude(x=>x.ShoppingCart).FirstOrDefault(x=>x.Id == id);
            if (order == null)
            {
                throw new ArgumentException($"Order with {id} not found");
               
            }
            _context.orders.Remove(order);
            _context.SaveChanges();
            return true;
        }

        public List<OrderResponserDTo> GetOrders(OrderResponserDTo orderDto)
        {
            var order = _context.orders.Include(p => p.Products)
                                       .Include(c => c.customer)
                                       .ThenInclude(s => s.ShoppingCart)
                                       .ToList();

            return order.Select(order => new OrderResponserDTo
            {
                TotalPrice = order.TotalPrice,  
                ProductRequstDto = order.Products.Select(product => new ProductRequstDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    stockQuantity = product.stockQuantity,
                }).ToList(),

                CustomerforOrderDto = order.customer != null ? new CustomerforOrderDto
                {
                    Name = order.customer.Name,
                    //Email = order.customer.Email,
                    phone = order.customer.phone,
                    ShoppingCartdto = new ShoppingCartRequstDto
                    {
                        NumOfItems = order.customer.ShoppingCart?.NumOfItems ?? 0  
                    }
                } : null  
            }).ToList();
        }




        public void UpdateOrder(OrderRequstDto orderDto, int id)
        {
            var order = _context.orders.Include(p=>p.Products).FirstOrDefault(p=>p.Id == id);
             order.TotalPrice = orderDto.TotalPrice;

            if (id == null)
            {
                throw new ArgumentException($"Order with {id} not found");
               
            }
            order.Products=orderDto.ProductRequstdto.Select(p=>new Product
            {
                Name = p.Name,
                Description = p.Description,
                stockQuantity=p.stockQuantity,

            }).ToList();
            _context.orders.Update(order);
            _context.SaveChanges();
        }
    }
}
