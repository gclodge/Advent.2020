using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class JoltageAdapter
    {
        public int Rating { get; } = 0;
        public int MinRating => Rating - 3;

        public JoltageAdapter(int rating)
        {
            Rating = rating;
        }

        public bool ConnectsTo(JoltageAdapter other)
        {
            int diff = (this.Rating - other.Rating);
            return (diff > 0 && diff <= 3);
        }
    }

    public class JoltageArray
    {
        public List<JoltageAdapter> Adapters { get; } = null;

        public JoltageAdapter ChargingOutlet { get; } = new JoltageAdapter(0);
        public JoltageAdapter BuiltIn => new JoltageAdapter(Adapters.Max(x => x.Rating));

        public JoltageArray(List<int> input)
        {
            //< Get the (sorted) array of JoltageAdapters
            Adapters = input.Select(x => new JoltageAdapter(x)).OrderBy(x => x.Rating).ToList();
        }

        public Dictionary<int, int> GetAdapterArrayDifferences()
        {
            //< Instantiate the result map with the 'built-in' difference of +3 pre-populated
            var resMap = new Dictionary<int, int>()
            {
                { 3, 1 },
            };

            JoltageAdapter curr = ChargingOutlet;
            for (int i = 0; i < Adapters.Count; i++)
            {
                if (Adapters[i].ConnectsTo(curr))
                {
                    int dJ = Adapters[i].Rating - curr.Rating;
                    if (!resMap.ContainsKey(dJ))
                    {
                        resMap.Add(dJ, 0);
                    }
                    resMap[dJ]++;
                    curr = Adapters[i];
                }
            }

            return resMap;
        }

        public long CountPossibleCombinations()
        {
            var adaptsWithCharger = Adapters;
            adaptsWithCharger.Insert(0, new JoltageAdapter(0));

            var steps = new long[adaptsWithCharger.Count];
            steps[0] = 1;

            //< Skipping the first (already set to 1) and last (zero connections)
            foreach (var i in Enumerable.Range(1, Adapters.Count - 1))
            {
                foreach (var j in Enumerable.Range(0, i))
                {
                    if (adaptsWithCharger[i].Rating - adaptsWithCharger[j].Rating <= 3)
                    {
                        steps[i] += steps[j];
                    }
                }
            }

            return steps.Last();
        }
    }
}
