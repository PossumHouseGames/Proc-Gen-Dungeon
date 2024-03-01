using System.Collections.Generic;

namespace ToolShed.MazeGeneration
{
    public class Distances
    {
        private Cell _root;

        public Dictionary<Cell, int> Cells { get; } = new();

        public Distances(Cell root)
        {
            Initialize(root);
        }

        void Initialize(Cell root)
        {
            _root = root;
            Cells.Add(root, 0);
        }

        public int this[Cell cell]
        {
            get => Cells.TryGetValue(cell, out var foundDistance) ? foundDistance : int.MaxValue;

            set
            {
                if (!Cells.ContainsKey(cell))
                    Cells.Add(cell, value);
                else
                    Cells[cell] = value;
            }
        }

        public Distances PathTo(Cell goal)
        {
            var current = goal;

            var breadcrumbs = new Distances(_root)
            {
                [current] = Cells[current]
            };

            while (current != _root)
            {
                foreach (var neighbor in current.Links)
                {
                    if (Cells[neighbor] >= Cells[current]) continue;
                    breadcrumbs[neighbor] = Cells[neighbor];
                    current = neighbor;
                }
            }

            return breadcrumbs;
        }

        public KeyValuePair<Cell, int> Max()
        {
            int maxDistance = 0;
            var maxCell = _root;

            foreach (var cell in Cells.Keys)
            {
                if(Cells[cell] > maxDistance)
                {
                    maxCell = cell;
                    maxDistance = Cells[cell];
                }
            }
            return new KeyValuePair<Cell, int>(maxCell, maxDistance);
        }

        public override string ToString()
        {
            string output = "Path: \n";

            foreach (var kv in Cells)
            {
                output += $"{kv.Value}: {kv.Key.ToString()}\n";
            }
            
            return output;
        }
    }
}
