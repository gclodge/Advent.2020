using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Advent._2020.Classes;

namespace Advent._2020.Tests.DailyTests
{
    [TestClass]
    public class Day21 : IDailyTests
    {
        public int Number => 21;

        public string InputFile => TestHelper.GetInputFile(this);
        public string TestFile => TestHelper.GetTestFile(this);

        [TestMethod]
        public void Test_KnownAllergens()
        {
            var input = Helpers.FileHelper.ParseFile(TestFile);
            var detector = new AllergenDetector(input);

            int count = detector.CountNonAllergenOccurrence();
            Assert.IsTrue(count == 5);

            string danger = detector.GetCanonicalDangerousIngredientList();
            Assert.IsTrue(danger == "mxmxvkd,sqjhc,fvjkl");
        }

        [TestMethod]
        public void PartOne()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var detector = new AllergenDetector(input);

            int count = detector.CountNonAllergenOccurrence();
            Assert.IsTrue(count == 2078);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = Helpers.FileHelper.ParseFile(InputFile);
            var detector = new AllergenDetector(input);

            string danger = detector.GetCanonicalDangerousIngredientList();
            Assert.IsTrue(danger == "lmcqt,kcddk,npxrdnd,cfb,ldkt,fqpt,jtfmtpd,tsch");
        }
    }
}
