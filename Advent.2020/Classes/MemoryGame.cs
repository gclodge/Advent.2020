using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class MemoryGame
    {
        public static int FindWordAtTurn(IEnumerable<int> input, int target)
        {
            var wordMap = new Dictionary<int, int>();
            int len = input.Count();
            //< Pre-populate the word map with the first occurence of the input (save the last number)
            for(int i = 0; i < len - 1; i++)
            {
                //< Map has the word as the key with the value being the 'turn' it was last spoken (here, i+1)
                wordMap.Add(input.ElementAt(i), i + 1);
            }
            //< Grab the 'last' word in the input sequence as the starting 'last' word spoken
            int lastWord = input.Last();
            for (int turn = len + 1; turn <= target; turn++) //< Loop until we hit our turn target (lawd, save my efficiency)
            {
                //< Check if the word's been spoken before
                if (!wordMap.ContainsKey(lastWord))
                {
                    wordMap[lastWord] = turn - 1; //< Was spoken on the last turn (turn - 1)
                    lastWord = 0;                 //< First time this word was spoken, thus next value is zero
                }
                else
                {
                    //< Get the difference between when it was last spoken
                    int newWord = (turn - 1) - wordMap[lastWord];
                    //< Update the map to last time spoken
                    wordMap[lastWord] = (turn - 1);
                    //< Set the 'last' word spoken to the newly calculated difference
                    lastWord = newWord;
                }
            }
            //< Return the last spoken word, assuming this actually terminates
            return lastWord;
        }
    }
}
