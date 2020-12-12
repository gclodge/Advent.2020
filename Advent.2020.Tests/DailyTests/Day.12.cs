using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day12 : IDailyTests
    {
        public int Number => 12;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownCommands()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var shipCapt = new ShipCaptain(input);
            shipCapt.Navigate();

            Assert.IsTrue(shipCapt.Distance == 25);
        }

        [TestMethod]
        public void Test_KnownCommands_WithWaypoint()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var shipCapt = new ShipCaptain(input, ShipNavigationType.Waypoint);
            shipCapt.Navigate();

            Assert.IsTrue(shipCapt.Distance == 286);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var shipCapt = new ShipCaptain(input);
            shipCapt.Navigate();

            Assert.IsTrue(shipCapt.Distance == 562);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var shipCapt = new ShipCaptain(input, ShipNavigationType.Waypoint);
            shipCapt.Navigate();

            Assert.IsTrue(shipCapt.Distance == 101860);
        }
    }
}
