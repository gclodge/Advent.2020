using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Advent._2020.Utility;

namespace Advent._2020.Classes
{
    public class SeatID
    {
        public int Row { get; } = 0;
        public int Column { get; } = 0;

        public int ID => Row * 8 + Column;

        public SeatID(int r, int c)
        {
            this.Row = r;
            this.Column = c;
        }
    }

    public class BoardingPass
    {
        private const int NumRows = 128;
        private const int NumCols = 8;

        public static IEnumerable<int> Rows { get; } = Enumerable.Range(0, NumRows);
        public static IEnumerable<int> Columns { get; } = Enumerable.Range(0, NumCols);

        public static SeatID GetSeatID(string pass)
        {
            var rowIndex = pass.Substring(0, 7);
            var colIndex = pass.Substring(7);

            int row = GetValue(Rows, rowIndex);
            int col = GetValue(Columns, colIndex);

            return new SeatID(row, col);
        }

        private static HalfSplit GetSplitType(char input)
        {
            switch (input)
            {
                case 'F':
                case 'L':
                    return HalfSplit.Bottom;
                case 'B':
                case 'R':
                    return HalfSplit.Top;
                default:
                    return HalfSplit.None;
            }
        }

        private static int GetValue(IEnumerable<int> values, string index)
        {
            IEnumerable<int> currValues = values;
            for (int i = 0; i < index.Length; i++)
            {
                var split = GetSplitType(index[i]);

                currValues = Functions.SplitInHalf(currValues, split);
            }
            return currValues.Single();
        }
    }
}
