namespace ToolShed.MazeGeneration
{
    public class Mask
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public bool[,] MaskArray { get; private set; }
    
        public int Count 
        {
            get
            {
                int count = 0;
                for (int x = 0; x < Rows; x++)
                    for (int y = 0; y < Columns; y++)
                        if (MaskArray[x, y])
                            count++;
                return count;
            }
        }
        
        public new bool this[int row, int column]
        {
            get
            {
                if (row < 0 || row > Rows - 1 ||
                    column < 0 || column > Columns - 1)
                    return false;
                
                return MaskArray[row, column];
            }
            
            set => MaskArray[row, column] = value;
        }
        
        public Mask(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            MaskArray = new bool[Rows, Columns];

            for (int row = 0; row < Rows; row++)
                for (int column = 0; column < Columns; column++)
                    MaskArray[row, column] = true;
        }

        public void GetRandomLocation(System.Random rng, out int row, out int column)
        {
            while (true)
            { 
                row = rng.Next(Rows);
                column = rng.Next(Columns);
                if (this[row, column]) break;
            }
        }
    }
}