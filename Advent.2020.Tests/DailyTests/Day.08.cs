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
    public class Day08 : IDailyTests
    {
        public int Number => 8;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownProgram()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var handheld = new HandheldGameSystem(input);
            var res = handheld.Run();

            Assert.IsTrue(res.Accumulator == 5);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var handheld = new HandheldGameSystem(input);

            var indicesToTry = handheld.GetIndicesToTry(new string[] { "nop", "jmp" });


            var res = handheld.Run();

            Assert.IsTrue(res.Accumulator == 1782);
        }

        [TestMethod]
        public void Test_KnownProgramWithSwap()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);

            var handheld = new HandheldGameSystem(input);
            var res = handheld.Run();

            Assert.IsTrue(res.Accumulator == 5);
        }

        [TestMethod]
        public void PartTwo()
        {
            //< Parse the data into memory - generate an initial Handheld
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var handheld = new HandheldGameSystem(input);
            //< Get the indices we may want to alter
            var indicesToTry = handheld.GetIndicesToTry(new string[] { "nop", "jmp" });

            int goodVal = -1;
            foreach (var idx in indicesToTry)
            {
                //< Reset the HandheldGameSystem back to 'normal' (the lazy way)
                handheld = new HandheldGameSystem(input);
                //< Run it with the swap
                var res = handheld.RunWithSwap(idx);
                if (!res.WasInfiniteLoop)
                {
                    goodVal = res.Accumulator;
                    break;
                }
            }

            Assert.IsTrue(goodVal == 797);
        }
    }
}
