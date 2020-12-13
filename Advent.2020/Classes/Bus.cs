using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class Bus
    {
        public int ID { get; } = -1;
        public int Index { get; } = -1;

        public int ModulusValue => ID - (ID % Index);

        public Bus(string str, int idx)
        {
            this.ID = int.Parse(str);
            this.Index = idx;
        }

        public long GetNearestTimeAfter(long timestamp)
        {
            int n = (int)Math.Ceiling((double)timestamp / (double)ID);
            return n * ID;
        }

        public long GetWaitTime(long timestamp)
        {
            return GetNearestTimeAfter(timestamp) - timestamp;
        }

        public override string ToString()
        {
            return $"Bus:{ID}";
        }
    }

    public class BusManager
    {
        public List<Bus> Busses { get; private set; } = null;

        public int MinTimestamp { get; } = 0;

        public BusManager(IEnumerable<string> input)
        {
            //< First line indicates the 'minimum timestamp' for departure
            this.MinTimestamp = int.Parse(input.First());
            //< Following line is a comma-delimted collection of busses -> parse all non-'x' busses
            this.Busses = new List<Bus>();
            var vals = input.Last().Split(',');
            for (int i = 0; i < vals.Count(); i++)
            {
                if (vals.ElementAt(i) != "x")
                {
                    //< Want to retain the value and the relative index (for part two) of the bus
                    Busses.Add(new Bus(vals.ElementAt(i), i));
                }
            }
        }

        public Bus GetEarliestBus()
        {
            return Busses.OrderBy(bus => bus.GetNearestTimeAfter(MinTimestamp)).First();
        }

        public long GetFirstContinuousDepartureTime()
        {
            //< This took far, far, far too long to figure out and was way too specific for my tastes
            //< Ended up googling CRT (Chinese Remainder Theorem) and a bunch of other ways of solving given the modular constraints

            long time = Busses.First().ID;
            long increment = time;

            for (int i = 1; i < Busses.Count; i++)
            {
                var cur = Busses[i].ID;
                var mod = cur - (Busses[i].Index % cur);

                while (time % cur != mod)
                {
                    time += increment;
                }
                increment = MathNet.Numerics.Euclid.LeastCommonMultiple(increment, cur);
            }

            return time;
        }

        public static bool CheckContinuity(long time, IEnumerable<Bus> busses)
        {
            return busses.All(bus => bus.GetWaitTime(time) == bus.Index);
        }
    }
}
