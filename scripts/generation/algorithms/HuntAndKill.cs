using System.Linq;
using ToolShed.Utilities;

namespace ToolShed.MazeGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public class HuntAndKill
    {
        // public static void Run(ref Grid grid)
        // {
        //     Run(ref grid, Utilities.Utilities.RngSingleton);
        // }
        //
        // public static void Run(ref Grid grid, System.Random rng)
        // {
        //     var current = grid.GetRandomCell(rng);
        //
        //     while (current != null)
        //     {
        //         List<Cell> unvisitedNeighbors = current.Neighbors.Where(x => x.Links.Length == 0).ToList();
        //         
        //         if(unvisitedNeighbors.Any())
        //         {
        //             var neighbor = unvisitedNeighbors.Random(rng);
        //             current.Link(neighbor);
        //             current = neighbor;
        //         }
        //         else
        //         {
        //             current = null;
        //
        //             foreach (var cell in grid.EachCell())
        //             {
        //                 var visitedNeighbors = cell.Neighbors.Where(c => c.Links.Length > 0).ToList();
        //
        //                 if (cell.Links.Length == 0 && visitedNeighbors.Count > 0)
        //                 {
        //                     current = cell;
        //                     var neighbor = visitedNeighbors.Random(rng);
        //                     current.Link(neighbor);
        //                     break;
        //                 }
        //             }
        //         }
        //     }
        // }
        
        public static void Run<T, V>(ref T grid) where T : Grid<V> where V : Cell
        {
            Run<T,V>(ref grid, Utilities.Utilities.RngSingleton);
        }

        public static void Run<T, V>(ref T grid, System.Random rng) where V : Cell where T : Grid<V> 
        {
            var current = grid.GetRandomCell(rng);

            while (current != null)
            {
                var unvisitedNeighbors = current.GetNeighbors<V>().Where<V>(x => x.Links.Length == 0).ToList();
                if (unvisitedNeighbors.Any())
                {
                    var neighbor = unvisitedNeighbors.Random(rng);
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (var cell in grid.EachCell())
                    {
                        if (cell == null) continue;
                        
                        var visitedNeighbors = cell.GetNeighbors<V>().Where<V>(c => c.Links.Length > 0).ToList();

                        if (cell.Links.Length == 0 && visitedNeighbors.Count > 0)
                        {
                            current = cell;
                            var neighbor = visitedNeighbors.Random(rng);
                            current.Link(neighbor);
                            break;
                        }
                    }
                }
            }
        }
        // public static void Run(ref ColoredGrid grid)
        // {
        //     Run(ref grid, Utilities.Utilities.RngSingleton);
        // }
        //
        // public static void Run(ref ColoredGrid grid, System.Random rng)
        // {
        //     var current = grid.GetRandomCell(rng);
        //
        //     while (current != null)
        //     {
        //         List<Cell> unvisitedNeighbors = current.Neighbors.Where(x => x.Links.Length == 0).ToList();
        //
        //         if (unvisitedNeighbors.Count() > 0)
        //         {
        //             var neighbor = unvisitedNeighbors.Random();
        //             current.Link(neighbor);
        //             current = neighbor;
        //         }
        //         else
        //         {
        //             current = null;
        //
        //             foreach (var cell in grid.EachCell())
        //             {
        //                 var visitedNeighbors = cell.Neighbors.Where(c => c.Links.Length > 0).ToList();
        //
        //                 if (cell.Links.Length == 0 && visitedNeighbors.Count > 0)
        //                 {
        //                     current = cell;
        //                     var neighbor = visitedNeighbors.Random();
        //                     current.Link(neighbor);
        //                     break;
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}
