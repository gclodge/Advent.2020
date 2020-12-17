using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day17 : IDailyTests
    {
        public int Number => 17;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_GetOffsetsForDimension()
        {
            var dims = new int[] { 2, 3, 4 };
            foreach (var dim in dims)
            {
                var offsets = ConwayCube.GetOffsets(dim).ToList();
                int targetCount = (int)Math.Pow(3, dim);

                Assert.IsTrue(offsets.Count() == targetCount);
            }
        }

        [TestMethod]
        public void Test_GetNeighbours3D()
        {
            var cube = new ConwayCube(new int[] { 0, 0, 0 });
            var neighs = cube.GetNeighbours().ToList();

            int targetCount = (int)Math.Pow(3, cube.Dimensions) - 1;
            Assert.IsTrue(neighs.Count == targetCount);
        }

        [TestMethod]
        public void Test_KnownCubes()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);
            var controller = new ConwayController(input);
            controller.SimulateSteps(6);

            Assert.IsTrue(controller.NumActive == 112);
        }

        [TestMethod]
        public void Test_KnownCubes_4D()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);
            var controller = new ConwayController(input, dimensions: 4);
            controller.SimulateSteps(6);

            Assert.IsTrue(controller.NumActive == 848);
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var controller = new ConwayController(input);
            controller.SimulateSteps(6);

            Assert.IsTrue(controller.NumActive == 293);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var controller = new ConwayController(input, dimensions: 4);
            controller.SimulateSteps(6);

            Assert.IsTrue(controller.NumActive == 1816);
        }
    }
}
