using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding
{
    internal class AStar : AbstractPathfinder
    {
        public override Path FindPath(Grid grid, Location start, Location goal)
        {
            Statistics.Reset();
            Statistics.TotalGridNodes = grid.Width * grid.Height;
            Statistics.StartTimer();

            if (!ValidateGoal(grid, goal))
            {
                var p = new Path();
                p.PushBack(start);
                return p;
            }

            var openList = new Heap<AStarNode>();
            var isClosed = new bool[grid.Width, grid.Height];
            var examined = new AStarNode[grid.Width, grid.Height];
            var hotspot = new AStarNode(null, start, goal);

            openList.Push(hotspot);
            examined[start.X, start.Y] = hotspot;

            var adjacents = new Location[8];

            while (hotspot.Position != goal)
            {
                if (openList.Count <= 0)
                {
                    return null;
                }

                hotspot = openList.Pop();

                isClosed[hotspot.Position.X, hotspot.Position.Y] = true;
                Statistics.AddClosedNode();

                SetAdjacentArray(adjacents, hotspot.Position);

                for (int i = 0; i < adjacents.Length; i++)
                {
                    var pos = adjacents[i];

                    if (ProbeMode == EProbeMode.FourDirections
                        && pos.X != hotspot.Position.X
                        && pos.Y != hotspot.Position.Y)
                    {
                        continue;
                    }

                    if (grid.InBounds(pos)
                        && hotspot.Position != pos
                        && grid[(uint)pos.X, (uint)pos.Y]
                        && !isClosed[(uint)pos.X, (uint)pos.Y])
                    {
                        if (!HasCornerBetween(grid, hotspot.Position, pos) &&
                            !HasDiagonalWall(grid, hotspot.Position, pos))
                        {
                            if (examined[(uint)pos.X, (uint)pos.Y] == null)
                            {
                                var node = new AStarNode(hotspot, pos, goal);

                                openList.Push(node);
                                examined[(uint)pos.X, (uint)pos.Y] = node;

                                Statistics.AddOpenedNode();
                            }
                            else
                            {
                                var aux = examined[(uint)pos.X, (uint)pos.Y];
                                var oldParent = aux.Parent;
                                var c1 = aux.G;

                                aux.Parent = hotspot;
                                var c2 = aux.G;

                                if (c2 > c1)
                                {
                                    aux.Parent = oldParent;
                                }
                            }
                        }
                    }

                    Statistics.AddIteration();
                }
            }

            var inverter = new Stack<Location>();

            if (hotspot != null)
            {
                Statistics.PathCost = hotspot.G;
            }

            while (hotspot != null)
            {
                inverter.Push(hotspot.Position);
                hotspot = hotspot.Parent;
            }

            var path = new Path();

            while (inverter.Count > 0)
            {
                path.PushBack(inverter.Pop());
            }

            path.PushBack(goal);

            Statistics.StopTimer();
            Statistics.PathLength = path.Size;

            return path;
        }

        private bool HasCornerBetween(Grid grid, Location from, Location to)
        {
            return grid[from] && grid[to] && ((grid[(uint)from.X, (uint)to.Y] && !grid[(uint)to.X, (uint)from.Y]) ||
                (!grid[(uint)from.X, (uint)to.Y] && grid[(uint)to.X, (uint)from.Y]));
        }

        private bool HasDiagonalWall(Grid grid, Location from, Location to)
        {
            return grid[from] && grid[to] && (!grid[(uint)from.X, (uint)to.Y] && !grid[(uint)to.X, (uint)from.Y]);
        }

        private bool ValidateGoal(Grid grid, Location goal)
        {
            if (goal.X < grid.Width && goal.Y < grid.Height)
            {
                return grid[(uint)goal.X, (uint)goal.Y];
            }

            return false;
        }

        private void SetAdjacentArray(Location[] adjacents, Location hotspot)
        {
            adjacents[0].X = hotspot.X - 1;
            adjacents[0].Y = hotspot.Y - 1;

            adjacents[1].X = hotspot.X + 1;
            adjacents[1].Y = hotspot.Y - 1;

            adjacents[2].X = hotspot.X - 1;
            adjacents[2].Y = hotspot.Y + 1;

            adjacents[3].X = hotspot.X + 1;
            adjacents[3].Y = hotspot.Y + 1;

            adjacents[4].X = hotspot.X;
            adjacents[4].Y = hotspot.Y - 1;

            adjacents[5].X = hotspot.X;
            adjacents[5].Y = hotspot.Y + 1;

            adjacents[6].X = hotspot.X - 1;
            adjacents[6].Y = hotspot.Y;

            adjacents[7].X = hotspot.X + 1;
            adjacents[7].Y = hotspot.Y;
        }
    }
}
