using System;

namespace PushingBoxStudios.Pathfinding
{
    internal class AStarNode : IComparable, IComparable<AStarNode>
    {
        private static readonly uint STRAIGHTCOST = 100;
        private static readonly uint DIAGONALCOST = 141;

        private AStarNode _parent;
        private Location _pos;
        private uint _heuristicalCost;
        private uint _soFarCost;

        public AStarNode Parent
        {
            get { return _parent; }

            set
            {
                _parent = value;
                _soFarCost = CalculateG();
            }
        }

        public Location Position
        {
            get { return _pos; }
        }

        public uint G
        {
            get { return _soFarCost; }
        }

        public uint Score
        {
            get { return (_heuristicalCost + _soFarCost); }
        }

        public AStarNode(AStarNode parent, Location pos, Location goal)
        {
            _parent = parent;
            _pos = pos;
            _soFarCost = CalculateG();
            _heuristicalCost = CalculateH(goal);
        }

        private uint CalculateG()
        {
            if (_parent == null)
            {
                return 0;
            }

            if (_parent.Position.X != _pos.X && _parent.Position.Y != _pos.Y)
            {
                return _parent.G + DIAGONALCOST;
            }

            return _parent.G + STRAIGHTCOST;
        }

        private uint CalculateH(Location goal)
        {
            uint dX = (uint)Math.Abs(goal.X - _pos.X);
            uint dY = (uint)Math.Abs(goal.Y - _pos.Y);

            return STRAIGHTCOST * (dX + dY) + (DIAGONALCOST - 2 * STRAIGHTCOST) * Math.Min(dX, dY);
            //return (dX + dY) * STRAIGHTCOST; //Manhattan
            //return (uint)Math.Sqrt(dX * dX + dY * dY) * STRAIGHTCOST; //Euclidean
        }

        public int CompareTo(object obj)
        {
            return CompareTo((AStarNode)obj);
        }

        public int CompareTo(AStarNode other)
        {
            if (other == null)
            {
                return 1;
            }

            return (int)Score - (int)other.Score;
        }
    }
}
