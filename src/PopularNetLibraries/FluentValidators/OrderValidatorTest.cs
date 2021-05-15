using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Xunit;

namespace PopularNetLibraries.FluentValidators
{
    public class OrderValidatorTest
    {
        
        private class LineItem
        {
            public string Product;
            public int Quantity;
            public decimal Price;
            public decimal Total => decimal.Round(Price * Quantity, 2);
        }
        
        private class Order
        {
            public List<LineItem> LineItems = new List<LineItem>();
            public Order AddItem(string product, int quantity, decimal price)
            { 
                LineItems.Add(new LineItem{Product = product, Quantity = quantity, Price = price});
                return this;
            }
            public DateTime OrderDate;
            public DateTime? ShipDate;
        }

        public static class ValidationErrors
        {
            public const string InvalidOrderDate = "INV-ORD-DATE";
        }
        
        private class OrderValidator : AbstractValidator<Order>
        {

            public OrderValidator()
            {
                RuleForEach(order => order.LineItems).SetValidator(new LineItemValidator());
                RuleFor(order => order.OrderDate)
                    .NotNull()
                    .GreaterThan(new DateTime(1900, 1, 1))
                    .WithErrorCode(ValidationErrors.InvalidOrderDate);
                When(order=>order.ShipDate.HasValue, 
                    () => RuleFor(order => order.ShipDate)
                        .GreaterThanOrEqualTo(order => order.OrderDate)
                        .WithMessage("Ship Date has to be greater than Order Date")
                    );
                RuleFor(order => order.LineItems)
                    .Must(lineItems => lineItems.Any())
                    .WithMessage("Orders has to have at least one item");
                RuleFor(order => order.LineItems.Sum(item => item.Total))
                    .GreaterThan(Decimal.Zero);
            }
        }
        private class LineItemValidator : AbstractValidator<LineItem>
        {
            public LineItemValidator()
            {
                RuleFor(lineItem => lineItem.Quantity).GreaterThan(0);
            }
        }
        
        [Fact]
        public void ValidateOrder_Ok()
        {
            var order = new Order {OrderDate = new DateTime(2021, 02, 15), ShipDate = null}
                .AddItem("Sony Alpha 7C Silver", 1, 2098.00M)
                .AddItem("USB C to HDMI Cable 6ft", 5, 13.49M);

            var orderValidator = new OrderValidator();
            var result = orderValidator.Validate(order);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateOrder_Invalid_ShipDate_LessThen_OrderDate()
        {
            var order = new Order
                {
                    OrderDate = new DateTime(2021, 02, 15), 
                    ShipDate = new DateTime(2021, 02, 01)
                }
                .AddItem("USB C to HDMI Cable 6ft", 5, 13.49M);

            var orderValidator = new OrderValidator();
            var result = orderValidator.Validate(order);

            Assert.False(result.IsValid);
            Assert.Equal("Ship Date has to be greater than Order Date",result.Errors[0].ErrorMessage);
        }
        
        [Fact]
        public void ValidateOrder_Invalid_OrderDate()
        {
            var order = new Order{OrderDate = new DateTime(1800, 12, 31), ShipDate = null}
                .AddItem("USB C to HDMI Cable 6ft", 5, 13.49M);

            var orderValidator = new OrderValidator();
            var result = orderValidator.Validate(order);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrors.InvalidOrderDate,result.Errors[0].ErrorCode);
        }
        
        // TODO: Add fact for invalid order: no LineItems
        // TODO: Add fact for invalid order: LineItems.Sum(total) == 0
        // TODO: Add fact for invalid order: empty order (multiple errors)
    }
}