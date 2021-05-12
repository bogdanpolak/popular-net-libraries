using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PopularNetLibraries
{
    public static class PlaygroundTests
    {
        public class ReferenceVsValueType
        {
            private struct ValueType
            {
                public int Number;
                public decimal Price;
            }

            private class ReferenceType
            {
                public int Number;
                public decimal Price;
            }

            [Fact]
            public void DemoFact()
            {
                var valueA = new ValueType {Number = 1, Price = 9.99M};
                var valueB = valueA;

                var referenceA = new ReferenceType {Number = 1, Price = 9.99M};
                var referenceB = referenceA;

                valueB.Number++;
                valueB.Price = 11.33M;
                referenceB.Number++;
                referenceB.Price = 11.33M;

                Assert.NotEqual(valueA.Number, valueB.Number);
                Assert.Equal(referenceA.Number, referenceB.Number);
                Assert.Equal(9.99M, valueA.Price);
                Assert.Equal(11.33M, referenceA.Price);
            }
        }

        public class ProblemMinMaxSum
        {
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
            public void OneToFive()
            {
                var numbers = new int[] {1, 2, 3, 4, 5};
                var actual = MinMaxSum(numbers);
                Assert.Equal("10 14", actual);
            }
        }

        public class ProblemAlmostSorted 
        {
            private static string AlmostSorted(List<int> number)
            {
                return ""; // swap idx1 idx2  | reverse idx1 idx2  | no
            }

            [Fact]
            public void AlmostSorted_Reverse()
            {
                var numbers = new List<int> {1, 2, 6, 5, 4, 3, 7, 8};
                var actual = AlmostSorted(numbers);
                Assert.Equal("reverse 3 6", actual);
            }

            [Fact]
            public void AlmostSorted_SwapOverReverse()
            {
                var numbers = new List<int> {1, 3, 2, 4}; // swap
                var actual = AlmostSorted(numbers);
                Assert.Equal("no", actual);
            }

            [Fact]
            public void AlmostSorted_no()
            {
                var numbers = new List<int> {1, 3, 2, 4, 7, 6, 5, 8}; // no = 2 operations needed: swap + reverse
                var actual = AlmostSorted(numbers);
                Assert.Equal("no", actual);
            }
        }
    }
}