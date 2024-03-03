using Godot;

public partial class RoomShapeController : Node
{
    [Export]
    public RoomShapeData data;

    [Export] public PackedScene gridTile;

    [Export] public PackedScene wallTile;
    [Export] public PackedScene floorTile;

    public override void _Ready()
    {
        base._Ready();
        Render();
    }

    public void Render()
    {
        for (int rawX = 0; rawX < 11; rawX++)
        {
            for (int rawZ = 0; rawZ < 11; rawZ++)
            {
                int x = Mathf.FloorToInt((rawX - 1) * 0.5f);
                int z = Mathf.FloorToInt((rawZ - 1) * 0.5f);

                bool isGridCoord = rawX % 2 == 1 && rawZ % 2 == 1;    // Both are odd
                if (!isGridCoord) continue;
                
                var tile = (Node3D) gridTile.Instantiate();
                tile.Position = new Vector3(-x, 0, z);
                tile.Name = $"Tile_{tile.Position}";

                var floor = floorTile.Instantiate();
                tile.AddChild(floor);
                
                
                AddChild(tile);
                
                
            }
        }
    }

    public void Clear()
    {
        
    }

}