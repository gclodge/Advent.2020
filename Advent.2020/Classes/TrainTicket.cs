using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    public class TicketNote
    {
        public string Name { get; } = null;
        public List<int[]> Ranges { get; } = null;

        public TicketNote(string line)
        {
            var arr = line.Split(new string[] { ": " }, StringSplitOptions.None);
            this.Name = arr[0];

            var rangeData = arr[1].Split(new string[] { " or " }, StringSplitOptions.None);
            this.Ranges = rangeData.Select(x => ParseRange(x)).ToList();
        }

        private static int[] ParseRange(string data)
        {
            return data.Split('-').Select(x => int.Parse(x)).ToArray();
        }
    }

    public class Ticket
    {
        public List<int> Values { get; } = null;
        public int Length => Values.Count;

        public Ticket(string line)
        {
            this.Values = line.Split(',').Select(x => int.Parse(x)).ToList();
        }

        public int GetValue(int idx)
        {
            return Values[idx];
        }

        public bool IsValid(TicketManager manager)
        {
            return Values.All(v => manager.Ranges.ContainsKey(v));
        }

        public IEnumerable<int> GetInvalidValues(TicketManager manager)
        {
            return Values.Where(x => !manager.Ranges.ContainsKey(x));
        }
    }

    public class TicketManager
    {
        public List<string> Source { get; } = null;

        public Ticket Mine { get; } = null;
        public List<Ticket> Nearby { get; } = new List<Ticket>();
        public List<TicketNote> Notes { get; } = new List<TicketNote>();

        public Dictionary<int, HashSet<string>> Ranges { get; private set; } = null;

        public TicketManager(IEnumerable<string> input)
        {
            this.Source = input.ToList();

            int blankCounter = 0;
            foreach (var line in input)
            {
                if (line == "")
                {
                    blankCounter++;
                }
                else if (!line.EndsWith(":"))
                {
                    switch (blankCounter)
                    {
                        case 0:
                            Notes.Add(new TicketNote(line));
                            break;
                        case 1:
                            Mine = new Ticket(line);
                            break;
                        case 2:
                            Nearby.Add(new Ticket(line));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            PopulateRangeMap();
        }

        private void PopulateRangeMap()
        {
            Ranges = new Dictionary<int, HashSet<string>>();
            foreach (var note in Notes)
            {
                foreach (var range in note.Ranges)
                {
                    int len = (range[1] - range[0]) + 1;
                    foreach (int i in Enumerable.Range(range[0], len))
                    {
                        if (!Ranges.ContainsKey(i))
                        {
                            Ranges.Add(i, new HashSet<string>());
                        }
                        Ranges[i].Add(note.Name);
                    }
                }
            }
        }

        public int GetScanningErrorRate()
        {
            var vals = new List<int>();
            foreach (var ticket in Nearby)
            {
                var invalid = ticket.GetInvalidValues(this);
                if (invalid.Count() > 0)
                {
                    vals.AddRange(invalid);
                }
            }
            return vals.Sum();
        }

        public Dictionary<int, string> GetValidFieldPositions()
        {
            var validTix = Nearby.Where(x => x.IsValid(this));

            var usedFields = new HashSet<string>();
            var positions = new Dictionary<int, string>();

            int ticketCount = validTix.Count();

            //< Need to solve for each position within the tickets
            while (usedFields.Count != Mine.Length)
            {
                for (int i = 0; i < Mine.Length; i++)
                {
                    if (positions.ContainsKey(i))
                    {
                        //< Already solved - skip
                        continue;
                    }

                    //< Get all the values at this position in the ticket from nearby, valid tix
                    var vals = validTix.Select(x => x.GetValue(i));
                    //< Get the map of number of times a note covered this index in nearby, valid tix
                    var noteMap = GetNoteMap(vals);
                    //< Get the matching notes (notes which were covered by all tix)
                    var matching = noteMap.Where(kvp => !usedFields.Contains(kvp.Key))
                                          .Where(kvp => kvp.Value == ticketCount)
                                          .ToList();
                    //< Switch on the resulting count - if more than one, need to solve other positions first
                    if (matching.Count() > 1)
                    {
                        //< Can't decide yet - retain for later check
                    }
                    else
                    {
                        //< Only one solution here - use it and move on
                        var match = matching.Single();
                        positions.Add(i, match.Key);
                        usedFields.Add(match.Key);
                    }
                }
            }

            return positions;
        }

        private Dictionary<string, int> GetNoteMap(IEnumerable<int> vals)
        {
            var noteMap = new Dictionary<string, int>();
            foreach (var v in vals)
            {
                foreach (var note in Ranges[v])
                {
                    if (!noteMap.ContainsKey(note))
                    {
                        noteMap.Add(note, 0);
                    }
                    noteMap[note]++;
                }
            }
            return noteMap;
        }

        public Dictionary<string, int> GetMyTicketValues(Dictionary<int, string> fieldPositions)
        {
            var map = new Dictionary<string, int>();
            foreach (var kvp in fieldPositions)
            {
                map.Add(kvp.Value, Mine.GetValue(kvp.Key));
            }
            return map;
        }
    }
}
