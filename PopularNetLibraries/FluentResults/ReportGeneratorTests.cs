using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using FluentResults;
using Xunit;

namespace PopularNetLibraries.FluentResults
{
    public class ReportGeneratorTests
    {
        [Fact]
        public void ReportGenerator_WithResultSuccess()
        {
            var result = ReportGenerator
                .ProcessData(21.November(2021))
                .Generate();
            result.IsSuccess.Should().BeTrue();
            result.ValueOrDefault.Should().HaveCount(2);
        }
        
        [Fact]
        public void ReportGenerator_WithResultFail()
        {
            var result = new ReportGenerator(21.November(2021))
                .Generate();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].Message.Should().Be("Report is not ready yet, please call ProcessData");
        }
    }

    public record ReportGenerator(DateTime StartDate)
    {
        private bool DataAreReady { get; init; }

        public static ReportGenerator ProcessData(DateTime startDate)
        {
            var demo = new ReportGenerator(startDate)
            {
                DataAreReady = true
            };
            return demo;
        }
        
        public Result<IEnumerable<Part>> Generate() 
        {
            return DataAreReady ? 
                Result.Ok(Parts.Where(x=>x.Created > StartDate)) : 
                Result.Fail($"Report is not ready yet, please call {nameof(ProcessData)}");
        }

        private static readonly DateTime Nov20 = 20.November(2021);
        private static readonly DateTime Nov21 = 21.November(2021);
        private IList<Part> Parts { get; } = new List<Part>
        {
            new("134562", 12.30m, Nov20.At(17,30)),
            new("315231", 7.98m, Nov20.At(21,45)),
            new("336622", 19.45m, Nov21.At(6,25)),
            new("653109", 9.51m, Nov21.At(8,42)),
        };
    };

    public record Part(string Number, decimal Weight, DateTime Created);
}