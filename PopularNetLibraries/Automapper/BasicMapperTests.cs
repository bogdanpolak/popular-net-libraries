using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace PopularNetLibraries.Automapper
{
    public class BasicMapperTests
    {
        [Fact]
        public void BasicMapping()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<Order, OrderDto>());
            var mapper = config.CreateMapper();

            var order = new Order{ OrderId = 99 };
            var orderDto = mapper.Map<OrderDto>(order);
            
            orderDto.OrderId.Should().Be(99);
        }

        [Fact]
        public void ReverseMapping()
        {
            var config = new MapperConfiguration(expression =>
            {
                expression.CreateMap<Order, OrderDto>().ReverseMap();
            });
            var mapper = config.CreateMapper();

            var order = new Order{ OrderId = 99 };
            var orderDto = mapper.Map<OrderDto>(order);
            var orderDto2 = new OrderDto { OrderId = 312 };
            var order2 = mapper.Map<Order>(orderDto2);
            
            orderDto.OrderId.Should().Be(99);
            order2.Should().BeEquivalentTo(new { OrderId = 312 });
        }

        [Fact]
        public void AssertConfigurationIsValid_WithUnmappedMembers_ShouldThrow()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<OrderRequest, Order>());
            
            var exception = Assert.Throws<AutoMapperConfigurationException>( 
                ()=>config.AssertConfigurationIsValid() );
            Assert.Contains("Unmapped members were found",exception.Message);
            Assert.Contains("OrderId",exception.Message);
        }

        [Fact]
        public void Map_WithCustomMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderRequest, Order>()
                    .ForMember(
                        dest => dest.OrderId,
                        opt => opt.MapFrom(src => src.Id));
            });
            var mapper = config.CreateMapper();

            var orderRequest = new OrderRequest{ Id = 124 };
            var order = mapper.Map<Order>(orderRequest);

            order.OrderId.Should().Be(orderRequest.Id);
        }

        [Fact]
        public void Map_WithFlattering()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<Journal, YearGrade>());
            var mapper = config.CreateMapper();
            var journal = new Journal
            {
                Student = new Student{ FirstName = "Bogdan", LastName = "Smith" },
                Grades = new List<double>{ 3.5, 4.5, 4, 3.5, 4.2, 4.5, 3.8, 5 }
            };
            journal.Student.GetFullName().Should().Be("Bogdan Smith");
            journal.GetAverageGrade().Should().Be(4.125);

            var yearGrade = mapper.Map<YearGrade>(journal);

            yearGrade.Should().BeEquivalentTo(new YearGrade
            {
                StudentFullName = "Bogdan Smith", 
                AverageGrade = 4.125
            });
        } 

        // --------------------------------------------------------------------------

        private class Order { public int OrderId; }
        private class OrderDto { public int OrderId; }
        private class OrderRequest { public int Id; }

        
        class Journal
        {
            public List<double> Grades; 
            public Student Student;
            public double GetAverageGrade() => Grades.Average();
        }

        private class Student
        {
            public string FirstName;
            public string LastName;
            public string GetFullName() => $"{FirstName} {LastName}";
        }

        private class YearGrade
        {
            public string StudentFullName;
            public double AverageGrade;
        }

    }
    
}