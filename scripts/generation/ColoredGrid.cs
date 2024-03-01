using System.Collections.Generic;
using Godot;
using ToolShed.Debug;

namespace ToolShed.MazeGeneration
{
    public class ColoredGrid : Grid<Cell>
    {
        public Distances Distances { get; set; }
        public KeyValuePair<Cell, int> Maximum => Distances.Max();


        public ColoredGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public ColoredGrid(Grid<Cell> grid) : base(grid)
        { }

        public override Color BackgroundColorFor(Cell cell)
        {
            var distance = Distances[cell];
            var intensity = (float) (Maximum.Value - distance) / (float) Maximum.Value;
            GameDebug.Log("Intensity: " + intensity);
            var dark = Mathf.Round(255 * intensity);
            var bright = 128 + Mathf.Round(127 * intensity);
            return new Color(dark/255f, bright/255f, dark/255f);
        }
    }
}
