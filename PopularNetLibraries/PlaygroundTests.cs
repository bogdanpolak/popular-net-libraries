using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                var numbers = new[] {1, 2, 3, 4, 5};
                var actual = MinMaxSum(numbers);
                Assert.Equal("10 14", actual);
            }
        }

        public class ProblemOneSwapToSort  // aka AlmostSorted
        {
            private static string SwapToSort(List<int> numbers)
            {
                var idx1 = -1;  // index of the first un ordered item in the lists (numbers)
                var idx2 = -1;  // index of the second un ordered item in the lists (numbers)
                for (var idx = 1; idx < numbers.Count; idx++)
                {
                    if (numbers[idx - 1] <= numbers[idx]) continue;
                    if (idx1 < 0)
                        idx1 = idx;
                    else if (idx2 < 0)
                        idx2 = idx;
                    else
                        return "no";
                }
                if (idx1 < 0) return "yes";
                if (idx2 < 0) idx2 = idx1;
                var isAbleToSwap = (idx1 == 1 || numbers[idx1 - 2] <= numbers[idx2]) &&
                                   (idx2 == numbers.Count - 1 ||
                                    numbers[idx1 - 1] <= numbers[idx2 + 1]);
                return isAbleToSwap ? $"swap {idx1} {idx2 + 1}" : "no";
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
            public void SwapToSort_Swap_TwoFirst_When4Numbers()
            {
                var numbers = new List<int> {2, 1, 3, 4};
                var actual = SwapToSort(numbers);
                Assert.Equal("swap 1 2", actual);
            }

            [Fact]
            public void SwapToSort_Swap_TwoLast_When4Numbers()
            {
                var numbers = new List<int> {1, 2, 4, 3};
                var actual = SwapToSort(numbers);
                Assert.Equal("swap 3 4", actual);
            }
            
            [Fact]
            public void SwapToSort_no_When4Numbers()
            {
                var numbers = new List<int> {1, 3, 4, 2};
                var actual = SwapToSort(numbers);
                Assert.Equal("no", actual);
            }
            
            [Fact]
            public void SwapToSort_Swap_TwoNext()
            {
                var numbers = new List<int> {1, 3, 2, 4};
                var actual = SwapToSort(numbers);
                Assert.Equal("swap 2 3", actual);
            }
            
            [Fact]
            public void SwapToSort_Swap_TwoSeparated()
            {
                var numbers = new List<int> {1, 5, 3, 4, 2, 6};
                var actual = SwapToSort(numbers);
                Assert.Equal("swap 2 5", actual);
            }
        }
        
        public class ProblemReverseToSort
        {
            private static string ReverseToSort(IReadOnlyList<int> numbers)
            {
                return numbers.Count<2 || numbers.Count==2 && numbers[0]<numbers[1] ? "yes" : "";  
                // yes | reverse idx1 idx2  | no
            }
            [Fact]
            public void ReverseToSort_Reverse4ElemsInside()
            {
                // var numbers = new List<int> {1, 2, 6, 5, 4, 3, 7, 8};
                // var actual = ReverseToSort(numbers);
                // TODO: Assert.Equal("reverse 3 6", actual);
            }

            [Fact]
            public void ReverseToSort_NotAbleToSort()
            {
                _ = ReverseToSort(new List<int> {1, 3, 2, 4, 7, 6, 5, 8});
                // var numbers = new List<int> {1, 3, 2, 4, 7, 6, 5, 8}; // no
                // var actual = ReverseToSort(numbers);
                // TODO: Assert.Equal("no", actual);
            }
        }
        
        // TODO: Add regex sample
        // https://docs.microsoft.com/en-us/azure/hdinsight/hadoop/apache-hadoop-dotnet-csharp-mapreduce-streaming
        public class RegexDemoTests
        {
            private string MostPopularWords(string[] text, int minimalOccurrence=2, string[] excludedWords = null) 
            {
                var words = new List<string>();
                foreach (var line in text)
                {
                    // strip out punctuation, numbers, etc.
                    var onlyText = Regex.Replace(line, @"\.|;|:|,|[0-9]|'", "");
                    // split at whitespace
                    var matches = Regex.Matches(onlyText, @"[\w]+");
                    words.AddRange( matches.Select(res => res.Value) );
                }
                if (excludedWords != null)
                {
                    words.RemoveAll(excludedWords.Contains);
                }
                var wordCounters = words.Aggregate(
                    new Dictionary<string, int>(),
                    (counters, word) =>
                    {
                        var lowerWord = word.ToLower();
                        counters[lowerWord] = counters.ContainsKey(lowerWord) ? counters[lowerWord]+1 : 1;
                        return counters;
                    });
                var orderedWordCounters = wordCounters
                    .OrderByDescending(pair => pair.Value)
                    .ToList();
                var mostPopularWords = orderedWordCounters
                    .Where(pair => pair.Value >= minimalOccurrence)
                    .Select(pair=>$"{pair.Key}={pair.Value}")
                    .ToList();
                var result = String.Join(" ", mostPopularWords);
                return result;
            }
            
            [Fact]
            public void Regex_ListWords()
            {
                var text = new[]{
                    "the action or activity of purchasing goods from stores",
                    "shopping is so exhausting—all those decisions",
                    "a busy shopping area blue painted",
                    "I unloaded all the shopping",
                    "you did a good job of explaining",
                    "she's had a plan to nose job",
                    "the car was a blue painted malevolent looking job",
                    "I have plan to take a part-time job",
                    "it's our job to find things out",
                    "A few pedestrians carrying their evening shopping sheltered from the rain in doorways",
                    "she is the architect who planned the new shopping centre",
                    "On the debit side the new shopping centre will increase traffic problems",
                    "immediately job a blue penknife into the throat",
                    "he prepared to job the huge brute",
                    "if left unfettered he would job",
                    "she wants to be left alone to get on with the job",
                    "someone had done a skillful paint job",
                    "his game plan is to buy in then job the shares on at a profit",
                    "we thought you'd have plan a job getting there"
                };
                var excludedWords = new[] {"a", "the", "to", "on", "is", "she", "he", "i", "you"};
                var mostPopularWords = MostPopularWords(text, 3, excludedWords);
                Assert.Equal("job=12 shopping=6 plan=4 blue=3", mostPopularWords);
            }
        }
    }
}