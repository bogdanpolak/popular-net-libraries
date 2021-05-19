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
            public void CountProcessed_LazyCalculateResult()
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

        public class ConverterMock
        {
            public interface IConverter
            {
                string ConvertName(string name);
                bool TryParse(string value, out int outputValue);
            }

            private const string SuperSessions = "super-sessions";

            [Fact]
            public void TryParse_WillReturnLength()
            {
                var converterMock = new Mock<IConverter>();
                var length = SuperSessions.Length;
                converterMock.Setup(converter => converter.TryParse(SuperSessions, out length))
                    .Returns(true);

                var result = converterMock.Object.TryParse(SuperSessions, out var actualLength);

                Assert.True(result);
                Assert.Equal(14, actualLength);
            }
            
            [Fact]
            public void ConvertName_DefineDynamicBehaviour()
            {
                var converterMock = new Mock<IConverter>();
                converterMock.Setup(converter => converter.ConvertName(It.IsAny<string>()))
                    .Returns((string s) => s.ToLower());

                var actual = converterMock.Object.ConvertName("Bogdan Polak");
                
                Assert.Equal("bogdan polak", actual);
            }

        }
    }
}