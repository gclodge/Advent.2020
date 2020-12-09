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
    public class Day09 : IDailyTests
    {
        public int Number => 9;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownPreamble()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile, long.Parse);
            var xmas = new XmasEncoder(input, 5);

            var badVal = xmas.CheckFirstInvalidValue();
            Assert.IsTrue(badVal.HasValue && badVal.Value == 127);
        }

        private const long _PartOneResult = 15353384;
        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile, long.Parse);
            var xmas = new XmasEncoder(input, 25);

            var badVal = xmas.CheckFirstInvalidValue();
            Assert.IsTrue(badVal.HasValue && badVal == _PartOneResult);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile, long.Parse);
            var xmas = new XmasEncoder(input, 25);

            var weakness = xmas.FindEncryptionWeakness(_PartOneResult);
            Assert.IsTrue(weakness.HasValue && weakness == 2466556);
        }
    }
}
