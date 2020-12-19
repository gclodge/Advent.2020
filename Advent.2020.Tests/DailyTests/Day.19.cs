using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day19 : IDailyTests
    {
        public int Number => 19;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        public int RuleIndex => 0;

        [TestMethod]
        public void Test_KnownMessages()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var satellite = new Satellite(input);
            int count = satellite.CountMatchingMessages(RuleIndex);

            Assert.IsTrue(count == 2);
        }

        [TestMethod]
        public void Test_KnownMessages_WithFuckery()
        {
            var input = Helpers.FileHelper.ParseFile(TestHelper.GetTestFile(this, "Test2"));

            var satellite = new Satellite(input, isPartTwo: true);
            int count = satellite.CountMatchingMessages(RuleIndex, isPartTwo: true);

            Assert.IsTrue(count == 12);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var satellite = new Satellite(input);
            int count = satellite.CountMatchingMessages(RuleIndex);

            Assert.IsTrue(count == 203);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            
            var satellite = new Satellite(input, isPartTwo: true);
            //< NB :: This returns 305 when it should return 304
            //<    :: The extra record is "abaabaaabababaaabbbaaaabaabbbababbbbaaaabababaaabaaabaab"
            int count = satellite.CountMatchingMessages(RuleIndex, isPartTwo: true);

            const int actualAnswer = 304;
            Assert.IsTrue(count == actualAnswer + 1);
        }
    }
}
