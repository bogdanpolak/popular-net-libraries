using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Xunit;

namespace PopularNetLibraries.MediatR
{
    public class CqrsMediatorTest
    {
        public CqrsMediatorTest() { Domain.InitOrders(); }
        
        [Fact]
        public async Task GetAllOrdersQuery()
        {
            var mediator = MediatorBuilder.Build(typeof(Domain.GetAllOrdersQuery).Assembly);
            var orders = await mediator.Send(new Domain.GetAllOrdersQuery());
            Assert.Collection(orders,
                o=>Assert.Contains("Cable",o.ProductName),
                o=>Assert.Contains("Monitor",o.ProductName),
                o=>Assert.Contains("Camera",o.ProductName)
                );
        }

        [Fact]
        public async Task CreateOrderCommand()
        {
            var mediator = MediatorBuilder.Build(typeof(Domain.CreateOrderCommand).Assembly);
            // ReSharper disable once StringLiteralTypo
            var order = new Domain.Order { ProductName = "HDMI Cable", CustomerCode = "BEPOL" };
            var orderId = await mediator.Send(new Domain.CreateOrderCommand {Order = order});
            Assert.IsType<Guid>(orderId);
            Assert.Equal(4,Domain.CountOrders());
        }
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Domain
    {
        private static List<Order> _orders;

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static void InitOrders()
        {
            _orders = new List<Order>
            {
                new Order
                {
                    ProductName = "HDMI Cable", CustomerCode = "BEPOL", ShipDate = null, 
                    Status = OrderStatus.Completing
                },
                new Order
                {
                    ProductName = "Monitor ViewSonic", CustomerCode = "JANCO", ShipDate = new DateTime(2021, 04, 20),
                    Status = OrderStatus.Shipped
                },
                new Order
                {
                    ProductName = "Microsoft LifeCam Camera", CustomerCode = "ABC", ShipDate = new DateTime(2021, 04, 15),
                    Status = OrderStatus.Delivered
                }
            };            
        }

        public static int CountOrders() => _orders.Count;
        
        public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<Order>>
        {
            public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
            {
                return await Task.Run(() => _orders, cancellationToken);
            }
        }
       
        public class GetAllOrdersQuery : IRequest<List<Order>> { }

        public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
        {
            public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                return await Task.Run(() =>
                {
                    _orders.Add(request.Order);
                    return new Guid();
                }, cancellationToken);
            }
        }

        public class CreateOrderCommand : IRequest<Guid> { public Order Order; }
        
        public class Order { public string ProductName; public string CustomerCode; public DateTime? ShipDate; public OrderStatus Status; }
        public enum OrderStatus { New, Completing, Shipped, Delivered, Returned }
    }
}