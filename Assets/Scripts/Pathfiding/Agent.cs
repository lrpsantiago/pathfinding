using System;

namespace PushingBoxStudios.Pathfinding
{
    internal class Agent : IAgent
    {
        private Grid m_grid;
        private Location m_position;
        private Path m_path;

        public Agent(Grid grid, uint x, uint y)
        {
            m_grid = grid;
            m_position = new Location((int)x, (int)y);
            m_path = null;
        }

        public Agent(Grid grid, Location position)
        {
            m_grid = grid;
            m_position = position;
            m_path = null;
        }

        public IPath FindPath(uint x, uint y)
        {
            return FindPath(new Location((int)x, (int)y));
        }

        public IPath FindPath(Location goal)
        {
            AbstractPathfinder algorithm = new AStar();
            m_path = algorithm.FindPath(m_grid, m_position, goal);

            return m_path;
        }

        public void MoveOnPath(uint steps)
        {
            if (steps <= m_path.OriginalSize)
            {
                for (uint i = 0; i < steps; i++)
                {
                    m_path.PopFront();
                }

                m_position = m_path.Front;
            }
        }
    }
}