using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day04 : IDailyTests
    {
        public int Number => 4;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownPassports()
        {
            var passFile = new Classes.PassportFile(TestFile);

            var goodPorts = passFile.Passports.Where(x => x.HasRequiredFields).ToList();

            Assert.IsTrue(goodPorts.Count == 2);
        }

        [TestMethod]
        public void PartOne()
        {
            var passFile = new Classes.PassportFile(InputFile);

            var goodPorts = passFile.Passports.Where(x => x.HasRequiredFields).ToList();

            Assert.IsTrue(goodPorts.Count() == 256);
        }

        [TestMethod]
        public void Test_CheckDigits()
        {
            const int min = 1920;
            const int max = 2002;
            const int cnt = 4;

            //< 'Good' cases
            Assert.IsTrue(Classes.Passport.CheckDigits("1921", min, max, cnt));
            Assert.IsTrue(Classes.Passport.CheckDigits("1999", min, max, cnt));
            Assert.IsTrue(Classes.Passport.CheckDigits("2001", min, max, cnt));

            //< 'Bad' cases
            Assert.IsFalse(Classes.Passport.CheckDigits("1919", min, max, cnt));
            Assert.IsFalse(Classes.Passport.CheckDigits("2003", min, max, cnt));
            Assert.IsFalse(Classes.Passport.CheckDigits("4", min, max, cnt));
            Assert.IsFalse(Classes.Passport.CheckDigits("4444444", min, max, cnt));
            Assert.IsFalse(Classes.Passport.CheckDigits("Pant", min, max, cnt));
        }

        [TestMethod]
        public void Test_HairColors()
        {
            var knownColors = new List<Tuple<string, bool>>()
            {
                Tuple.Create("#123abc", true),
                Tuple.Create("#123abz", false),
                Tuple.Create("123abc", false),
            };

            foreach (var tup in knownColors)
            {
                Assert.IsTrue(Classes.Passport.CheckHairColor(tup.Item1) == tup.Item2);
            }
        }

        [TestMethod]
        public void Test_KnownValidPassports()
        {
            var validFile = new Classes.PassportFile(TestHelper.GetTestFile(this, "Test.Valid"));
            Assert.IsTrue(validFile.Passports.All(p => p.IsValid));

            var invalidFile = new Classes.PassportFile(TestHelper.GetTestFile(this, "Test.Invalid"));
            Assert.IsTrue(invalidFile.Passports.All(p => !p.IsValid));
        }

        [TestMethod]
        public void PartTwo()
        {
            var passFile = new Classes.PassportFile(InputFile);

            var goodPorts = passFile.Passports.Where(x => x.IsValid).ToList();

            Assert.IsTrue(goodPorts.Count == 198);
        }
    }
}
