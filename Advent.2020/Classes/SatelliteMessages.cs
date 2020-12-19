using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using MoreLinq;

namespace Advent._2020.Classes
{
    public class Satellite
    {
        public string PlaceholderC => "c";
        public string PlaceholderD => "d";

        public List<string> Source { get; } = null;
        public int SplitIndex => Source.IndexOf("");

        public List<string> Messages { get; } = null;
        public Dictionary<int, SatelliteRule> Rules { get; private set; } = null;

        public Dictionary<int, string> Simplified { get; private set; } = null;

        public Satellite(IEnumerable<string> input, bool isPartTwo = false)
        {
            //< Retain the source in-memory as a List<string>
            this.Source = input.ToList();
            //< Parse the rules into memory
            this.Rules = ParseRules(Source.Take(SplitIndex));
            //< Handle the complete malarky of part two
            if (isPartTwo) //< Shoutout to /u/Floydianx33 for this
            {
                //< Set these two rules to placeholder values so we can alter them after simplification
                Rules[8].SetValue("c");
                Rules[11].SetValue("d");
            }
            //< Get the 'simplified/solved' rules so we can just match against messages
            this.Simplified = SimplifyRules(Rules);
            //< Parse the messages into a collection for later validation
            this.Messages = Source.Skip(SplitIndex + 1).ToList();
        }

        public int CountMatchingMessages(int ruleIndex, bool isPartTwo = false)
        {
            //< Handle user fuckery
            if (!Simplified.ContainsKey(ruleIndex))
                throw new IndexOutOfRangeException($"No matching rule with given index: {ruleIndex}");

            //< Get the rule from the map
            string rule = Simplified[ruleIndex];

            //< Handle the absolute malarky of part two
            if (isPartTwo) //< Shoutout to /u/Floydianx33 for this
            {
                //< Can be shown that: Rule8  ==> (Rule42)+  [one or more]
                //< Then, (allegedly) it be shown that: Rule11 ==>  Rule42{k}Rule31{k} where k >= 1 [balanced group]
                rule = rule.Replace("c", $"(?:{Simplified[42]})+")
                           .Replace("d", $"(?>(?:{Simplified[42]})(?<DEPTH>)|(?:{Simplified[31]})(?<-DEPTH>))+(?(DEPTH)(?!))");
            }
            
            //< NB :: '^' indicates start of string and '$' is end of string
            //< NB :: So here we're looking for an explicit match of the rule's simplified regex string
            //< NB :: RegexOptions.Compiled makes each execution faster at a higher up-front compilation cost
            var ruleRegex = new Regex($"^{rule}$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

            //< Return the count of messages that actually match the rule
            return Messages.Count(ruleRegex.IsMatch); //< Didn't realize you could skip the lambda if it's a Func<T, bool>
        }

        private static Dictionary<int, SatelliteRule> ParseRules(IEnumerable<string> lines)
        {
            var rules = new Dictionary<int, SatelliteRule>();
            foreach (var line in lines)
            {
                var rule = new SatelliteRule(line);
                rules.Add(rule.Index, rule);
            }
            return rules;
        }

        private static Dictionary<int, string> SimplifyRules(Dictionary<int, SatelliteRule> rules)
        {
            //< Create a HashSet to map which rule indexs are 'simplified' already
            var complete = new HashSet<int>();
            foreach (var kvp in rules)
            {
                //< To start, only rules with length of one (a/b/etc) are simplified
                if (kvp.Value.Length == 1)
                    complete.Add(kvp.Key);
            }

            //< Parse the source rules into an editable map
            var dict = rules.ToDictionary(x => x.Key, x => x.Value.Value);

            //< Loop until each rule has been completely simplified
            while (complete.Count != rules.Count)
            {
                foreach (int i in Enumerable.Range(0, dict.Keys.Count))
                {
                    //< Grab the rule index and check if it's already complete
                    int idx = dict.Keys.ElementAt(i);
                    if (complete.Contains(idx))
                        continue;

                    string val = dict[idx];
                    bool remainder = false;
                    //< NB :: \d matches any digits [0, 9] put \d+ is a 'greedy' match (1 to unlimited times)
                    dict[idx] = Regex.Replace(val, @"\d+", match =>
                    {
                        //< Get the rule key for this match
                        int currKey = int.Parse(match.Value);
                        //< If the matched key has been completed, return that value
                        if (complete.Contains(currKey))
                        {
                            return $"(?:{dict[currKey]})";
                        }
                        else //< Otherwise, we still have a remainder and should return the match value
                        {
                            remainder = true;
                            return match.Value;
                        }
                    });

                    if (!remainder)
                    {
                        complete.Add(idx);
                    }
                }
            }

            return dict;
        }
    }

    public class SatelliteRule
    {
        public int Index { get; } = 0;
        public string Value { get; private set; } = null;

        public int Length => Value.Length;

        public SatelliteRule(string line)
        {
            //< Split the line by ':' to get the index / rule pair
            var arr = line.Split(':');
            //< Retain the index as we need it for reference
            this.Index = int.Parse(arr[0]);
            //< Parse the actual rule string
            var val = arr[1];
            this.Value = (val[1] == '"') ? val.Substring(2, 1) : val.Substring(1);
        }

        public void SetValue(string val)
        {
            this.Value = val;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
