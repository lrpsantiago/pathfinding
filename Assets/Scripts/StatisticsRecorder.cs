using PushingBoxStudios.Pathfinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assets.Scripts
{
    public class StatisticsRecorder
    {
        public static StatisticsRecorder _instance;
        private IList<PathfindingStatisticsRecord> _records;

        public static StatisticsRecorder Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StatisticsRecorder();
                }

                return _instance;
            }
        }

        public StatisticsRecorder()
        {
            _records = new List<PathfindingStatisticsRecord>();
        }

        public void ClearRecords()
        {
            _records.Clear();
        }

        public void Add(PathfindingStatisticsRecord statistics)
        {
            _records.Add(statistics);
        }

        public void SaveAsCsvFile(string fileName)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Index;Time Lapsed;Interations;Grid Nodes;Opened Nodes;Closed Nodes;Path Length;Path Cost");

            for (int i = 0; i < _records.Count; i++)
            {
                var line = CreateCsvLine(i, _records[i]);
                builder.AppendLine(line);
            }

            var strContent = builder.ToString();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            File.WriteAllText(desktopPath + "\\" + fileName + ".csv", strContent);
        }

        private string CreateCsvLine(int index, PathfindingStatisticsRecord statstics)
        {
            return index + ";" +
                statstics.TimeLapsed + ";" +
                statstics.IterationsCount + ";" +
                statstics.TotalGridNodes + ";" +
                statstics.OpenedNodesCount + ";" +
                statstics.ClosedNodesCount + ";" +
                statstics.PathLength + ";" +
                statstics.PathCost;
        }
    }
}
