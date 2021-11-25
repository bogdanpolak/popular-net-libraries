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
            _mediator = MediatorBuilder.Build(typeof(NotificationTest).GetTypeInfo().Assembly);
        }
        
        [Fact]
        public async Task AddTwoProductsIntoBasket()
        {
            const string userBogdanPolak = "bpolak";
            await _mediator.Publish(new AddProductToBasketEvent.Notification(
                userBogdanPolak,
                "15939975"));
            await _mediator.Publish(new AddProductToBasketEvent.Notification(
                userBogdanPolak,
                "1520287912081"));
            Basket.ForAUser(userBogdanPolak).Products.Should().HaveCount(2);
            Basket.ForAUser("abc").Products.Should().HaveCount(0);
        }
    }

    internal static class AddProductToBasketEvent
    {
        internal record Notification(string UserName, string ProductNumber) : INotification;

        internal class Handler : INotificationHandler<Notification>
        {
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                var (userName, productNumber) = notification;
                if (string.IsNullOrEmpty(userName)) throw new ArgumentException("User name is empty");
                Basket.ForAUser(userName).AddProduct(productNumber);
                return Task.CompletedTask;
            }
        }
    }
    
    internal record Basket(List<Product> Products)
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
    internal record Product(string Code);
}