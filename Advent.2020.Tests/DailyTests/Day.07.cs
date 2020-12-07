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
    public class Day07 : IDailyTests
    {
        public int Number => 7;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownRules()
        {
            string testType = "shiny gold";
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var handler = new BaggageHandler(input);
            int count = handler.CountBagsThatCanContain(testType);

            Assert.IsTrue(count == 4);
        }

        [TestMethod]
        public void PartOne()
        {
            string type = "shiny gold";
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var handler = new BaggageHandler(input);
            int count = handler.CountBagsThatCanContain(type);

            Assert.IsTrue(count == 161);
        }

        [TestMethod]
        public void Test_KnownContents()
        {
            string testType = "shiny gold";
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var handler = new BaggageHandler(input);

            int count = handler.CountBagsWithin(testType);

            Assert.IsTrue(count == 32);
        }

        [TestMethod]
        public void Test_KnownContents_Two()
        {
            string testType = "shiny gold";
            var input = Helpers.FileHelper.ParseFile(TestHelper.GetTestFile(this, "Test2"));

            var handler = new BaggageHandler(input);
            int count = handler.CountBagsWithin(testType);

            Assert.IsTrue(count == 126);
        }

        [TestMethod]
        public void PartTwo()
        {
            string type = "shiny gold";
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var handler = new BaggageHandler(input);
            int count = handler.CountBagsWithin(type);

            Assert.IsTrue(count == 30899);
        }
    }
}
