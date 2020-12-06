using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent._2020.Classes
{
    public class CustomsAnswer
    {
        public Dictionary<char, int> Results { get; } = null;

        public CustomsAnswer(string input)
        {
            Results = new Dictionary<char, int>();
            foreach (var c in input)
            {
                if (!Results.ContainsKey(c))
                {
                    Results.Add(c, 0);
                }
                Results[c]++;
            }
        }
    }

    public class CustomsGroup
    {
        public List<CustomsAnswer> Answers { get; } = null;

        public int CountYes => CountDistinctYes();
        public int CountAllYes => CountIfAllYes();

        public int GroupSize => Answers.Count;

        public CustomsGroup(IEnumerable<string> answers)
        {
            this.Answers = answers.Select(x => new CustomsAnswer(x)).ToList();
        }

        private int CountDistinctYes()
        {
            var yes = new HashSet<char>();
            foreach (var answer in Answers)
            {
                foreach (var c in answer.Results.Keys)
                {
                    yes.Add(c);
                }
            }
            return yes.Count;
        }

        private int CountIfAllYes()
        {
            var yesDict = new Dictionary<char, int>();
            foreach (var answer in Answers)
            {
                foreach (var c in answer.Results.Keys)
                {
                    if (!yesDict.ContainsKey(c))
                    {
                        yesDict.Add(c, 0);
                    }
                    yesDict[c]++;
                }
            }
            return yesDict.Keys.Where(c => yesDict[c] == GroupSize).Count();
        }
    }
}
