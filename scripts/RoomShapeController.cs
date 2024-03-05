using Godot;

public partial class RoomShapeController : Node
{
    [Export]
    public RoomShapeData data;

    [Export] public PackedScene gridTile;

    [Export] public PackedScene wallTile;
    [Export] public PackedScene floorTile;
    [Export] public PackedScene doorTile;

    public override void _Ready()
    {
        base._Ready();
        Render();
    }

    public void Render()
    {
        int imageSize = Mathf.RoundToInt(Mathf.Sqrt(data.data.Length));
        data.shapeData = new Flattened2DArray<byte>(imageSize, imageSize)
        {
            array = data.data,
        };

        int gridSize = (imageSize - 1) / 2;
        
        for (int rawX = 0; rawX < imageSize; rawX++)
        {
            for (int rawZ = 0; rawZ < imageSize; rawZ++)
            {
                int x = Mathf.FloorToInt((rawX - 1) * 0.5f);
                int z = gridSize - 1 - Mathf.FloorToInt((rawZ - 1) * 0.5f);

                bool isGridCoord = rawX % 2 == 1 && rawZ % 2 == 1;    // Both are odd
                if (!isGridCoord) continue;
                
                var tileType = (RoomShapeImporterEditor.TileTypes)data.shapeData[rawX, rawZ];

                var tile = (Node3D) gridTile.Instantiate();
                tile.Position = new Vector3(-x, 0, z);
                tile.Name = $"Tile_{tile.Position}";

                bool isWalkable = tileType == RoomShapeImporterEditor.TileTypes.Walkable;
                bool isDoor = tileType == RoomShapeImporterEditor.TileTypes.Door;
                bool isEmpty = tileType == RoomShapeImporterEditor.TileTypes.Empty;

                if (tileType == RoomShapeImporterEditor.TileTypes.Walkable)
                {
                    var floor = floorTile.Instantiate();
                    tile.AddChild(floor);
                }
                
                // Check up
                bool northNeighborIsValid = data.shapeData.CoordIsValid((rawX, rawZ - 1));
                bool northNeighborIsWall = northNeighborIsValid &&
                                           data.shapeData[rawX, rawZ - 1] ==
                                           (byte)RoomShapeImporterEditor.TileTypes.Wall;
                bool northNeighborIsWalkable = northNeighborIsValid &&
                                               data.shapeData[rawX, rawZ - 1] ==
                                               (byte)RoomShapeImporterEditor.TileTypes.Walkable;
                bool northNeighborIsDoor = northNeighborIsValid &&
                                           data.shapeData[rawX, rawZ - 1] ==
                                           (byte)RoomShapeImporterEditor.TileTypes.Door;


                if ((northNeighborIsWall && !northNeighborIsWalkable) || 
                    (northNeighborIsWall && isWalkable && northNeighborIsWalkable))
                {
                    var wall = wallTile.Instantiate();
                    tile.AddChild(wall);
                }

                if (northNeighborIsValid && northNeighborIsDoor)
                {
                    var door = doorTile.Instantiate();
                    tile.AddChild(door);
                }
                
                // Check right
                bool eastNeighborIsValid = data.shapeData.CoordIsValid((rawX + 1, rawZ));
                bool eastNeighborIsWall = eastNeighborIsValid &&
                                           data.shapeData[rawX + 1, rawZ] ==
                                           (byte)RoomShapeImporterEditor.TileTypes.Wall;
                bool eastNeighborIsWalkable = eastNeighborIsValid &&
                                               data.shapeData[rawX + 1, rawZ] ==
                                               (byte)RoomShapeImporterEditor.TileTypes.Walkable;
                bool eastNeighborIsDoor = eastNeighborIsValid &&
                                           data.shapeData[rawX + 1, rawZ] ==
                                           (byte)RoomShapeImporterEditor.TileTypes.Door;
                
                if ((eastNeighborIsWall && !eastNeighborIsWalkable) || 
                    (eastNeighborIsWall && isWalkable && eastNeighborIsWalkable))
                {
                    var wall = (Node3D) wallTile.Instantiate();
                    wall.RotationDegrees = Vector3.Up * 270;
                    tile.AddChild(wall);
                }
                
                if (eastNeighborIsValid && eastNeighborIsDoor)
                {
                    var door = (Node3D) doorTile.Instantiate();
                    door.RotationDegrees = Vector3.Up * 270;
                    tile.AddChild(door);
                }
                
                // Check left
                bool westNeighborIsValid = data.shapeData.CoordIsValid((rawX - 1, rawZ));
                bool westNeighborIsWall = westNeighborIsValid &&
                                           data.shapeData[rawX - 1, rawZ] ==
                                           (byte)RoomShapeImporterEditor.TileTypes.Wall;
                bool westNeighborIsWalkable = westNeighborIsValid &&
                                               data.shapeData[rawX - 1, rawZ] ==
                                               (byte)RoomShapeImporterEditor.TileTypes.Walkable;
                bool westNeighborIsDoor = westNeighborIsValid &&
                                          data.shapeData[rawX - 1, rawZ] ==
                                          (byte)RoomShapeImporterEditor.TileTypes.Door;

                if (westNeighborIsWall && !westNeighborIsWalkable && x == 0)
                {
                    var wall = (Node3D) wallTile.Instantiate();
                    wall.RotationDegrees = Vector3.Up * 90;
                    tile.AddChild(wall);
                }
                
                if (westNeighborIsValid && westNeighborIsDoor)
                {
                    var door = (Node3D) doorTile.Instantiate();
                    door.RotationDegrees = Vector3.Up * 90;
                    tile.AddChild(door);
                }
                
                // Check south
                bool southNeighborIsValid = data.shapeData.CoordIsValid((rawX, rawZ + 1));
                bool southNeighborIsWall = southNeighborIsValid &&
                                           data.shapeData[rawX, rawZ + 1] ==
                                           (byte)RoomShapeImporterEditor.TileTypes.Wall;
                bool southNeighborIsWalkable = southNeighborIsValid &&
                                               data.shapeData[rawX, rawZ + 1] ==
                                               (byte)RoomShapeImporterEditor.TileTypes.Walkable;
                bool southNeighborIsDoor = southNeighborIsValid &&
                                          data.shapeData[rawX, rawZ + 1] ==
                                          (byte)RoomShapeImporterEditor.TileTypes.Door;
                if (southNeighborIsWall && !southNeighborIsWalkable && z == 0)
                {
                    var wall = (Node3D) wallTile.Instantiate();
                    wall.RotationDegrees = Vector3.Up * 180;
                    tile.AddChild(wall);
                }
                if (southNeighborIsValid && southNeighborIsDoor)
                {
                    var door = (Node3D) doorTile.Instantiate();
                    door.RotationDegrees = Vector3.Up * 180;
                    tile.AddChild(door);
                }
                

                AddChild(tile);
            }
        }
    }

    public void Clear()
    {
        
    }

}