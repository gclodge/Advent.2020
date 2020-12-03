using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day03 : IDailyTests
    {
        public int Number => 3;

        public string InputFile => TestHelper.GetInputFile(this);

        [TestMethod]
        public void PartOne()
        {
            var rows = Helpers.FileHelper.ParseFile(InputFile);

            var slope = Utility.Functions.GetVector(3, 1);

            var grid = new Classes.TreeGrid(rows);
            grid.TraverseAndCountTrees(slope);

            Assert.IsTrue(grid.Trees.Count == 228);
        }

        [TestMethod]
        public void PartTwo()
        {
            var slopes = new List<Vector<double>>()
            {
                Utility.Functions.GetVector(1, 1),
                Utility.Functions.GetVector(3, 1),
                Utility.Functions.GetVector(5, 1),
                Utility.Functions.GetVector(7, 1),
                Utility.Functions.GetVector(1, 2),
            };

            var rows = Helpers.FileHelper.ParseFile(InputFile);
            var grid = new Classes.TreeGrid(rows);

            var results = new List<int>();
            foreach (var slope in slopes)
            {
                grid.TraverseAndCountTrees(slope);

                results.Add(grid.Trees.Count);
            }

            double val = 1;
            foreach (var res in results)
            {
                val *= res;
            }

            Assert.IsTrue(val == 6818112000);
        }
    }
}
