using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day13 : IDailyTests
    {
        public int Number => 13;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownBusses()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var manager = new BusManager(input);
            var bus = manager.GetEarliestBus();

            long wait = bus.GetWaitTime(manager.MinTimestamp);
            long val = wait * bus.ID;

            Assert.IsTrue(val == 295);
        }

        [TestMethod]
        public void Test_KnownMinContinous()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var manager = new BusManager(input);
            var t = manager.GetFirstContinuousDepartureTime();

            Assert.IsTrue(t == 1068781);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var manager = new BusManager(input);
            var bus = manager.GetEarliestBus();

            long wait = bus.GetWaitTime(manager.MinTimestamp);
            long val = wait * bus.ID;

            Assert.IsTrue(val == 153);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var manager = new BusManager(input);
            var t = manager.GetFirstContinuousDepartureTime();

            Assert.IsTrue(t == 471793476184394);
        }
    }
}
