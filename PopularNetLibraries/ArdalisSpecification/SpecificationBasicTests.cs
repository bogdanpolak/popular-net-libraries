using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using FluentAssertions;
using Xunit;

namespace PopularNetLibraries.ArdalisSpecification
{
    public class SpecificationBasicTests
    {
        [Fact]
        public void Test()
        {
            var orders = new List<Order>
            {
                new(12862, Buyer.Telco, 1700.00m),
                new(20365, "SmartServices", 2150.00m),
                new(28057, Buyer.Telco, 1700.00m),
                new(31635, "SmartServices", 4575.00m),
            };
            var specification = new CustomerOrdersSpecification(Buyer.Telco);
            var telcoOrders = specification.Evaluate(orders).ToArray();
            telcoOrders.Should().HaveCount(2);
            var orderIds = telcoOrders.Select(x => x.OrderId);
            orderIds.Should().StartWith(new[] { 12862, 28057 });
        }
        
        private static class Buyer
        {
            public const string Telco = "Telco";
        }
    }

    public sealed class CustomerOrdersSpecification : Specification<Order>
    {
        public CustomerOrdersSpecification(string buyerId)
        {
            Query.Where(o => o.BuyerId == buyerId);
        }
    }

    public record Order(int OrderId, string BuyerId, decimal Total);
}