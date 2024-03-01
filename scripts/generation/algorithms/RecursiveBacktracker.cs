using System.Collections.Generic;
using System.Linq;
using ToolShed.Utilities;

namespace ToolShed.MazeGeneration
{
    /// <summary>
    /// The Recursive Backtracker Algorithm
    /// - Starting at an arbitrary location, perform a random walk, avoiding previously visited cells.
    ///   When no moves are possible, backtrack to the most recently visited cell and resume the random
    ///   walk from there. The algorithm ends when it tries to backtrack from the cell where it started.
    /// - Long, twisty passages ("high river"), with relatively few dead ends. Closely related to Hunt-and-Kill
    ///   but potentially faster, as it is guaranteed to visit every cell only twice, though it needs considerably
    ///   more memory to keep track of previously visited cells.  
    /// </summary>
    public class RecursiveBacktracker
    {
        public static void Run(ref Grid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton, grid.GetRandomCell(Utilities.Utilities.RngSingleton));
        }

        public static void Run(ref Grid grid, System.Random rng, Cell startAt)
        {
            Stack<Cell> stack = new Stack<Cell>();
            stack.Push(startAt);

            while(stack.Count > 0)
            {
                var current = stack.Peek();
                var neighbors = current.Neighbors.Where(n => n.Links.Length == 0).ToList();

                if(neighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    var neighbor = neighbors.Random(rng);
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }
        }
        
        public static void Run(ref MaskedGrid grid)
        {
            Run(ref grid, Utilities.Utilities.RngSingleton, grid.GetRandomCell(Utilities.Utilities.RngSingleton));
        }

        public static void Run(ref MaskedGrid grid, System.Random rng, Cell startAt)
        {
            Stack<Cell> stack = new Stack<Cell>();
            stack.Push(startAt);

            while(stack.Count > 0)
            {
                var current = stack.Peek();
                var neighbors = current.Neighbors.Where(n => n.Links.Length == 0).ToList();

                if(neighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    var neighbor = neighbors.Random(rng);
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }
        }
    }
}