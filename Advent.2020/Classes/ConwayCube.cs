using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Classes
{
    public class ConwayCube
    {
        public const char Active = '#';
        public const char Inactive = '.';

        public static readonly int[] Offsets = new int[] { 0, -1, 1 };

        public char State { get; set; } = Inactive;

        public int[] Position { get; } = null;
        public string Index { get; } = null;

        public int Dimensions => Position.Length;

        public ConwayCube(IEnumerable<int> pos, char state = Active)
        {
            this.Position = pos.ToArray();
            this.Index = GetPositionString(Position);
            this.State = state;
        }

        public ConwayCube(string index, char state = Active)
        {
            this.Index = index;
            this.Position = GetPosition(index);
            this.State = state;
        }

        public IEnumerable<string> GetNeighbours()
        {
            return GetNeighbours(Index, Dimensions);
        }

        public static IEnumerable<string> GetNeighbours(string index, int dim)
        {
            //< Get the position array for this index
            var pos = ConwayCube.GetPosition(index);
            //< Get the neighbouring positions (removing the position at the given index)
            var neighs = GetOffsets(dim).Select(offset => GetPosition(pos, offset))
                                        .Select(p => GetPositionString(p))
                                        .Where(idx => idx != index);
            return neighs;
        }

        public static int[] GetPosition(int[] pos, int[] offset)
        {
            return pos.Zip(offset, (a, b) => a + b).ToArray();
        }

        public static char GetNewState(char currState, int numActiveNeigh)
        {
            if (currState == Active)
            {
                return (numActiveNeigh == 2 || numActiveNeigh == 3) ? Active : Inactive;
            }
            else
            {
                return (numActiveNeigh == 3) ? Active : Inactive;
            }
        }

        public static IEnumerable<int[]> GetOffsets(int dims)
        {
            return Utility.Functions.CartesianProduct(Offsets, dims);
        }

        public static string GetPositionString(int[] pos)
        {
            return string.Join(",", pos);
        }

        public static int[] GetPosition(string index)
        {
            return index.Split(',').Select(x => int.Parse(x)).ToArray();
        }

        public override string ToString()
        {
            return State.ToString();
        }
    }

    public class ConwayController
    {
        public const int OriginValue = 0;
        public const int DefaultDimensions = 3;

        public List<string> Source { get; } = null;

        public int Dimensions { get; } = DefaultDimensions;
        public Dictionary<string, ConwayCube> CubeMap { get; private set; } = null;

        public long NumActive => CubeMap.Count;

        public ConwayController(IEnumerable<string> input, int dimensions = DefaultDimensions)
        {
            //< Hold onto the raw input as a list
            this.Source = input.ToList();
            //< Retain the dimensionality
            this.Dimensions = dimensions;

            this.CubeMap = new Dictionary<string, ConwayCube>();
            //< Iterate over each row (y)
            foreach (int y in Enumerable.Range(0, Source.Count))
            {
                //< Iterate over each available (x) position
                foreach (int x in Enumerable.Range(0, Source[y].Length))
                {
                    char state = Source[y][x];
                    //< If this cube is active -> add to the map
                    if (state == ConwayCube.Active)
                    {
                        //< Parse the starting position
                        var pos = GetStartingPosition(x, y, Dimensions);
                        //< Generate the new ConwayCube object and add to the dimension map
                        var cube = new ConwayCube(pos, state);
                        CubeMap.Add(cube.Index, cube);
                    }
                }
            }
        }

        public static int[] GetStartingPosition(int x, int y, int dimensions)
        {
            var pos = new List<int>() { x, y };
            foreach (int dim in Enumerable.Range(0, dimensions - 2))
            {
                pos.Add(OriginValue);
            }
            return pos.ToArray();
        }

        public void SimulateSteps(int steps)
        {
            //< Each cube gets a state change simultaneously
            foreach (int step in Enumerable.Range(0, steps))
            {
                //< Instantiate map of any changes to existing cubes
                var changeMap = new Dictionary<string, char>();
                //< Instantiate map of any 'new' neighbours to be added after this turn
                var neighsToCheck = new HashSet<string>();

                //< Iterate over each currently active cube in the current iteration of the map
                foreach (var kvp in CubeMap)
                {
                    //< Get all the neighbour positions/indices
                    var neighs = ConwayCube.GetNeighbours(kvp.Key, Dimensions);
                    //< Check if we need to change this cube's state
                    var newState = GetNewState(neighs, kvp.Value.State);
                    if (newState != kvp.Value.State)
                    {
                        changeMap.Add(kvp.Key, newState);
                    }
                    //< Add all newly encountered (not in the map) neighbours to be considered
                    foreach (var neigh in neighs.Where(n => !CubeMap.ContainsKey(n)))
                    {
                        if (!neighsToCheck.Contains(neigh))
                        {
                            neighsToCheck.Add(neigh);
                        }
                    }
                }

                //< Add any newly-encountered neighbours to the map between turns
                foreach (var neigh in neighsToCheck)
                {
                    //< Get all the neighbour positions/indices
                    var neighs = ConwayCube.GetNeighbours(neigh, Dimensions);
                    //< Check if we need to change this cube's state
                    var newState = GetNewState(neighs, ConwayCube.Inactive);
                    if (newState == ConwayCube.Active)
                    {
                        changeMap.Add(neigh, newState);
                    }
                }

                //< Apply changes
                ApplyChanges(changeMap);
            }
        }

        private void ApplyChanges(Dictionary<string, char> changeMap)
        {
            foreach (var kvp in changeMap)
            {
                //< If changing to active, we're simply adding it to the map
                if (kvp.Value == ConwayCube.Active)
                {
                    if (!CubeMap.ContainsKey(kvp.Key))
                    {
                        CubeMap.Add(kvp.Key, new ConwayCube(kvp.Key, ConwayCube.Active));
                    }
                }
                else
                {
                    //< Setting a cube to inactive means removing it from the map
                    if (CubeMap.ContainsKey(kvp.Key))
                    {
                        CubeMap.Remove(kvp.Key);
                    }
                }
            }
        }

        private char GetNewState(IEnumerable<string> neighs, char state)
        {
            //< Count active neighbour cubes
            int numActiveNeighs = neighs.Count(x => CubeMap.ContainsKey(x));
            //< Get the resulting state and return it
            return ConwayCube.GetNewState(state, numActiveNeighs);
        }
    }
}
