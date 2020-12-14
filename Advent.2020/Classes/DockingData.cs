using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Advent._2020.Utility;

namespace Advent._2020.Classes
{
    public class BitValue
    {
        public const int NumBits = 36;
        public int StartIndex => NumBits - 1;

        public ulong Index { get; } = 0;
        public ulong Source { get; } = 0;
        public bool[] Bits { get; } = null;

        public BitValue(string[] line, bool v2 = false)
        {
            this.Index = ParseIndex(line[0]);
            this.Source = ulong.Parse(line[1]);
            //< Parse the individual bits from the source value (NB: stored as LSB -> MSB, must reverse)
            var arr = v2 ? new BitArray(BitConverter.GetBytes(Index)) : new BitArray(BitConverter.GetBytes(Source));
            //< Populate our bool[] of the 36 bits we want
            this.Bits = new bool[NumBits];
            for (int i = 0; i < NumBits; i++)
            {
                this.Bits[i] = arr[StartIndex - i];
            }
        }

        private ulong ParseIndex(string s)
        {
            var val = s.Substring(4).Replace("]", "");
            return ulong.Parse(val);
        }
    }

    public class BitMask
    {
        public string Source { get; } = null;

        public HashSet<byte> Wildcards { get; } = null;
        public Dictionary<byte, bool> Mask { get; } = null;

        public BitMask(string[] line)
        {
            this.Source = line.Last();
            this.Mask = new Dictionary<byte, bool>();
            this.Wildcards = new HashSet<byte>();

            for (byte i = 0; i < Source.Length; i++)
            {
                if (Source[i] == 'X')
                {
                    Wildcards.Add(i);
                }
                else
                {
                    int val = byte.Parse(Source[i].ToString());
                    Mask.Add(i, val == 1);
                }
            }
        }

        public ulong GetValue(bool[] bits, int startIndex)
        {
            ulong val = 0;
            for (byte i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    ulong curr = GetValue(startIndex - i);
                    val += curr;
                }
            }
            return val;
        }

        public ulong GetValue(int index)
        {
            return (ulong)Math.Pow(2, index);
        }

        public ulong GetFinalValue(BitValue bv)
        {
            var bits = new bool[bv.Bits.Length];
            for (byte i = 0; i < bv.Bits.Length; i++)
            {
                bits[i] = (Mask.ContainsKey(i)) ? Mask[i] : bv.Bits[i];
            }
            return GetValue(bits, bv.StartIndex);
        }

        public List<ulong> GetFinalAddresses(BitValue bv)
        {
            int numPerms = (int)Math.Pow(2, Wildcards.Count);

            //< Get the 'intermediate' bits (using version 2 mask rules)
            var interBits = GetIntermediateBits(bv);
            //< Get the 'base value' from those bits (where all wild cards are 0)
            ulong baseVal = GetValue(interBits, bv.StartIndex);

            //< Get the actual numerical values of each wild card when set (1)
            var wildMap = new Dictionary<byte, ulong>();
            for (byte i = 0; i < Wildcards.Count; i++)
            {
                byte index = Wildcards.ElementAt(i);
                wildMap.Add(i, GetValue(bv.StartIndex - index));
            }

            var adds = new List<ulong>() { baseVal };
            //< Order the wildcard values first, so we have like (1, 32) for the first case and (1, 2, 8) for the second
            var wildVals = wildMap.Values.OrderBy(x => x).ToList();
            //< Get all the possible combinations of elements in this collection of wild card values (skips the all zero case so returns (2^n) - 1 combos)
            var combs = Functions.GetPossibleCombinations(wildVals);

            foreach (var comb in combs)
            {
                ulong val = baseVal + comb.Sum();
                adds.Add(val);
            }

            return adds;
        }

        private bool[] GetIntermediateBits(BitValue bv)
        {
            var interBits = new bool[bv.Bits.Length];
            for (byte i = 0; i < interBits.Length; i++)
            {
                if (Wildcards.Contains(i))
                {
                    interBits[i] = false; //< Setting false to 'take' the 0 value first
                }
                else if (Mask.ContainsKey(i))
                {
                    //< If the mask is 1 -> set to 1, else retain original value
                    interBits[i] = Mask[i] ? true : bv.Bits[i];
                }
                else
                    interBits[i] = bv.Bits[i];
            }
            return interBits;
        }
    }

    public class Docker
    {
        public List<string[]> Data { get; } = null;

        public BitMask Mask { get; private set; } = null;
        public Dictionary<ulong, ulong> Memory { get; private set; } = null;

        public Docker(IEnumerable<string> input)
        {
            //< Parse the input data into string[] by splitting on the uniform " = " delimeter
            this.Data = input.Select(x => x.Split(new string[] { " = " }, StringSplitOptions.None))
                             .ToList();
        }

        public ulong GetSumInMemory()
        {
            //< Hacking a sum loop in since ulongs arent supported by Sum()
            ulong sum = 0;
            foreach (var kvp in Memory)
            {
                sum += kvp.Value;
            }
            return sum;
        }

        public ulong Initialize(bool v2 = false)
        {
            this.Memory = new Dictionary<ulong, ulong>();
            //< Iterate over each input line in the source data
            foreach (var line in Data)
            {
                //< If starts with Mask -> update current BitMask
                if (line[0].StartsWith("mask"))
                {
                    this.Mask = new BitMask(line);
                }
                else //< Otherwise, is a set command
                {
                    //< Get the BitValue from the line
                    var bv = new BitValue(line, v2);
                    if (v2) //< If 'Version 2' - must account for wild cards and assign to multiple addresses
                    {
                        AssignValueToAddresses(bv);
                    }
                    else    //< If 'Version 1' - get final value with mask and assign
                    {
                        AssignValue(bv);
                    } 
                }
            }

            return GetSumInMemory();
        }

        private void AssignValue(BitValue bv)
        {
            //< Ensure this memory address exists
            if (!Memory.ContainsKey(bv.Index))
            {
                Memory.Add(bv.Index, 0);
            }
            //< Get the final value (using current BitMask) and assign it
            Memory[bv.Index] = Mask.GetFinalValue(bv);
        }

        private void AssignValueToAddresses(BitValue bv)
        {
            var addresses = Mask.GetFinalAddresses(bv);
            foreach (var add in addresses)
            {
                if (!Memory.ContainsKey(add))
                {
                    Memory.Add(add, 0);
                }
                Memory[add] = bv.Source;
            }
        }
    }
}
