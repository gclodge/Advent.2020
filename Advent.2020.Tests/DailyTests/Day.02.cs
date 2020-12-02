using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day02 : IDailyTests
    {
        public int Number => 2;

        public string InputFile => TestHelper.GetInputFile(this);

        #region Test Inputs
        private static readonly List<string> TestInputs = new List<string>()
        {
            "1-3 a: abcde",
            "1-3 b: cdefg",
            "2-9 c: ccccccccc"
        };

        private static readonly List<bool> RangeOutcomes = new List<bool>()
        {
            true,
            false,
            true
        };

        private static readonly List<bool> PositionOutcomes = new List<bool>()
        {
            true,
            false,
            false
        };
        #endregion

        [TestMethod]
        public void Test_KnownPasswords_ByRange()
        {
            var passes = TestInputs.Select(x => new TobogganPassword(x, ValidationType.Range)).ToList();

            for (int i = 0; i < passes.Count; i++)
            {
                Assert.IsTrue(passes[i].IsGood == RangeOutcomes[i]);
            }
        }

        [TestMethod]
        public void Test_KnownPasswords_ByPosition()
        {
            var passes = TestInputs.Select(x => new TobogganPassword(x, ValidationType.Position)).ToList();

            for (int i = 0; i < passes.Count; i++)
            {
                Assert.IsTrue(passes[i].IsGood == PositionOutcomes[i]);
            }
        }

        [TestMethod]
        public void PartOne()
        {
            //< Parse all the passwords into memory
            var input = Helpers.FileHelper.ParseFile(InputFile)
                                          .Select(x => new TobogganPassword(x, ValidationType.Range));

            //< Get all the 'good' passwords and snag the count
            var good = input.Where(pass => pass.IsGood);
            int count = good.Count();

            //< Ensure we match the known result
            Assert.IsTrue(count == 474);
        }

        [TestMethod]
        public void PartTwo()
        {
            //< Parse all the passwords into memory
            var input = Helpers.FileHelper.ParseFile(InputFile)
                                          .Select(x => new TobogganPassword(x, ValidationType.Position));

            //< Get all the 'good' passwords and snag the count
            var good = input.Where(pass => pass.IsGood);
            int count = good.Count();

            //< Ensure we match the known result
            Assert.IsTrue(count == 745);
        }
    }
}
