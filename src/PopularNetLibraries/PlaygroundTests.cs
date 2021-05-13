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

        public class ProblemOneSwapToSort
        {
            private static string SwapToSort(List<int> number)
            {
                return ""; // yes | swap idx1 idx2  | no
            }

            [Fact]
            public void SwapToSort_Swap_WhenTwoElemArray()
            {
                var numbers = new List<int> {6, 5};
                var actual = SwapToSort(numbers);
                Assert.Equal("swap 1 2", actual);
            }

            [Fact]
            public void SwapToSort_Yes_WhenTwoSorted()
            {
                var numbers = new List<int> {50, 72};
                var actual = SwapToSort(numbers);
                Assert.Equal("yes", actual);
            }

            [Fact]
            public void SwapToSort_Swap_TwoNext()
            {
                var numbers = new List<int> {1, 3, 2, 4};
                var actual = SwapToSort(numbers);
                Assert.Equal("swap 2 3", actual);
            }
        }
        
        public class ProblemReverseToSort
        {
            private string ReverseToSort(List<int> number)
            {
                return "";  // yes | reverse idx1 idx2  | no
            }
            [Fact]
            public void ReverseToSort_Reverse4ElemsInside()
            {
                var numbers = new List<int> {1, 2, 6, 5, 4, 3, 7, 8};
                var actual = ReverseToSort(numbers);
                Assert.Equal("reverse 3 6", actual);
            }

            [Fact]
            public void ReverseToSort_NotAbleToSort()
            {
                var numbers = new List<int> {1, 3, 2, 4, 7, 6, 5, 8}; // no
                var actual = ReverseToSort(numbers);
                Assert.Equal("no", actual);
            }
        }
    }
}