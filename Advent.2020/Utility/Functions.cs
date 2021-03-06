﻿using System;
using System.Linq;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Utility
{
    public enum HalfSplit
    {
        Top,
        Bottom,
        None
    }

    public static class Functions
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

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static List<IEnumerable<T>> GetPossibleCombinations<T>(IEnumerable<T> vals)
        {
            var combos = new List<IEnumerable<T>>();

            int n = vals.Count();
            while (n > 0)
            {
                var combs = Combinations(vals, n);
                combos.AddRange(combs);
                n--;
            }

            return combos;
        }

        public static ulong Sum(this IEnumerable<ulong> elements)
        {
            ulong sum = 0;
            foreach (var ele in elements)
            {
                sum += ele;
            }
            return sum;
        }

        public static Vector<double> GetVector(double x, double y)
        {
            return CreateVector.Dense(new double[] { x, y });
        }

        public static IEnumerable<IEnumerable<string>> SplitByByElement(IEnumerable<string> recs, string element)
        {
            var groups = new List<List<string>>();
            var currGroup = new List<string>();
            for (int i = 0; i < recs.Count(); i++)
            {
                string item = recs.ElementAt(i);
                if (item != element)
                {
                    currGroup.Add(item);
                }
                else
                {
                    groups.Add(currGroup);
                    currGroup = new List<string>();
                }
            }

            if (currGroup.Count > 0)
            {
                groups.Add(currGroup);
            }

            return groups;
        }

        public static IEnumerable<T> SplitInHalf<T>(IEnumerable<T> recs, HalfSplit split)
        {
            int halfCount = recs.Count() / 2;
            switch (split)
            {
                case HalfSplit.Top:
                    return recs.Skip(halfCount);
                case HalfSplit.Bottom:
                    return recs.Take(halfCount);
                default:
                    return recs;
            }
        }

        public static IEnumerable<T[]> CartesianProduct<T>(IList<T> items, int repeat)
        {
            var total = (int)Math.Pow(items.Count, repeat);
            var res = new T[repeat];
            for (var i = 0; i != total; i++)
            {
                var n = i;
                for (var j = repeat - 1; j >= 0; j--)
                {
                    res[j] = items[n % items.Count];
                    n /= items.Count;
                }
                yield return res;
            }
        }

        public static List<int> GetAllIndicesOf(string exp, HashSet<char> targets)
        {
            var indices = new List<int>();
            foreach (int i in Enumerable.Range(0, exp.Length))
            {
                if (targets.Contains(exp[i]))
                {
                    indices.Add(i);
                }
            }
            return indices;
        }
    }
}
