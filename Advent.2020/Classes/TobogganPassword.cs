using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public enum ValidationType
    {
        Range,
        Position,
        Unset
    }

    public class TobogganPolicy
    {
        public string Source { get; } = null;

        public int Min { get; } = int.MaxValue;
        public int Max { get; } = int.MinValue;

        public int MinPosition => Min - 1;
        public int MaxPosition => Max - 1;

        public char Value { get; } = default(char);

        public TobogganPolicy(string policy)
        {
            Source = policy;

            var data = policy.Split(' ');
            //< Grab the 'value' - the last character
            this.Value = data.Last().Single();
            //< Parse the 'range' - min/max occurrence of the 'Value'
            var range = data.First().Split('-');
            this.Min = int.Parse(range.First());
            this.Max = int.Parse(range.Last());
        }
    }

    public class TobogganPassword
    {
        private const char _Delim = ':';

        public string Password { get; } = null;
        public TobogganPolicy Policy { get; } = null;

        public ValidationType Type { get; } = ValidationType.Unset;

        public bool IsGood => ValidatePassword();

        public TobogganPassword(string source, ValidationType type)
        {
            //< Retain the ValidationType
            this.Type = type;

            //< Split incoming pair by delimiter
            var data = source.Split(_Delim);
            //< Get the TobogganPolicy
            this.Policy = new TobogganPolicy(data[0]);
            //< Retain the actual password (removing leading/trailing whitespace)
            this.Password = data[1].Trim();
        }

        private bool ValidatePassword()
        {
            switch (Type)
            {
                case ValidationType.Range:
                    return ValidateByRange();
                case ValidationType.Position:
                    return ValidateByPostion();
                default:
                    throw new NotImplementedException($"Ya blew it - please set the ValidationType");
            }
        }

        private bool ValidateByRange()
        {
            //< Get the map of char -> # occurrence
            var charMap = GetCharacterMap(Password);

            //< If no occurence of Policy's 'Value' (and min was non-zero) -> invalid
            if (!charMap.ContainsKey(Policy.Value) && Policy.Min > 0)
            {
                return false;
            }
            else
            {
                //< Get the # times this 'value' occurred
                int count = charMap[Policy.Value];
                //< Check against the min/max occurence value
                return (count >= Policy.Min && count <= Policy.Max);
            }
        }

        private bool ValidateByPostion()
        {
            bool bMin = Password[Policy.MinPosition] == Policy.Value;
            bool bMax = Password[Policy.MaxPosition] == Policy.Value;

            //< Return XOR -> invalid if both postiions contain the value
            return bMin ^ bMax;
        }

        private static Dictionary<char, int> GetCharacterMap(string pass)
        {
            var map = new Dictionary<char, int>();
            foreach (char c in pass)
            {
                if (!map.ContainsKey(c))
                {
                    map.Add(c, 0);
                }
                map[c]++;
            }
            return map;
        }
    }
}
