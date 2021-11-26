using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Xunit;

namespace PopularNetLibraries.MediatR
{
    public class NotificationTest
    {
        private readonly IMediator _mediator;

        public NotificationTest()
        {
            
            _mediator = MediatorBuilder
                .Build(GetType().GetTypeInfo().Assembly);
        }
        
        [Fact]
        public async Task AddTwoProductsIntoBasket()
        {
            const string userBogdan = "bogdan.smith";
            await _mediator.Publish(new AddProductToBasket.Event(
                userBogdan,
                "15939975"));
            await _mediator.Publish(new AddProductToBasket.Event(
                userBogdan,
                "1520287912081"));
            Basket.ForAUser(userBogdan).Products.Should().HaveCount(2);
        }
    }

    public static class AddProductToBasket
    {
        public record Event(string UserName, string ProductNumber) : INotification;

        public class Handler : INotificationHandler<Event>
        {
            public Task Handle(Event @event, CancellationToken cancellationToken)
            {
                var (userName, productNumber) = @event;
                if (string.IsNullOrEmpty(userName)) 
                    throw new ArgumentException("User name is empty");
                Basket
                    .ForAUser(userName)
                    .AddProduct(productNumber);
                return Task.CompletedTask;
            }
        }
    }

    public record Basket(List<Product> Products)
    {
        private static readonly Dictionary<string, Basket> CurrentBaskets = new(); 
        public void AddProduct(string productCode) => Products.Add(new Product(productCode));
        public static Basket ForAUser(string userName)
        {
            if (CurrentBaskets.ContainsKey(userName))
                return CurrentBaskets[userName];
            var basket = new Basket(new List<Product>());
            CurrentBaskets[userName] = basket;
            return basket;
        }
    }

    public record Product(string Code);
}
