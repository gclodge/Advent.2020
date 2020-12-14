using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day14 : IDailyTests
    {
        public int Number => 14;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownMask()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var docker = new Docker(input);
            var val = docker.Initialize();

            Assert.IsTrue(val == 165);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var docker = new Docker(input);
            var val = docker.Initialize();

            Assert.IsTrue(val == 9879607673316);
        }

        [TestMethod]
        public void Test_KnownAddresses()
        {
            var input = Helpers.FileHelper.ParseFile(TestHelper.GetTestFile(this, "Test2"));

            var docker = new Docker(input);
            var val = docker.Initialize(v2: true);

            Assert.IsTrue(val == 208);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);

            var docker = new Docker(input);
            var val = docker.Initialize(v2: true);

            Assert.IsTrue(val == 3435342392262);
        }
    }
}
