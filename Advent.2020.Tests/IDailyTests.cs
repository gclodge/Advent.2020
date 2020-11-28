using Microsoft.VisualStudio.TestTools.UnitTesting;


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent._2020.Tests
{
    public enum Outcome
    {
        Ok,
        Fucked,
        Default
    }

    public interface IDailyTests
    {
        int Number { get; }

        [TestMethod]
        void PartOne();

        [TestMethod]
        void PartTwo();
    }
}
