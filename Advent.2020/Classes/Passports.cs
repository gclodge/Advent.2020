using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class PassportFile
    {
        public string Source { get; private set; } = null;

        public List<string> Rows { get; private set; } = null;

        public List<Passport> Passports { get; private set; } = null;

        public PassportFile(string file)
        {
            //< Retain reference to source file and part in rows
            this.Source = file;
            this.Rows = Helpers.FileHelper.ParseFile(Source).ToList();

            //< Get all the individual Passports contained
            this.Passports = Utility.Functions.SplitByByElement(Rows, "")
                                              .Select(x => new Passport(x))
                                              .ToList();
        }
    }

    public enum PassportUnits
    {
        Inches,
        Centimeters,
        Unknown
    }

    public class Passport
    {
        #region Delimiters & Field Names
        private const char _KvpDelim = ' ';
        private const char _KeyValueDelim = ':';

        private const string _BirthYear      = "byr";
        private const string _IssueYear      = "iyr";
        private const string _ExpirationYear = "eyr";
        private const string _Height         = "hgt";
        private const string _HairColor      = "hcl";
        private const string _EyeColor       = "ecl";
        private const string _PassportID     = "pid";
        private const string _CountryID      = "cid";
        #endregion

        public Dictionary<string, string> Fields { get; private set; } = null;

        public bool HasRequiredFields => CheckRequiredFields();

        public bool IsValid => CheckValidity();

        private Dictionary<string, bool> _Requirements = new Dictionary<string, bool>()
        {
            { _BirthYear, true },
            { _IssueYear, true },
            { _ExpirationYear, true },
            { _Height, true },
            { _HairColor, true },
            { _EyeColor, true },
            { _PassportID, true },
            { _CountryID, false },
        };

        public List<string> RequiredFields => _Requirements.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

        public Passport(IEnumerable<string> rows)
        {
            this.Fields = new Dictionary<string, string>();

            foreach (var row in rows)
            {
                var kvps = row.Split(_KvpDelim);
                foreach (var kvp in kvps)
                {
                    var data = kvp.Split(_KeyValueDelim);
                    Fields.Add(data[0], data[1]);
                }
            }
        }

        private bool CheckRequiredFields()
        {
            return RequiredFields.All(x => Fields.ContainsKey(x));
        }

        private bool CheckValidity()
        {
            //< Check all required fields are present
            if (!CheckRequiredFields())
            {
                return false;
            }
            else
            {
                //< Loop over only required fields -> ensure all values are valid
                foreach (var field in RequiredFields)
                {
                    if (!CheckField(field, Fields[field]))
                    {
                        return false;
                    }
                }
                //< None failed - we're valid
                return true;
            }
        }

        public static bool CheckField(string key, string value)
        {
            switch (key)
            {
                case _BirthYear:
                    return CheckDigits(value, 1920, 2002, 4);
                case _IssueYear:
                    return CheckDigits(value, 2010, 2020, 4);
                case _ExpirationYear:
                    return CheckDigits(value, 2020, 2030, 4);
                case _Height:
                    return CheckHeight(value);
                case _HairColor:
                    return CheckHairColor(value);
                case _EyeColor:
                    return CheckEyeColor(value);
                case _PassportID:
                    return CheckPassportID(value);
                case _CountryID:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CheckValue(int value, int min, int max)
        {
            return (min <= value && max >= value);
        }

        public static bool CheckDigits(string value, int min, int max, int count)
        {
            int v = -1;

            if (value.Length != count)
            {
                return false;
            }
            else if (!int.TryParse(value, out v))
            {
                return false;
            }
            else
            {
                return CheckValue(v, min, max);
            }
        }

        public static bool CheckHeight(string value)
        {
            int v = -1;

            var units = GetUnits(value);
            if (units == PassportUnits.Unknown)
            {
                return false;
            }

            string numStr = value.Substring(0, value.Length - 2);
            if (!int.TryParse(numStr, out v))
            {
                return false;
            }

            switch (units)
            {
                case PassportUnits.Centimeters:
                    return CheckValue(v, 150, 193);
                case PassportUnits.Inches:
                    return CheckValue(v, 59, 76);
                default:
                    return false;
            }
        }

        public static PassportUnits GetUnits(string value)
        {
            if (value.EndsWith("cm"))
            {
                return PassportUnits.Centimeters;
            }
            else if (value.EndsWith("in"))
            {
                return PassportUnits.Inches;
            }
            else
                return PassportUnits.Unknown;
        }

        public static bool CheckHairColor(string value)
        {
            if (!value.StartsWith("#"))
            {
                return false;
            }
            else if (value.Length != 7)
            {
                return false;
            }
            else
            {
                return value.Skip(1).All(c => (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9'));
            }
        }

        private static readonly HashSet<string> _AllowedEyeColor = new HashSet<string>
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth",
        };
        public static bool CheckEyeColor(string value)
        {
            return _AllowedEyeColor.Contains(value);
        }

        public static bool CheckPassportID(string value)
        {
            int v = -1;

            if (value.Length != 9)
            {
                return false;
            }
            else
            {
                return int.TryParse(value, out v);
            }
        }
    }
}
