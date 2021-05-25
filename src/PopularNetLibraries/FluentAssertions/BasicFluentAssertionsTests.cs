using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace PopularNetLibraries.FluentAssertions
{
    public class BasicFluentAssertionsTests
    {
        [Fact]
        public void Should_ForString()
        {
            var actual = "ABCD";
            actual += "EFGHI";
            
            actual.Should()
                .StartWith("AB")
                .And.EndWith("HI")
                .And.Contain("EF")
                .And.HaveLength(9);
        }
    }
}
