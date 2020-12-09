using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    using PreambleMap = Dictionary<long, List<Tuple<long, long>>>;

    public class XmasEncoder
    {
        public List<long> Inputs { get; } = null;

        public int Index { get; private set; } = 0;
        public int PreambleSize { get; private set; } = 25;

        public long CurrentValue => Inputs[Index];

        public List<long> Preamble => Inputs.GetRange(Index - PreambleSize, PreambleSize);
        public PreambleMap PreambleMap => GetPreambleMap(Preamble);

        public XmasEncoder(IEnumerable<long> inputs, int preambleSize)
        {
            this.Inputs = inputs.ToList();
            this.PreambleSize = preambleSize;
        }

        public long? CheckFirstInvalidValue()
        {
            //< Reset the starting index to right after the preamble
            Index = PreambleSize;
            //< Loop over inputs and see which doesn't have a match in the pre-amble map
            while (Index < Inputs.Count)
            {
                if (!PreambleMap.ContainsKey(CurrentValue))
                {
                    return CurrentValue;
                }
                Index++;
            }
            //< Didn't encounter any without matching sums in preamble -> return null
            return null;
        }

        public long? FindEncryptionWeakness(long sumTarget)
        {
            Index = 0;

            List<long> vals = null;
            bool bFoundMatch = false;
            //< Want to loop over indices looking for contiguous sets that sum to target
            while (Index < Inputs.Count && !bFoundMatch)
            {
                vals = new List<long>();
                for (int i = Index; i < Inputs.Count; i++)
                {
                    //< Current sum less than target -> add new value and progress
                    if (vals.Sum() < sumTarget)
                    {
                        vals.Add(Inputs[i]);
                    }
                    //< Match found -> break
                    else if (vals.Sum() == sumTarget)
                    {
                        bFoundMatch = true;
                        break;
                    }
                    //< Current sum is greater than target -> move on
                    else
                    {
                        break;
                    }
                }
                Index++;
            }

            if (bFoundMatch)
            {
                long weakness = vals.Min() + vals.Max();
                return weakness;
            }
            else
                return null;
        }

        public static PreambleMap GetPreambleMap(IEnumerable<long> preamble)
        {
            var map = new PreambleMap();
            for (int i = 0; i < preamble.Count(); i++)
            {
                long a = preamble.ElementAt(i);
                for (int j = 0; j < preamble.Count(); j++)
                {
                    if (i != j)
                    {
                        long b = preamble.ElementAt(j);
                        long sum = a + b;
                        if (!map.ContainsKey(sum))
                        {
                            map.Add(sum, new List<Tuple<long, long>>());
                        }
                        map[sum].Add(Tuple.Create(a, b));
                    }
                }
            }
            return map;
        }
    }
}
