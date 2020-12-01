using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day01 : IDailyTests
    {
        public int Number => 1;

        public string InputFile => TestHelper.GetInputFile(this);

        private static readonly List<int> TestInputs = new List<int>()
        {
            1721,
            979,
            366,
            299,
            675,
            1456
        };

        private const int _SumValue = 2020;

        [TestMethod]
        public void Test_KnownInputs()
        {
            //< Get the value (A * B) of the two values that sum to 2020
            var val = Utility.Functions.FindRecordsThatSumTo(TestInputs, _SumValue, 2);
            //< Ensure we match the known result
            Assert.IsTrue(val == 514579);
        }

        [TestMethod]
        public void PartOne()
        {
            const int numToSum = 2;

            //< Parse all input data - convert to ints
            var input = Helpers.FileHelper.ParseFile(InputFile, int.Parse);
            //< Get the value (A * B) of the two values that sum to 2020
            var val = Utility.Functions.FindRecordsThatSumTo(input, _SumValue, numToSum);
            //< Ensure we match the known result
            Assert.IsTrue(val == 1019571);
        }

        [TestMethod]
        public void PartTwo()
        {
            const int numToSum = 3;

            //< Parse all input data - convert to ints
            var input = Helpers.FileHelper.ParseFile(InputFile, int.Parse);
            //< Try to find the local permutation (n = 3) that sums to 2020 - return the multiple of those values
            var val = Utility.Functions.FindRecordsThatSumTo(input, _SumValue, numToSum);

            //< Check we match that known result, boah
            Assert.IsTrue(val == 100655544);
        }
    }
}
