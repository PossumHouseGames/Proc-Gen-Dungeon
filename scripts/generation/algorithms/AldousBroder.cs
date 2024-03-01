using ToolShed.Utilities;

namespace ToolShed.MazeGeneration
{
    public class AldousBroder
    {
        public static void Run(ref Grid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }
        public static void Run(ref Grid grid, System.Random rng)
        {
            var cell = grid.GetRandomCell(rng);
            var unvisited = grid.Size - 1; 
            
            while (unvisited > 0)
            {
                var neighbor = cell.Neighbors.Random(rng);
                if(neighbor.Links.Length == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;
                }

                cell = neighbor;
            }
        }
        
        public static void Run(ref ColoredGrid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }

        public static void Run(ref ColoredGrid grid, System.Random rng)
        {
            var cell = grid.GetRandomCell(rng);
            var unvisited = grid.Size - 1;

            while (unvisited > 0)
            {
                var neighbor = cell.Neighbors.Random(rng);
                if (neighbor.Links.Length == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;
                }

                cell = neighbor;
            }
        }
    }
}
