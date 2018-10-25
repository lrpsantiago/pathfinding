
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding
{
    public interface IPath
    {
        Location Front { get; }
        uint OriginalSize { get; }
        uint Size { get; }
        void PopFront();
        IPath Clone();
        IList<Location> ToList();
    }
}
