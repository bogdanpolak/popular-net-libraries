using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace PopularNetLibraries.FluentAssertions
{
    public class CollectionAssertTests
    {
        [Fact]
        public void Assertions_ListOf_Int()
        {
            var numbers = new[] { 0.5, 1.3, 2, 3 };

            numbers.Should()
                .OnlyContain(n => n >= 0)
                .And.HaveCount(4)
                .And.Equal(0.5, 1.3, 2, 3)
                .And.OnlyHaveUniqueItems()
                .And.StartWith(0.5)
                .And.ContainSingle(elem => elem >= 3)
                .And.NotContain(0)
                .And.NotBeNull()
                .And.BeInAscendingOrder();
        }

        [Fact]
        public void AssertionException()
        {
            var numbers = new[] { 0.5, 1.3, 2, 3 };
            
            var exception = Assert.Throws<XunitException>( ()=>
                numbers.Should().OnlyContain(n => n >= 1));
            Assert.Contains(
                "Expected numbers to contain only items matching (n >= 1)",
                exception.Message);
        }

        [Fact]
        public void Assertions_ListOf_String()
        {
            var texts = new[] {"Initial item", "Final item"};

            texts.Should()
                .ContainMatch("* item");
        }

        private class Employee { public string Name; public int Level; public DateTime EmployedOn; }
        
        [Fact]
        public void Assertion_ObjectCollection()
        {
            var employees = new List<Employee> {
                new Employee {Name = "Matt", Level = 3, EmployedOn = new DateTime(2020, 02, 01)},
                new Employee {Name = "Lou", Level = 5, EmployedOn = new DateTime(2019, 05, 01)},
                new Employee {Name = "Chris", Level = 5, EmployedOn = new DateTime(2017, 12, 01)}
            };

            employees.Should()
                .Contain(emp => emp.Level >= 1 && emp.Level <= 9)
                .And.Contain(emp => emp.EmployedOn >= 1.January(2001))
                .And.NotContainNulls();
            employees.Select(emp => emp.EmployedOn).Should();

            employees.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Matt");
                    first.Level.Should().BeInRange(1, 9);
                    first.EmployedOn.Should().Be(1.February(2020));
                },
                second => second.EmployedOn.Should().Be(1.May(2019)),
                third => third.EmployedOn.Should().Be(1.December(2017))
                );
        }
    }
}