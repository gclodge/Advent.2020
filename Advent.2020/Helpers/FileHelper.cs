using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Helpers
{
    public class FileHelper
    {
        public static IEnumerable<string> ParseFile(string file)
        {
            var res = new List<string>();
            using (var sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    res.Add(sr.ReadLine());
                }
            }
            return res;
        }

        public static IEnumerable<T> ParseFile<T>(string file, Func<string, T> deserialize)
        {
            return ParseFile(file).Select(x => deserialize(x));
        }
    }
}
