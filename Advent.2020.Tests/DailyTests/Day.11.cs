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
    public class Day11 : IDailyTests
    {
        public int Number => 11;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownAdjacentSeats()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var map = new SeatingMap(input);
            map.SimulateSeating();

            Assert.IsTrue(map.Occupied == 37);
        }

        [TestMethod]
        public void Test_KnownVisibleSeats()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var map = new SeatingMap(input, tolerance: 5, searchFirstVisible: true);
            map.SimulateSeating();

            Assert.IsTrue(map.Occupied == 26);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var map = new SeatingMap(input);
            map.SimulateSeating();

            Assert.IsTrue(map.Occupied == 2265);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var map = new SeatingMap(input, tolerance: 5, searchFirstVisible: true);
            map.SimulateSeating();

            Assert.IsTrue(map.Occupied == 2045);
        }
    }
}
