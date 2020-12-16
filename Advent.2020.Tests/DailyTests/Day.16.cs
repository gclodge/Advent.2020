using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day16 : IDailyTests
    {
        public int Number => 16;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownTickets()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var manager = new TicketManager(input);
            int error = manager.GetScanningErrorRate();

            Assert.IsTrue(error == 71);
        }

        [TestMethod]
        public void Test_KnownFields()
        {
            var input = Helpers.FileHelper.ParseFile(TestHelper.GetTestFile(this, "Test2"));

            var manager = new TicketManager(input);
            var positions = manager.GetValidFieldPositions();
            var values = manager.GetMyTicketValues(positions);

            Assert.IsTrue(values["row"] == 11);
            Assert.IsTrue(values["class"] == 12);
            Assert.IsTrue(values["seat"] == 13);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var manager = new TicketManager(input);
            int error = manager.GetScanningErrorRate();

            Assert.IsTrue(error == 29759);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var manager = new TicketManager(input);
            var positions = manager.GetValidFieldPositions();
            var values = manager.GetMyTicketValues(positions);

            string target = "departure";
            //< Want the values that start with 'departure'
            var targetValues = values.Where(kvp => kvp.Key.StartsWith(target)).ToList();
            //< Ensure that it's only six values
            Assert.IsTrue(targetValues.Count == 6);

            long finalVal = 1;
            foreach (var kvp in targetValues)
            {
                finalVal *= kvp.Value;
            }

            Assert.IsTrue(finalVal == 1307550234719);
        }
    }
}
