
namespace PushingBoxStudios.Pathfinding
{
    public abstract class AbstractPathfinder
    {
        public PathfindingStatistics Statistics { get; private set; }
        
        public EProbeMode ProbeMode { get; set; }

        public AbstractPathfinder()
        {
            this.Statistics = new PathfindingStatistics();
            this.ProbeMode = EProbeMode.EightDirections;
        }

        public abstract Path FindPath(Grid grid, Location start, Location goal);
        //{
        //    return this.FindPath(grid, start.X, start.Y, goal.X, goal.Y);
        //}

        //public abstract Path FindPath(Grid grid, ushort startX, ushort startY, ushort goalX, ushort goalY);
    }
}
