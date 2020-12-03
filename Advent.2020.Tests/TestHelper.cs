using System.IO;
using System.Linq;
using System.Reflection;

namespace Advent._2020.Tests
{
    public class TestHelper
    {
        static Assembly Self = Assembly.GetExecutingAssembly();

        private const string _TestDir = "TestData";
        public static string TestDir => GetTestDirectoryRoot(_TestDir);

        public static string GetTestDirectoryRoot(string relativePath = null)
        {
            string[] hypotheticals = new[]
            {
                Path.Combine(Path.GetDirectoryName(Self.Location), @"..\..\.."),
                Path.Combine(Path.GetDirectoryName(Self.Location), @"..\..\..\..")
            };

            if (relativePath != null)
            {
                hypotheticals = hypotheticals.Select(x => Path.Combine(x, relativePath)).ToArray();
            }

            var exists = hypotheticals.Where(x => File.Exists(x) || Directory.Exists(x)).FirstOrDefault();
            return exists ?? null;
        }

        public static string GetInputFile(IDailyTests dt)
        {
            return GetFile(dt, "Input");
        }

        public static string GetTestFile(IDailyTests dt)
        {
            return GetFile(dt, "Test");
        }

        private static string GetFile(IDailyTests dt, string kernel)
        {
            return Path.Combine(TestDir, $"Day.{dt.Number.ToString().PadLeft(2, '0')}.{kernel}.txt");
        }
    }
}
