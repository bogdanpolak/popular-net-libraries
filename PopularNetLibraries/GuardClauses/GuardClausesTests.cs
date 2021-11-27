using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace PopularNetLibraries.GuardClauses
{
    public class GuardClausesTests
    {
        [Fact]
        public void CheckDomainClasses()
        {
            var order = Order.New( 10.November(2021).At(12,37))
                .AddItem("4812570", 1, 19.5m)
                .AddItem("2457601", 2, 8.99m)
                .Place();
            order.OrderId.Should().NotBeNull();
            order.DateCreated.Should().HaveDay(10);
            order.Items.Should().HaveCount(2);
        }

        [Fact]
        public void Guard_CratedDate_OutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Order.New(DateTime.MaxValue));
        }

        [Fact]
        public void Guard_OrderItem_UnitPrice()
        {
            var ex = Assert.Throws<ArgumentException>(()=> 
                Order.New()
                    .AddItem("37420987", 1, -11.20m));
            ex.Message.Should().Contain("Required input unitPrice cannot be negative");
        }

        [Fact]
        public void Guard_OrderItem_Quantity()
        {
            var ex = Assert.Throws<ArgumentException>(()=> 
                Order.New()
                    .AddItem("37420987", 0, -11.20m));
            ex.Message.Should().Contain("Required input quantity cannot be zero or negative");
        }

        [Fact]
        public void Guard_OrderItem_ProductCode()
        {
            var ex = Assert.Throws<ArgumentException>(()=> 
                Order.New()
                    .AddItem("", 0, -11.20m));
            ex.Message.Should().Contain("Required input productCode was empty");
        }
        
        [Fact]
        public void Guard_Against_InvalidInput()
        {
            var idx= Guard.Against.InvalidInput(20, "idx", i => i>10 );
            idx.Should().Be(20);
        }
        
        [Fact]
        public void Guard_Against_OutOfRange_ForEnumerable()
        {
            var list= Guard.Against.OutOfRange(
                new[] {10, 20, 30}, "list", 10,30 );
            list.Should().HaveCount(3);
        }
        
        [Fact]
        public void Guard_Against_InvalidFormat()
        {
            const string emailRegex = 
                @"^([0-9a-zA-Z]" + //Start with a digit or alphabetical
                @"([\+\-_\.][0-9a-zA-Z]+)*" + // No continuous or ending +-_. chars in email
                @")+" +
                @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";
            var email = Guard.Against.InvalidFormat("bogdan.smith@gmail.com", "email", 
                emailRegex);
            email.Should().Be("bogdan.smith@gmail.com");
            
            Assert.Throws<ArgumentException>(() =>
                Guard.Against.InvalidFormat("bogdan@", "email", emailRegex));

        }
    }

    public class Order
    {
        public string OrderId { get; private set; }
        public DateTime DateCreated { get; private init;  }
        public List<OrderItem> Items { get; } = new();

        public static Order New(DateTime? created = null)
        {
            return new Order
            {
                DateCreated = Guard.Against.OutOfSQLDateRange(created ?? DateTime.Now, nameof(created))
            };
        }

        public Order AddItem(string productCode, int quantity, decimal unitPrice)
        {
            Items.Add(new OrderItem(
                Guard.Against.NullOrWhiteSpace(productCode, nameof(productCode)),
                Guard.Against.NegativeOrZero(quantity, nameof(quantity)),
                Guard.Against.Negative(unitPrice, nameof(unitPrice))
            ));
            return this;
        }

        public Order Place()
        {
            var random = new Random();
            OrderId = $"order-{1000+random.Next(9000)}";
            return this;
        }
    };

    public record OrderItem(string ProductCode, int Quantity, decimal UnitPrice);
}