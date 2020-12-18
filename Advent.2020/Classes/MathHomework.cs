using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public enum HomeworkType
    {
        Simple,  //< No precendence, simply L->R
        Advanced //< Addition before Multiplication
    }

    public class MathEquation
    {
        public string Source { get; } = null;
        public string Simplified { get; } = null;

        public HomeworkType Type { get; } = HomeworkType.Simple;

        public long Result { get; } = 0;

        public MathEquation(string line, HomeworkType type = HomeworkType.Simple)
        {
            //< Retain the source equation and the HomeWorktype (simple -> no precedence / advanced -> addition first)
            this.Source = line;
            this.Type = type;
            //< Simplify the equation based on the specified HomeworkType
            this.Simplified = Simplify(Source, Type);
            //< Get the result based on the specific HomeworkType
            this.Result = (Type == HomeworkType.Simple) ? SimpleSolve(Simplified) : PrecedenceSolve(Simplified);
        }

        public static string Simplify(string line, HomeworkType type)
        {
            var simplified = new StringBuilder();
            var openIdx = new List<int>();

            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                        //< Add the leading bracket to 'simplified' string
                        simplified.Append(c);
                        //< Add the index of the bracket to the index list
                        openIdx.Add(simplified.Length); 
                        break;
                    case ')':
                        //< Get the index of the last open bracket
                        int lastIdx = openIdx.Last();
                        //< Get the current sub-equation to solve (substring starting at last open bracket's index)
                        var eqn = simplified.ToString().Substring(lastIdx);
                        //< Get the result of that equation
                        var result = (type == HomeworkType.Simple) ? SimpleSolve(eqn) : PrecedenceSolve(eqn);
                        //< Replace the current un-solved sub-equation with its result
                        simplified.Remove(lastIdx - 1, simplified.Length - (lastIdx - 1));
                        simplified.Append(result);
                        //< Remove the index to the 'solved' open bracket (the last one)
                        openIdx.RemoveAt(openIdx.Count - 1);
                        break;
                    default:
                        simplified.Append(c);           //< Not a parentheses, simply add to the 'simplified' string
                        break;
                }
            }

            return simplified.ToString();
        }

        public static long SimpleSolve(string equation)
        {
            //< Split by space so we get { value, operator, value, .. }
            var vals = equation.Split(' ');
            //< Result starts at the first value
            long res = long.Parse(vals.First());
            //< Start on first operator, adjust result based on operator and following value
            for (int i = 1; i < vals.Count(); i += 2)
            {
                switch (vals[i])
                {
                    case "+":
                        res += long.Parse(vals[i + 1]);
                        break;
                    case "*":
                        res *= long.Parse(vals[i + 1]);
                        break;
                }
            }
            return res;
        }

        public static long PrecedenceSolve(string equation)
        {
            //< Split by space so we get { value, operator, value, .. }
            var toParse = equation.Split(' ');
            //< Instantiate the StringBuilder and 'last addition' placeholder
            var newLine = new StringBuilder();
            var lastAdd = string.Empty;
            //< Iterate through equation
            for (int i = 0; i < toParse.Length; i++)
            {
                switch (toParse[i])
                {
                    case "+":
                        //< Plus operator hit -> get what we're adding to (either the last addition or the previous value)
                        string addTo = !string.IsNullOrEmpty(lastAdd) ? lastAdd : toParse[i - 1];
                        //< Solve the intermediate equation (our current 'addTo' plus the next value after the operator)
                        var solved = SimpleSolve($"{addTo} + {toParse[i + 1]}").ToString();
                        //< Set this solution to the last 'addition' value
                        lastAdd = solved;
                        //< Remove the unsolved component from the 'simplified' string we're building
                        newLine.Remove(newLine.Length - addTo.Length - 1, addTo.Length + 1);
                        //< Append the simplified component (with a trailing space)
                        newLine.Append(solved + " ");
                        i += 1;
                        break;
                    default:
                        //< Simply add the value (plus a trailing space) to the newly 'simplified' expression
                        newLine.Append(toParse[i] + " ");
                        //< Set the 'last addition' placeholder to blank
                        lastAdd = string.Empty;
                        break;
                }
            }
            //< Equation is now reduced to something that can be solved via part one
            return SimpleSolve(newLine.ToString());
        }
    }

    public class MathHomework
    {
        public List<MathEquation> Equations { get; } = null;

        public MathHomework(IEnumerable<string> input, HomeworkType type = HomeworkType.Simple)
        {
            this.Equations = input.Select(x => new MathEquation(x, type)).ToList();
        }
    }
}
