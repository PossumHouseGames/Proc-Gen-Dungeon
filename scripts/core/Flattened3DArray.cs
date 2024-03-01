public struct Flattened3DArray<T>
{
    public int width; // X Right
    public int depth; // Z Forward
    public int height; // Y Up
    public T[] array;

    public int Length => array.Length;

    public Flattened3DArray(int width, int depth, int height)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        array = new T[width * depth * height];
    }

    public ref T this[int index] => ref array[index];
    public ref T this[int x, int y, int z] => ref this[CoordToIndex(x, y, z)];

    public ref T this[(int x, int y, int z) coord] => ref this[CoordToIndex(coord.x, coord.y, coord.z)];

    public (int, int, int) IndexToCoord(int index)
    {
        return (index % width, index / (width * depth), (index / depth) % width);
    }

    private int CoordToIndex(int x, int y, int z)
    {
        return x + y * width * depth + z * depth;
    }

    public bool CoordIsValid((int x, int y, int z) coord)
    {
        return coord.x >= 0 && coord.x < width &&
               coord.y >= 0 && coord.y < height &&
               coord.z >= 0 && coord.z < depth;
    }
}