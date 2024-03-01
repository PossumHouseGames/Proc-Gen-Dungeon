using System.Collections.Generic;
using ToolShed.Utilities;

namespace ToolShed.MazeGeneration
{
    /// <summary>
    /// The Sidewinder algorithm
    /// - Consider the grid one row at a time. For each row, link random runs of
    ///     adjacent cells, and then carve north from a random cell in each run.
    ///     Treat the norther row specially, linking all the cells into a single
    ///     corridor.
    /// - A strong vertical texture. One corridor runs the length of the northern row.
    ///     Difficult to use with masks and non-rectangular grids.
    /// - The texture changes if you choose south instead of north, or if you run the
    ///     algorithm by columns instead of rows and choose east or west instead of
    ///     north. Easily adapted to 3D by choosing between north and (for example), up
    ///     or down for each run. 
    /// </summary>
    public class Sidewinder
    {
        public static void Run(ref Grid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }
        public static void Run(ref MaskedGrid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton);
        }
        public static void Run(ref MaskedGrid grid, System.Random rng)
        {
            foreach (var row in grid.EachRow())
            {
                List<Cell> run = new List<Cell>();
                
                foreach (var cell in row)
                {
                    if (cell == null) continue;
                    
                    run.Add(cell);

                    bool atEasternBoundary = cell.East == null;
                    bool atNorthernBoundary = cell.North == null;

                    // If at eastern boundary, cant carve east so close out
                    // If not at northern boundary, we can still roll 50:50
                    // to choose whether keep going east or choose north somewhere
                    // in the run. 
                    bool shouldCloseOut = atEasternBoundary ||
                                          (!atNorthernBoundary && rng.NextBool());

                    if (shouldCloseOut)
                    {
                        var member = run.Random(rng);
                        if (member.North != null) member.Link(member.North);
                        run.Clear();
                    }
                    else // Otherwise keep carving east
                        cell.Link(cell.East);
                }                
            }
        }
        public static void Run(ref Grid grid, System.Random rng)
        {
            foreach (var row in grid.EachRow())
            {
                List<Cell> run = new List<Cell>();
                
                foreach (var cell in row)
                {
                    run.Add(cell);

                    bool atEasternBoundary = cell.East == null;
                    bool atNorthernBoundary = cell.North == null;

                    // If at eastern boundary, cant carve east so close out
                    // If not at northern boundary, we can still roll 50:50
                    // to choose whether keep going east or choose north somewhere
                    // in the run. 
                    bool shouldCloseOut = atEasternBoundary ||
                        (!atNorthernBoundary && rng.NextBool());

                    if (shouldCloseOut)
                    {
                        var member = run.Random(rng);
                        if (member.North != null) member.Link(member.North);
                        run.Clear();
                    }
                    else // Otherwise keep carving east
                        cell.Link(cell.East);
                }                
            }
        }
    }
}