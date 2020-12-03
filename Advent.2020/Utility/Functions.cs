using System;
using System.Linq;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Utility
{
    public class Functions
    {
        public static int? FindRecordsThatSumTo(IEnumerable<int> recs, int sumValue, int len)
        {
            foreach (var perm in GetPermutations(recs.OrderBy(x => x), len))
            {
                if (perm.Sum() == sumValue)
                {
                    int val = perm.First();
                    for (int i = 1; i < perm.Count(); i++)
                    {
                        val *= perm.ElementAt(i);
                    }
                    return val;
                }
            }

            return null;
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)),(t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static Vector<double> GetVector(double x, double y)
        {
            return CreateVector.Dense(new double[] { x, y });
        }
    }
}
