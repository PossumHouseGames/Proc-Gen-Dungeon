using System.Collections.Generic;
using ToolShed.Utilities;

namespace ToolShed.MazeGeneration
{
    public class Wilsons
    {
        public static void Run(ref Grid grid)
        {
            var unvisited = new List<Cell>();
            foreach (var cell in grid.EachCell())
            {
                unvisited.Add(cell);
            }

            var first = unvisited.Random();

            unvisited.Remove(first);

            while (unvisited.Count > 0)
            {
                var cell = unvisited.Random();
                var path = new List<Cell>();
                path.Add(cell);

                while (unvisited.Contains(cell))
                {
                    cell = cell.Neighbors.Random();
                    var position = path.IndexOf(cell);

                    // If the cell is in the path, returns -1 if its not
                    if (position >= 0)
                    {
                        // we hit a loop. truncate path after this cell
                        path = path.Truncate(0, position);
                    }
                    else
                    {
                        path.Add(cell);
                    }
                }

                for (int i = 0; i < path.Count - 1; i++)
                {
                    path[i].Link(path[i + 1]);
                    unvisited.Remove(path[i]);
                }
            }
        }

        public static void Run(ref ColoredGrid grid)
        {
            var unvisited = new List<Cell>();
            foreach (var cell in grid.EachCell())
            {
                unvisited.Add(cell);
            }

            var first = unvisited.Random();

            unvisited.Remove(first);

            while (unvisited.Count > 0)
            {
                var cell = unvisited.Random();
                var path = new List<Cell>();
                path.Add(cell);

                while (unvisited.Contains(cell))
                {
                    cell = cell.Neighbors.Random();
                    var position = path.IndexOf(cell);

                    // If the cell is in the path, returns -1 if its not
                    if (position >= 0)
                    {
                        // we hit a loop. truncate path after this cell
                        path = path.Truncate(0, position);
                    }
                    else
                    {
                        path.Add(cell);
                    }
                }

                for (int i = 0; i < path.Count - 1; i++)
                {
                    path[i].Link(path[i + 1]);
                    unvisited.Remove(path[i]);
                }
            }
        }
    }
}
