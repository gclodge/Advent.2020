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
    public class Day05 : IDailyTests
    {
        public int Number => 5;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        public static readonly List<Tuple<string, int>> KnownIDs = new List<Tuple<string, int>>()
        {
            Tuple.Create("BFFFBBFRRR", 567),
            Tuple.Create("FFFBBBFRRR", 119),
            Tuple.Create("BBFFBBFRLL", 820)
        };

        [TestMethod]
        public void Test_KnownIDs()
        {
            foreach (var tup in KnownIDs)
            {
                var id = BoardingPass.GetSeatID(tup.Item1);

                Assert.IsTrue(id.ID == tup.Item2);
            }
        }

        [TestMethod]
        public void PartOne()
        {
            var inputLines = Helpers.FileHelper.ParseFile(InputFile);
            //< Parse each input line into a final 'SeatID'
            var seatIDs = inputLines.Select(x => BoardingPass.GetSeatID(x));
            //< Grab the largest 'SeatID.ID' encountered
            var maxID = seatIDs.Max(x => x.ID);
            //< Ensure we match that known result
            Assert.IsTrue(maxID == 878);
        }

        [TestMethod]
        public void PartTwo()
        {
            //< Looking for IDs that are missing.. but also have existing +1/-1 seats


            var inputLines = Helpers.FileHelper.ParseFile(InputFile);
            //< Parse each input line into a final 'SeatID'
            var seatIDs = inputLines.Select(x => BoardingPass.GetSeatID(x));
            //< Get a map of all existing ID -> SeatID pairs
            var seatMap = seatIDs.ToDictionary(x => x.ID, x => x);

            var possible = new List<SeatID>();
            for(int x = 0; x < BoardingPass.Rows.Count(); x++)
            {
                for (int y = 0; y < BoardingPass.Columns.Count(); y++)
                {
                    var seatID = new SeatID(x, y);

                    if (!seatMap.ContainsKey(seatID.ID))
                    {
                        int prevID = seatID.ID - 1;
                        int nextID = seatID.ID + 1;

                        if (seatMap.ContainsKey(prevID) && seatMap.ContainsKey(nextID))
                        {
                            possible.Add(seatID);
                        }
                    }
                }
            }

            Assert.IsTrue(possible.Single().ID == 504);
        }


    }
}
