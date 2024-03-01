using System;
using System.Collections.Generic;

namespace ToolShed.MazeGeneration
{
    /// <summary>
    /// The Binary Tree algorithm
    /// - For each cell in the grid, carve either North or East
    /// - A strong diagonal texture, tending towards the north-east corner
    ///   of the grid. Corridors run the length of the northern row and eastern
    ///   column. Difficult to use with masks and non rectangular grids.
    /// - Variations: The texture changes if you choose south instead of north
    ///   or west instead of east. Also the algorithm is easily adapted to 3D
    ///   (and higher) grids by adding either up or down as a choice in each cell.
    /// - Tips: Its more difficult to navigate if you start at the north east end 
    /// </summary>
    public class BinaryTree
    {
        public static void Run(ref Grid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }
        
        public static void Run(ref Grid grid, System.Random rng)
        {
            // For each cell
            foreach (var cell in grid.EachCell())
            {
                if (cell != null)
                {
                    List<Cell> neighbors = new List<Cell>();
                    if (cell.North != null) neighbors.Add(cell.North);
                    if (cell.East != null) neighbors.Add(cell.East);

                    if (neighbors.Count > 0)
                    {
                        int index = rng.Next(0, neighbors.Count);
                        var neighbor = neighbors[index];

                        if (neighbor != null)
                            cell.Link(neighbor);
                    }
                }
            }
        }

        public static void Run(ref DistanceGrid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }

        public static void Run(ref DistanceGrid grid, System.Random rng)
        {
            foreach (var cell in grid.EachCell())
            {
                if (cell != null)
                {
                    List<Cell> neighbors = new List<Cell>();
                    if (cell.North != null) neighbors.Add(cell.North);
                    if (cell.East != null) neighbors.Add(cell.East);

                    if (neighbors.Count > 0)
                    {
                        int index = rng.Next(0, neighbors.Count);
                        var neighbor = neighbors[index];

                        if (neighbor != null)
                            cell.Link(neighbor);
                    }
                }
            }
        }

        public static void Run(ref ColoredGrid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }
        
        public static void Run(ref ColoredGrid grid, System.Random rng)
        {
            foreach (var cell in grid.EachCell())
            {
                if (cell != null)
                {
                    List<Cell> neighbors = new List<Cell>();
                    if (cell.North != null) neighbors.Add(cell.North);
                    if (cell.East != null) neighbors.Add(cell.East);

                    if (neighbors.Count > 0)
                    {
                        int index = rng.Next(0, neighbors.Count);
                        var neighbor = neighbors[index];

                        if (neighbor != null)
                            cell.Link(neighbor);
                    }
                }
            }
        }
    }
}
