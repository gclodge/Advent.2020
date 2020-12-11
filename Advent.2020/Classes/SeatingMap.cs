using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Classes
{
    public enum SeatType
    {
        Floor,
        Empty,
        Occupied
    }

    public class Seat
    {
        public const char Floor = '.';
        public const char Empty = 'L';
        public const char Occupied = '#';

        public static readonly Vector<double>[] Directions = new Vector<double>[]
        {
            CreateVector.Dense(new double[] { 1.0, 0.0 }),   //< Right
            CreateVector.Dense(new double[] { -1.0, 0.0 }),  //< Left
            CreateVector.Dense(new double[] { 0.0, 1.0 }),   //< 'Up'
            CreateVector.Dense(new double[] { 0.0, -1.0 }),  //< 'Down'

            CreateVector.Dense(new double[] { 1.0, 1.0 }),   //< 'Up'- right
            CreateVector.Dense(new double[] { 1.0, -1.0 }),  //< 'Down'- right
            CreateVector.Dense(new double[] { -1.0, 1.0 }),  //< 'Up'- left
            CreateVector.Dense(new double[] { -1.0, -1.0 }), //< 'Down'- left
        };

        public int X { get; } = -1;
        public int Y { get; } = -1;

        public SeatType Type { get; set; } = SeatType.Floor;

        public Seat(int x, int y, char seat)
        {
            this.X = x;
            this.Y = y;
            this.Type = GetType(seat);
        }

        public bool IsSeat()
        {
            return Type != SeatType.Floor;
        }

        public static SeatType GetType(char seat)
        {
            switch (seat)
            {
                case Empty:
                    return SeatType.Empty;
                case Occupied:
                    return SeatType.Occupied;
                case Floor:
                default:
                    return SeatType.Floor;
            }
        }

        public static List<int[]> GetNeighbours(Seat s, SeatingMap map)
        {
            return map.SearchFirstVisible ? GetVisiblePositions(s, map) : GetAdjacentPositions(s, map);
        }

        private static List<int[]> GetAdjacentPositions(Seat s, SeatingMap map)
        {
            //< Bleh
            var adj = new List<int[]>();
            foreach (int x in Enumerable.Range(s.X - 1, 3))
            {
                foreach (int y in Enumerable.Range(s.Y - 1, 3))
                {
                    if (x == s.X && y == s.Y)
                        continue;

                    var pos = new int[] { x, y };
                    if (map.IsValid(pos))
                    {
                        adj.Add(pos);
                    }
                }
            }
            return adj;
        }

        private static List<int[]> GetVisiblePositions(Seat s, SeatingMap map)
        {
            var startPos = new int[] { s.X, s.Y };
            var seats = new List<int[]>();
            foreach (var dir in Directions)
            {
                //< Get the 'next' position along this direction
                var pos = GetNextPosition(startPos, dir);
                //< While the 'next' position is valid (to the map) -> check if it's a seat mafk
                while (map.IsValid(pos))
                {
                    //< Check if we're a seat - if so, grab that position and halt (first visible)
                    if (map.Grid[pos[0], pos[1]].IsSeat())
                    {
                        seats.Add(pos);
                        break;
                    }
                    //< Move onto the next position
                    pos = GetNextPosition(pos, dir);
                }
            }
            return seats;
        }

        private static int[] GetNextPosition(int[] pos, Vector<double> dir)
        {
            return new int[] { pos[0] + (int)dir[0], pos[1] + (int)dir[1] };
        }
    }

    public class SeatChange
    {
        public Seat Source { get; } = null;
        public SeatType NewType { get; } = SeatType.Floor;

        public int X => Source.X;
        public int Y => Source.Y;

        public SeatChange(Seat src, SeatType newType)
        {
            this.Source = src;
            this.NewType = newType;
        }
    }

    public class SeatingMap
    {
        public int Width { get; } = 0;
        public int Height { get; } = 0;

        public Seat[,] Grid { get; private set; } = null;
        public List<Seat> Seats { get; private set; } = null;

        public int Steps { get; private set; } = 0;
        public int Occupied => Seats.Count(x => x.Type == SeatType.Occupied);

        public int Tolerance { get; } = 4;
        public bool SearchFirstVisible { get; } = false;

        public SeatingMap(IEnumerable<string> input, int tolerance = 4, bool searchFirstVisible = false)
        {
            //< Parse Width/Height
            this.Width = input.First().Length;
            this.Height = input.Count();
            //< Assign the 'visible occupied seat tolerance' and how we search
            this.Tolerance = tolerance;
            this.SearchFirstVisible = searchFirstVisible;
            //< Make dat Grid in memory, boah
            this.Grid = new Seat[Width, Height];
            this.Seats = new List<Seat>();
            //< Populate them shits
            for (int y = 0; y < Height; y++)
            {
                string line = input.ElementAt(y);
                for (int x = 0; x < Width; x++)
                {
                    Grid[x, y] = new Seat(x, y, line[x]);
                    if (Grid[x, y].IsSeat())
                    {
                        //< Retain list of all non-floor locations for later use
                        Seats.Add(Grid[x, y]);
                    }
                }
            }
        }

        public bool IsValid(int[] pos)
        {
            var bX = pos[0] >= 0 && pos[0] < Width;
            var bY = pos[1] >= 0 && pos[1] < Height;
            return (bX && bY);
        }

        public void SimulateSeating()
        {
            Steps = 0;
            List<SeatChange> changes = new List<SeatChange>();

            while (changes.Count > 0 || Steps == 0)
            {
                //< Get all changes to seats
                changes = GetChanges();
                //< Apply changes
                ApplyChanges(changes);
                //< Update 'Step' index
                Steps++;
            }
        }

        private List<SeatChange> GetChanges()
        {
            var changes = new List<SeatChange>();
            foreach (var seat in Seats)
            {
                //< Check if it's changing - generate SeatChange if it does
                var change = GetChange(seat);
                if (change != null)
                {
                    changes.Add(change);
                }
            }
            return changes;
        }

        private void ApplyChanges(List<SeatChange> changes)
        {
            foreach (var change in changes)
            {
                Grid[change.X, change.Y].Type = change.NewType;
            }
        }

        private SeatChange GetChange(Seat seat)
        {
            //< Get all neighbour seats to be checked
            var neighs = Seat.GetNeighbours(seat, this);

            if (seat.Type == SeatType.Empty)
            {
                if (CheckPositions(neighs, _EmptyTypes))
                {
                    return new SeatChange(seat, SeatType.Occupied);
                }
            }
            else if (seat.Type == SeatType.Occupied)
            {
                if (CheckPositions(neighs, _OccupiedTypes, Tolerance))
                {
                    return new SeatChange(seat, SeatType.Empty);
                }
            }

            //< No change - return null
            return null;
        }

        private static readonly SeatType[] _EmptyTypes = new SeatType[] { SeatType.Empty, SeatType.Floor };
        private static readonly SeatType[] _OccupiedTypes = new SeatType[] { SeatType.Occupied };

        private bool CheckPositions(IEnumerable<int[]> seats, SeatType[] types, int? numToMatch = null)
        {
            int countMatch = 0;
            foreach (var seat in seats)
            {
                if (types.Contains(Grid[seat[0], seat[1]].Type))
                {
                    countMatch++;
                }
            }

            if (numToMatch.HasValue)
                return countMatch >= numToMatch;
            else
                return (countMatch == seats.Count());
        }
    }
}
