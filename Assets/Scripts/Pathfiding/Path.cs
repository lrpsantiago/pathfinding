using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding
{
    public class Path : IPath
    {
        private List<Location> m_waypoints;

        public Location Front
        {
            get
            {
                return m_waypoints[0];
            }
        }

        public uint OriginalSize { get; private set; }

        public uint Size
        {
            get { return (uint)m_waypoints.Count; }
        }

        internal Path()
        {
            m_waypoints = new List<Location>();
        }

        public void PushBack(Location pos)
        {
            m_waypoints.Add(pos);
            this.OriginalSize = (uint)m_waypoints.Count;
        }

        public void PopFront()
        {
            if (this.Size > 0)
            {
                m_waypoints.RemoveAt(0);
            }
        }

        public IPath Clone()
        {
            Path clonePath = new Path();

            Location[] waypoints = m_waypoints.ToArray();

            for (int i = 0; i < waypoints.Length; i++)
            {
                clonePath.PushBack(waypoints[i]);
            }

            return clonePath;
        }

        public IList<Location> ToList()
        {
            return new List<Location>(m_waypoints.ToArray());
        }

        //public void RemoveEdges()
        //{
        //    List<Location> waypointsToRemove = new List<Location>();

        //    for (int i = 1; i < m_waypoints.Count - 1; i++)
        //    {
        //        Location hotspot = m_waypoints[i];
        //        Location prev = m_waypoints[i - 1];
        //        Location next = m_waypoints[i + 1];

        //        Location prevDirection = new Location(prev.X - hotspot.X, prev.Y - hotspot.Y);
        //        Location nextDirection = new Location(hotspot.X - next.X, hotspot.Y - next.Y);

        //        if (prevDirection == nextDirection)
        //        {
        //            waypointsToRemove.Add(hotspot);
        //        }
        //    }

        //    for (int i = 0; i < waypointsToRemove.Count; i++)
        //    {
        //        m_waypoints.Remove(waypointsToRemove[i]);
        //    }
        //}
    }
}