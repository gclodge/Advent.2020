using System;
using System.Linq;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;

namespace Advent._2020.Classes
{
    public class ShipCommand
    {
        public int Value { get; } = 0;
        public char Action { get; } = default(char);
   
        public ShipCommand(string line)
        {
            this.Action = line[0];
            this.Value = int.Parse(line.Substring(1));
        }

        public override string ToString()
        {
            return $"{Action} -> {Value}";
        }
    }

    public enum ShipNavigationType
    {
        Position, Waypoint
    }

    public class Ship
    {
        private const double DEG_TO_RAD = Math.PI / 180.0;

        public static readonly Vector<double> Origin = CreateVector.Dense(new double[] { 0.0, 0.0 });

        public static readonly Dictionary<char, Vector<double>> Directions = new Dictionary<char, Vector<double>>()
        {
            { 'N', CreateVector.Dense(new double[] { 0.0, 1.0 }) },
            { 'S', CreateVector.Dense(new double[] { 0.0, -1.0 }) },
            { 'E', CreateVector.Dense(new double[] { 1.0, 0.0 }) },
            { 'W', CreateVector.Dense(new double[] { -1.0, 0.0 }) },
        };

        public Vector<double> Position { get; private set; } = Origin.Clone();
        public Vector<double> Heading  { get; private set; } = Directions['E'].Clone();
        public Vector<double> Waypoint { get; private set; } = CreateVector.Dense(new double[] { 10.0, 1.0 });

        public ShipNavigationType NavigationType { get; } = ShipNavigationType.Position;

        public int X => (int)Position[0];
        public int Y => (int)Position[1];

        public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

        public Ship(ShipNavigationType navType = ShipNavigationType.Position)
        {
            NavigationType = navType;
        }

        public void ExecuteCommand(ShipCommand command)
        {
            switch (command.Action)
            {
                case 'N':
                case 'S':
                case 'E':
                case 'W':
                    Translate(Directions[command.Action], command.Value);
                    break;
                case 'F':
                    MoveForward(command.Value);
                    break;
                case 'L':
                    Rotate(command.Value);
                    break;
                case 'R':
                    Rotate(-1 * command.Value);
                    break;
                default:
                    throw new NotImplementedException($"Unknown action encountered: {command.Action}");
            }
        }

        public void Rotate(int degrees)
        {
            if (NavigationType == ShipNavigationType.Position)
            {
                Heading = RotateVector(degrees, Heading);
            }
            else
            {
                Waypoint = RotateVector(degrees, Waypoint);
            }
        }

        public static Vector<double> RotateVector(int degrees, Vector<double> v)
        {
            double rads = degrees * DEG_TO_RAD;

            var ca = Math.Cos(rads);
            var sa = Math.Sin(rads);

            var newHeading = CreateVector.Dense(new double[]
            {
                Math.Round(ca * v[0] - sa * v[1], 2),
                Math.Round(sa * v[0] + ca * v[1], 2)
            });

            return newHeading.Clone();
        }

        public static Vector<double> GetDirection(Vector<double> a, Vector<double> b)
        {
            var ab = (b - a);
            var ab_hat = ab / ab.L2Norm();
            return CreateVector.Dense(new double[] { Math.Round(ab_hat[0], 2), Math.Round(ab_hat[1], 2) });
        }

        public void Translate(Vector<double> unit, int magnitude)
        {
            if (NavigationType == ShipNavigationType.Position)
            {
                this.Position += (unit * magnitude);
            }
            else
            {
                this.Waypoint += (unit * magnitude);
            }
        }

        public void MoveForward(int magnitude)
        {
            var diff = (NavigationType == ShipNavigationType.Position) ? Heading * magnitude : Waypoint * magnitude;
            Position += diff;
        }
    }

    public class ShipCaptain
    {
        public Ship Ship { get; } = null;

        public List<ShipCommand> Commands { get; } = null;

        public int Distance => Ship.ManhattanDistance;
        public Vector<double> Position => Ship.Position;

        public ShipCaptain(IEnumerable<string> input, ShipNavigationType navType = ShipNavigationType.Position)
        {
            this.Ship = new Ship(navType);
            this.Commands = input.Select(x => new ShipCommand(x)).ToList();
        }

        public void Navigate()
        {
            foreach (var command in Commands)
            {
                Ship.ExecuteCommand(command);
            }
        }
    }
}
