using System;
using FluentAssertions;
using FluentResults;
using Xunit;

namespace PopularNetLibraries.FluentResults
{
    public class FluentResultTests
    {
        [Fact]
        public void ResultInt_Fail()
        {
            var result = Result.Fail<int>("error message");
            result.IsSuccess.Should().BeFalse();
            Assert.Throws<InvalidOperationException>(() => result.Value);
            result.Reasons.Should().HaveCount(1);
            result.Reasons[0].Message.Should().Be("error message");
        }

        [Fact]
        public void Result_FailWithReasons()
        {
            var ex = new ArgumentException("exception message");
            var result = Result.Fail("error message")
                .WithError("error 2")
                .WithReason(new Error("error 3")
                    .CausedBy(ex))
                .WithSuccess("success message")
                .WithReason(new Success("success 2"));
            result.IsSuccess.Should().BeFalse();
            result.Reasons.Should().HaveCount(5);
        }
    }
}