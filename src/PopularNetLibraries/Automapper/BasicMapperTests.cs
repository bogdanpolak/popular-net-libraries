using AutoMapper;
using Xunit;

namespace PopularNetLibraries.Automapper
{
    public class BasicMapperTests
    {
        private class Order { public int OrderId; }
        private class OrderDto { public int OrderId; }
        
        [Fact]
        public void BasicMapping()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<Order, OrderDto>());
            var mapper = config.CreateMapper();

            var order = new Order {OrderId = 99};
            OrderDto orderDto = mapper.Map<OrderDto>(order);
            
            Assert.True(orderDto.OrderId == order.OrderId);
        }
        private class Source { public int Value; }
        private class Destination { public int Valueee; }

        [Fact]
        public void InvalidMapping()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<Source, Destination>());
            var mapper = config.CreateMapper();
            
            var exception = Assert.Throws<AutoMapperConfigurationException>( 
                ()=>config.AssertConfigurationIsValid() );
            /*
            Unmapped members were found. Review the types and members below.
            Add a custom mapping expression, ignore, add a custom resolver, or modify the source/destination type
            For no matching constructor, add a no-arg ctor, add optional arguments, or map all of the constructor parameters
            ============================================================================================================
            Source -> Destination (Destination member list)
            PopularNetLibraries.Automapper.BasicMapper+Source -> PopularNetLibraries.Automapper.BasicMapper+Destination (Destination member list)

            Unmapped properties:
            Valueee
             */
            Assert.Contains("Unmapped members were found.",exception.Message);
            Assert.Contains("Unmapped properties:\nValueee",exception.Message);
        } 
    }
    
}