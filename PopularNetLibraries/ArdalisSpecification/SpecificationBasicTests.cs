using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace PopularNetLibraries.ArdalisSpecification
{
    public class SpecificationBasicTests
    {
        [Fact]
        public void EvaluateSpecification_Where()
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

        [Fact]
        public void EvaluateSpecification_Paging()
        {
            var fixture = new Fixture();
            var orders = fixture.CreateMany<Order>(21).ToArray();
            var spec = new OrdersPageSpec(5);
            var pageOrders = spec.Evaluate(orders).ToList();
            pageOrders.Should().HaveCount(5);
            pageOrders[0].Should().Be(orders[10]);
        }
        
        private static class Buyer
        {
            public const string Telco = "Telco";
        }
    }

    public sealed class OrdersPageSpec : Specification<Order>
    {
        public OrdersPageSpec(int page)
        {
            if (page <= 0) 
                throw new ArgumentException("page have to be grater than zero", nameof(page));
            var skip = (page-1) * 5;
            Query.Skip(skip).Take(5);
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