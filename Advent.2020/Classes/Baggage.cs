using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class Baggage
    {
        public int Count { get; } = 0;
        public string Type { get; } = null;
        
        public Baggage(string type, int count)
        {
            Type = type;
            Count = count;
        }

        public static Baggage Parse(string rule)
        {
            var arr = rule.Trim().Split(' ');

            int count = int.Parse(arr.First());
            string type = string.Join(" ", arr.Skip(1).Take(arr.Length - 2));

            return new Baggage(type, count);
        }

        public static Baggage operator *(Baggage b, int mult)
        {
            return new Baggage(b.Type, b.Count * mult);
        }

        public override string ToString()
        {
            return $"{Count} x {Type}";
        }
    }

    public class BaggageRule
    {
        private const string _SplitDelim = "bags contain";
        private const char _RuleDelim = ',';

        public Baggage Bag { get; } = null;

        public string Type => Bag.Type;
        public int Count => Contents.Sum(x => x.Count);

        public List<Baggage> Contents { get; } = new List<Baggage>();

        public BaggageRule(string line)
        {
            //< Split on 'bags contain' to get type -> rules
            var arr = line.Split(new string[] { _SplitDelim }, StringSplitOptions.None);

            //< Parse the type and generate the 'source' Bag
            string type = arr[0].Trim();
            this.Bag = new Baggage(type, 1);

            if (!arr[1].Contains("no other bags"))
            {
                //< Parse each rule (delimeted by comma)
                var rules = arr[1].Split(_RuleDelim);
                foreach (var rule in rules)
                {
                    this.Contents.Add(Baggage.Parse(rule));
                }
            }
        }

        public bool DirectlyContains(string type)
        {
            return Contents.Any(x => x.Type == type);
        }

        public bool DirectlyContains(IEnumerable<string> types)
        {
            return types.Any(x => this.DirectlyContains(x));
        }
    }

    public class BaggageHandler
    {
        public IEnumerable<string> Source { get; } = null;

        Dictionary<string, BaggageRule> RulesByType { get; } = null;

        public IEnumerable<BaggageRule> Rules => RulesByType.Values;

        public BaggageHandler(IEnumerable<string> rules)
        {
            Source = rules;
            //< Parse each source 'BaggageRule' into memory and index by bag type
            RulesByType = new Dictionary<string, BaggageRule>();
            foreach (var line in Source)
            {
                var rule = new BaggageRule(line);
                RulesByType.Add(rule.Type, rule);
            }
        }

        public int CountBagsThatCanContain(string type)
        {
            var types = GetCoveringTypes(type);
            return types.Count();
        }

        private IEnumerable<string> GetContainingTypes(string type)
        {
            return Rules.Where(x => x.DirectlyContains(type)).Select(x => x.Type);
        }

        private IEnumerable<string> GetCoveringTypes(string type)
        {
            //< Get the types that directly cover our type
            var direct = GetContainingTypes(type);

            var types = new HashSet<string>();
            var check = new Stack<string>();
            
            foreach (var bagType in direct)
            {
                check.Push(bagType);
                types.Add(bagType);
            }
            
            while (check.Count > 0)
            {
                var curr = check.Pop();

                var covering = GetContainingTypes(curr);
                var newTypes = covering.Where(x => !types.Contains(x));

                foreach (var newType in newTypes)
                {
                    types.Add(newType);
                    check.Push(newType);
                }
            }

            return types;
        }

        public int CountBagsWithin(string type)
        {
            var baseRule = RulesByType[type];

            var check = new Stack<Baggage>();
            foreach (var bag in baseRule.Contents)
            {
                check.Push(bag);
            }

            var components = new List<Baggage>();
            while (check.Count > 0)
            {
                var curr = check.Pop();
                var currRule = RulesByType[curr.Type];

                components.Add(curr);
                foreach (var bag in currRule.Contents)
                {
                    check.Push(bag * curr.Count);
                }
            }

            return components.Sum(x => x.Count);
        }
    }
}
