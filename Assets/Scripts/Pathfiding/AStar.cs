using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding
{
    internal class AStar : AbstractPathfinder
    {
        private uint CalculateDistance(Location from, Location to)
        {
            uint dX = (uint)Math.Abs(to.X - from.X);
            uint dY = (uint)Math.Abs(to.Y - from.Y);

            return (dX + dY);
        }

        public override Path FindPath(Grid grid, Location start, Location goal)
        {
            this.Statistics.Reset();
            this.Statistics.TotalGridNodes = grid.Width * grid.Height;
            this.Statistics.StartTimer();

            if (!ValidateGoal(grid, goal))
            {
                Path p = new Path();
                p.PushBack(start);
                return p;
            }

            IPriorityQueue<AStarNode> openList = new Heap<AStarNode>(HeapType.MinHeap); //new SortedList<AStarNode>();

            bool[,] isClosed = new bool[grid.Width, grid.Height];
            AStarNode[,] examined = new AStarNode[grid.Width, grid.Height];

            AStarNode hotspot = new AStarNode(null, start, goal);
            openList.Push(hotspot);

            examined[start.X, start.Y] = hotspot;

            while (hotspot.Position != goal)
            {
                if (openList.Count <= 0)
                {
                    return null;
                }

                hotspot = openList.Pop();

                isClosed[hotspot.Position.X, hotspot.Position.Y] = true;
                this.Statistics.AddClosedNode();

                Location pos = new Location();

                for (pos.Y = hotspot.Position.Y - 1; pos.Y <= hotspot.Position.Y + 1; pos.Y++)
                {
                    for (pos.X = hotspot.Position.X - 1; pos.X <= hotspot.Position.X + 1; pos.X++)
                    {
                        if (this.ProbeMode == EProbeMode.FourDirections)
                        {
                            if (pos.X != hotspot.Position.X && pos.Y != hotspot.Position.Y)
                            {
                                continue;
                            }
                        }

                        if (grid.InBounds(pos))
                        {
                            if (hotspot.Position != pos)
                            {
                                if (grid[(uint)pos.X, (uint)pos.Y])
                                {
                                    if (!isClosed[(uint)pos.X, (uint)pos.Y])
                                    {
                                        if (!this.HasCornerBetween(grid, hotspot.Position, pos) &&
                                            !this.HasDiagonalWall(grid, hotspot.Position, pos))
                                        {
                                            if (examined[(uint)pos.X, (uint)pos.Y] == null)
                                            {
                                                AStarNode node = new AStarNode(hotspot, pos, goal);

                                                openList.Push(node);
                                                examined[(uint)pos.X, (uint)pos.Y] = node;

                                                this.Statistics.AddOpenedNode();
                                            }
                                            else
                                            {
                                                AStarNode aux = examined[(uint)pos.X, (uint)pos.Y];
                                                AStarNode oldParent = aux.Parent;
                                                uint c1 = aux.G;

                                                aux.Parent = hotspot;
                                                uint c2 = aux.G;

                                                if (c2 > c1)
                                                {
                                                    aux.Parent = oldParent;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        this.Statistics.AddIteration();
                    }
                }
            }

            Stack<Location> inverter = new Stack<Location>();

            if (hotspot != null)
            {
                this.Statistics.PathCost = hotspot.G;
            }

            while (hotspot != null)
            {
                inverter.Push(hotspot.Position);
                hotspot = hotspot.Parent;
            }

            Path path = new Path();

            while (inverter.Count > 0)
            {
                path.PushBack(inverter.Pop());
            }

            path.PushBack(goal);

            this.Statistics.StopTimer();
            this.Statistics.PathLength = path.Size;

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

        private bool IsNotInList(SortedList<AStarNode> list, Location position)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Position == position)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsNotInList(Dictionary<Location, AStarNode> list, Location position)
        {
            return !list.ContainsKey(position);
        }
    }
}
