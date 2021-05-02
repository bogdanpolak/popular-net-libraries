using AutoMapper;
using Xunit;

namespace PopularNetLibraries.Automapper
{
    class Order { public int OrderId; }

    class OrderDto { public int OrderId; }

    public class BasicMapper
    {
        [Fact]
        public void Test01()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<Order, OrderDto>());
            var mapper = config.CreateMapper();

            var order = new Order {OrderId = 99};
            OrderDto orderDto = mapper.Map<OrderDto>(order);
            
            Assert.Equal(99, orderDto.OrderId);
        }
    }
}