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
    public class Day06 : IDailyTests
    {
        public int Number => 6;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var groups = Utility.Functions.SplitByByElement(input, "")
                                          .Select(grp => new CustomsGroup(grp))
                                          .ToList();

            int num = groups.Sum(x => x.CountYes);

            Assert.IsTrue(num == 7283);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var groups = Utility.Functions.SplitByByElement(input, "")
                                          .Select(grp => new CustomsGroup(grp))
                                          .ToList();

            int num = groups.Sum(x => x.CountAllYes);

            Assert.IsTrue(num == 3520);
        }
    }
}
