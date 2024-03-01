public struct Flattened2DArray<T>
{
    public int width; // X Right
    public int height; // Y Up

    public T[] array;
    
    public int Length => array.Length;

    public Flattened2DArray(int width, int height)
    {
        this.width = width;
        this.height = height;
        array = new T[width * height];
    }

    public ref T this[int index] => ref array[index];

    public ref T this[int x, int y] => ref this[CoordToIndex(x, y)];

    public (int, int) IndexToCoord(int index)
    {
        return (index % Length, index / Length);
    }

    public int CoordToIndex(int x, int y)
    {
        return x + y * width;
    }


    public bool CoordIsValid((int x, int y) coord)
    {
        return coord.x >= 0 && coord.x < width &&
               coord.y >= 0 && coord.y < height;
    }
}