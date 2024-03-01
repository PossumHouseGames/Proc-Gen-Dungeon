namespace ToolShed.MazeGeneration
{
    public class MaskedGrid : Grid<Cell>
    {
        public override int Size => CurrentMask.Count;


        public MaskedGrid(Mask mask) : base(mask)
        {
        }

        public MaskedGrid(int rows, int columns) : base(rows, columns)
        {
        }
        
        public MaskedGrid(Grid<Cell> grid) : base(grid)
        {
        }

        public void Initialize(Mask mask)
        {
            base.Initialize(mask.Rows, mask.Columns);
        }

        private Cell[,] PrepareGrid()
        {
            Cell[,] grid = new Cell[Rows, Columns];

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (CurrentMask != null && CurrentMask[row,column])
                        grid[row, column] = new Cell(row, column);
                }
            }

            return grid;
        }

        public override Cell GetRandomCell(System.Random rng)
        {
            int row = 0;
            int column = 0;
            if (CurrentMask != null)
                CurrentMask.GetRandomLocation(rng, out row, out column);

            return this[row, column];
        }
    }
}