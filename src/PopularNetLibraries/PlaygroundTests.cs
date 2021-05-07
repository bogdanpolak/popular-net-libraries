using System.Linq;
using Xunit;

namespace PopularNetLibraries
{
    public class PlaygroundTests
    {
        private struct ValueType { public int Number; public decimal Price; }
        private class ReferenceType { public int Number; public decimal Price; }
        
        [Fact]
        public void ReferenceVsValueType()
        {
            var valueA = new ValueType {Number = 1, Price = 9.99M};
            var valueB = valueA;

            var referenceA = new ReferenceType {Number = 1, Price = 9.99M};
            var referenceB = referenceA;

            valueB.Number++;
            valueB.Price = 11.33M;
            referenceB.Number++;
            referenceB.Price = 11.33M;
            
            Assert.NotEqual( valueA.Number, valueB.Number);
            Assert.Equal( referenceA.Number, referenceB.Number);
            Assert.Equal(9.99M, valueA.Price);
            Assert.Equal(11.33M, referenceA.Price);
        }

        private string MinMaxSum(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0) 
                return "Invalid size. Input numbers cant be empty";
            if (numbers.Length != 5) 
                return "Invalid size. Input numbers requires to have 5 items";
            var sum = numbers.Sum();
            var max = numbers.Max();
            var min = numbers.Min();
            return $"{sum - max} {sum - min}";
        }
    
        [Fact]
        public void MinMaxHacker()
        {
            var numbers = new int[] {1, 2, 3, 4, 5};
            var actual = MinMaxSum(numbers);
            Assert.Equal("10 14", actual);
        }
    }
}