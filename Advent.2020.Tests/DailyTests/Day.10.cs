using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day10 : IDailyTests
    {
        public int Number => 10;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        public static List<int> KnownAdapters = new List<int>()
        {
            16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4
        };

        [TestMethod]
        public void Test_KnownAdapters()
        {
            var arr = new JoltageArray(KnownAdapters);
            var diffs = arr.GetAdapterArrayDifferences();

            Assert.IsTrue(diffs.Count == 2);
            Assert.IsTrue(diffs[1] == 7);
            Assert.IsTrue(diffs[3] == 5);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile, int.Parse).ToList();
            var arr = new JoltageArray(input);

            var diffs = arr.GetAdapterArrayDifferences();
            int res = diffs[1] * diffs[3];

            Assert.IsTrue(res == 2170);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile, int.Parse).ToList();
            var arr = new JoltageArray(input);

            long combos = arr.CountPossibleCombinations();
            Assert.IsTrue(combos == 24803586664192);
        }
    }
}
