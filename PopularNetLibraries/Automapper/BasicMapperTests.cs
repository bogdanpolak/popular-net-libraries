using System.Collections.Generic;
using System.Linq;
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
            Assert.Contains("Unmapped members were found",exception.Message);
            Assert.Contains("Valueee",exception.Message);
        }

        class Journal
        {
            public List<double> Grades; 
            public Student Student;
            public double GetAverageGrade() => Grades.Average();
        }

        private class Student
        {
            public int ID;
            public string FirstName;
            public string LastName;
            public string GetFullName() => $"{FirstName} {LastName}";
        }

        private class YearGrade
        {
            public string StudentFullName;
            public double AverageGrade;
        }

        [Fact]
        public void MapWithFlattering()
        {
            var config = new MapperConfiguration(expression => expression.CreateMap<Journal, YearGrade>());
            var mapper = config.CreateMapper();
            var journal = new Journal
            {
                Student = new Student {ID = 1, FirstName = "Bogdan", LastName = "Polak"},
                Grades = new List<double>{3.5, 4.5, 4, 3.5, 4.2, 4.5, 3.8, 5}
            };

            var yearGrade = mapper.Map<YearGrade>(journal);
            
            Assert.Equal("Bogdan Polak",yearGrade.StudentFullName);
            Assert.Equal(expected:4.125,actual:yearGrade.AverageGrade,precision:4);
        } 
    }
    
}