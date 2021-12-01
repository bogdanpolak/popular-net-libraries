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

        [Fact]
        public void Result_Try_WithFail()
        {
            var resultFailed = Result.Try(() => Converters.ToTemperature("110"));
            resultFailed.IsFailed.Should().BeTrue();
            var error = resultFailed.Errors[0] as ExceptionalError;
            if (error is null) throw new Exception("Invalid error");
            error.Exception.Should().BeAssignableTo<ArgumentOutOfRangeException>();
            error.Exception.Message.Should().Be("Out of temperature range (-150..100) (Parameter 'text')");

        }

        [Fact]
        public void Result_Try_WithSuccess()
        {
            var resultSuccess = Result.Try(() => Converters.ToTemperature("-10"));
            resultSuccess.ValueOrDefault.Should().Be(-10);
        }
    }

    public static class Converters
    {
        public static int ToTemperature(string text)
        {
            if (!int.TryParse(text, out var value))
                throw new ArgumentException("Not a number", nameof(text));
            if (value is <= -150 or >= 100)
                throw new ArgumentOutOfRangeException(nameof(text), "Out of temperature range (-150..100)");
            return value;
        }
    }
}