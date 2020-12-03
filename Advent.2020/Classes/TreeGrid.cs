using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Advent._2020.Utility;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Classes
{
    public class TreeGrid
    {
        public static readonly Vector<double> Origin = Functions.GetVector(0, 0);

        public List<string> Rows { get; private set; } = null;

        public List<Vector<double>> Trees { get; private set; } = new List<Vector<double>>();

        public Vector<double> Position { get; private set; } = Origin.Clone();

        public int Height => Rows.Count;
        public int Width => Rows.First().Length;

        public int X => (int)Position[0];
        public int Y => (int)Position[1];

        public TreeGrid(IEnumerable<string> rows)
        {
            Rows = rows.ToList();
        }

        public void ResetPositionAndTrees()
        {
            Position = Origin.Clone();
            Trees = new List<Vector<double>>();
        }

        public void TraverseAndCountTrees(Vector<double> slope)
        {
            ResetPositionAndTrees();

            while (Y < Height)
            {
                //< Move the position
                if (!MoveWithOverflow(slope))
                {
                    break;
                }
                //< Check if we a tree mafk
                else if (IsTree(Position))
                {
                    Trees.Add(Position.Clone());
                }
            }
        }

        public bool MoveWithOverflow(Vector<double> slope)
        {
            int newX = X + (int)slope[0];
            if (newX >= Width)
            {
                newX = (newX - Width);
            }

            int newY = Y + (int)slope[1];

            if (newY >= Height)
            {
                return false;
            }

            Position = Functions.GetVector(newX, newY);
            return true;
        }

        private const char Tree = '#';
        public bool IsTree(Vector<double> pos)
        {
            return Rows[Y].ElementAt(X) == Tree;
        }
    }
}
