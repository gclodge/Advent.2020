using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class HandheldArg
    {
        public string Type { get; set; } = null;
        public int Value { get; } = 0;

        public HandheldArg(string ln)
        {
            //< Split the input line by space
            var arr = ln.Trim().Split(' ');
            //< Grab the type and value
            this.Type = arr[0];
            this.Value = int.Parse(arr[1]);
        }
    }

    public class HandheldResult
    {
        public int Accumulator { get; } = 0;
        public bool WasInfiniteLoop { get; } = false;

        public HandheldResult(int acc, bool isInfinite)
        {
            this.Accumulator = acc;
            this.WasInfiniteLoop = isInfinite;
        }
    }

    public class HandheldGameSystem
    {
        public List<HandheldArg> Arguments { get; } = null;

        public int Index { get; private set; } = 0;
        public int Accumulator { get; private set; } = 0;

        public Dictionary<int, int> Calls { get; private set; } = null;

        public HandheldGameSystem(IEnumerable<string> args)
        {
            this.Arguments = args.Select(x => new HandheldArg(x)).ToList();
        }

        public void Reset()
        {
            Index = 0;
            Accumulator = 0;

            Calls = new Dictionary<int, int>();
        }

        public HandheldResult Run()
        {
            //< Ensure everything is at its ready state
            Reset();
            //< Boolean flag to use if we detect an infinite loop
            bool isInfinite = false;
            //< Loop over them arguments, boah
            while (Index < Arguments.Count)
            {
                if (!Calls.ContainsKey(Index))
                {
                    //< Add the call map
                    Calls.Add(Index, 1);
                    //< Execute the argument
                    ExecuteArgument(Arguments[Index]);
                }
                else
                {
                    //< We've hit this index (command) before -> break
                    isInfinite = true;
                    break;
                }
            }
            //< Return the results, boah
            return new HandheldResult(Accumulator, isInfinite);
        }

        public HandheldResult RunWithSwap(int idx)
        {
            //< Swap the argument at the given index
            SwapIndex(idx);
            //< Run the loop
            return Run();
        }

        public void ExecuteArgument(HandheldArg arg)
        {
            switch (arg.Type)
            {
                case "nop":
                    Index += 1;
                    return;
                case "acc":
                    Accumulator += arg.Value;
                    Index += 1;
                    return;
                case "jmp":
                    Index += arg.Value;
                    return;
            }
        }

        public IEnumerable<int> GetIndicesToTry(IEnumerable<string> types)
        {
            HashSet<int> vals = new HashSet<int>();
            for (int i = 0; i < Arguments.Count; i++)
            {
                if (types.Contains(Arguments[i].Type))
                {
                    vals.Add(i);
                }
            }
            return vals;
        }

        public void SwapIndex(int idx)
        {
            switch (Arguments[idx].Type)
            {
                case "jmp":
                    Arguments[idx].Type = "nop";
                    break;
                case "nop":
                    Arguments[idx].Type = "jmp";
                    break;
                default:
                    return;
            }
        }
    }
}
