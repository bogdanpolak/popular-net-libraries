using System;
using Moq;
using Xunit;

namespace PopularNetLibraries.Moq
{
    public class BasicMoqTests
    {
        public class MockProcessor
        {
            public interface IAssetProcessor
            {
                bool Process(string category);
                bool Process(string category, int maxItems);
                int CountProcessed();
            }

            private const string Alternate = "alternate";
            private const string Illustrations = "illustrations";

            [Fact]
            public void Process_Defined_WillReturnTrue()
            {
                var assetProcessorMock = new Mock<IAssetProcessor>();
                assetProcessorMock.Setup(processor => processor.Process(Alternate)).Returns(true);
                var actual = assetProcessorMock.Object.Process(Alternate);
                Assert.True(actual);
            }

            [Fact]
            public void Process_WillReturnFalse()
            {
                var assetProcessorMock = new Mock<IAssetProcessor>();
                assetProcessorMock.Setup(processor => processor.Process(Alternate)).Returns(true);
                var actual = assetProcessorMock.Object.Process(Alternate, 10);
                Assert.False(actual);
            }

            [Fact]
            public void Process_WillhrowException()
            {
                var assetProcessorMock = new Mock<IAssetProcessor>();
                assetProcessorMock.Setup(foo => foo.Process(Illustrations))
                    .Throws<InvalidOperationException>();
                assetProcessorMock.Setup(foo => foo.Process(""))
                    .Throws(new ArgumentException("command"));

                Assert.Throws<InvalidOperationException>(
                    () => assetProcessorMock.Object.Process(Illustrations));
                Assert.Throws<ArgumentException>(
                    () => assetProcessorMock.Object.Process(""));
            }

            [Fact]
            public void CountProcessedd_LazyCalculateResult()
            {
                var count = 1;
                var assetProcessorMock = new Mock<IAssetProcessor>();
                assetProcessorMock.Setup(foo => foo.CountProcessed()).Returns(() => count);
                count += 99;

                var actual = assetProcessorMock.Object.CountProcessed();
                Assert.Equal(100, actual);
            }
            // TODO: Add Async method to IAssetProcessor
        }

    }
}