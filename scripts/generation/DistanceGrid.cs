namespace ToolShed.MazeGeneration
{
    public class DistanceGrid : Grid<Cell>
    {
        public Distances Distances { get; set; }

        public DistanceGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public override string ContentsOf(Cell cell)
        {
            if (Distances != null && Distances.Cells.ContainsKey(cell))
                return Base36Converter.ConvertTo(Distances[cell]);
            else
                return base.ContentsOf(cell);
        }
    }
}

public static class Base36Converter
{
    private const int Base = 36;
    private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string ConvertTo(int value)
    {
        string result = "";
        if (value == 0) result = "0";
        while (value > 0)
        {
            result = Chars[value % Base] + result; // use StringBuilder for better performance
            value /= Base;
        }

        return result;
    }
}