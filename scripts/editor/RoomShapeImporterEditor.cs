#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Tool]
public partial class RoomShapeImporterEditor : EditorScript
{
	public enum TileTypes
	{
		Empty = 0,
		Wall = 1 << 0,
		Door = 1 << 1,
		Walkable = 1 << 2,
	}

	// Called when the script is executed (using File -> Run in Script Editor).
	public override void _Run()
	{
		GD.Print("Running Csharp version!");
		
		string shapeDataImage = "res://art/RoomShapes.png";
		// string shapeDataImage = "Assets/Art/Test Room.png";
		string targetDirectory = "res://data/room_shape_data";
		Image shapes = Image.LoadFromFile(shapeDataImage);
		GD.Print($"Got Pixels {shapes.GetWidth()}x{shapes.GetHeight()}");

		Vector2I roomSize = new Vector2I(5, 5);
		Vector2I imageRoomSize = new Vector2I(roomSize.X * 2 + 1, roomSize.Y * 2 + 1);
		int totalColumns = shapes.GetWidth() / imageRoomSize.X;
		int totalRows = shapes.GetHeight() / imageRoomSize.Y;
		GD.Print($"Found {totalColumns} Columns and {totalRows} Rows in texture");
		List<byte> roomData = new List<byte>();
		for (int c = 0; c < totalColumns; c++)
        {
            for (int r = 0; r < totalRows; r++)
            {
                roomData.Clear();
                int xOffset = c * imageRoomSize.X;
                int yOffset = r * imageRoomSize.Y;
                Vector2I bottomLeftCorner = new Vector2I(xOffset, yOffset);
                GD.Print($"Processing Shape at ({totalColumns},{totalRows}) with bottomLeftCorner at {bottomLeftCorner}.");
                Image roomImage = Image.Create(imageRoomSize.X, imageRoomSize.Y, false, Image.Format.Rgba8);
                roomImage.Fill(Colors.White);
                for (int y = 0; y < imageRoomSize.Y; y++)
                {
                    for (int x = 0; x < imageRoomSize.X; x++)
                    {
                        var xCoord = x + bottomLeftCorner.X;
                        var yCoord = y + bottomLeftCorner.Y;
                        Color color = shapes.GetPixel(xCoord, yCoord);
                        if (color.A == 0)
                        {
                            roomData.Add(0);
                            continue;
                        }
                        
                        if (Colors.Black == color)
                        {
                            roomData.Add((byte)TileTypes.Wall);
                            // GD.Print($"Found wall ({c},{r})-({xCoord},{yCoord})");
                        }
                        else if ( color == Colors.Red)
                        {
                            roomData.Add((byte)TileTypes.Door);
                            // GD.Print($"Found door ({c},{r})-({xCoord},{yCoord})");
                        }
                        else if (color == Colors.Green)
                        {
                            roomData.Add((byte)TileTypes.Walkable);
                            // GD.Print($"Found walkable space ({c},{r})-({xCoord},{yCoord})");
                        }
                        else
                        {
                            roomData.Add(1 << 6);
                            // GD.Print($"Found color({c},{r})-({x},{yCoord}) {color}");
                        }
                        
                        roomImage.SetPixel(x, y, color);
                        
                    }
                }

                
                if (roomData.Any(val => val != 0))
                {
                    string label = r switch
                    {
                        0 => "End",
                        1 => "L",
                        2 => "I",
                        3 => "T",
                        4 => "Cross",
                        _ => ""
                    };

                    var assetName = $"RoomShapeData_{label}_{c}";
                    var assetPath = $"{targetDirectory}/{assetName}.tres";
                    // GD.Print($"Would write to target: {assetPath}");

                    // var rsd = GD.Load<RoomShapeData>(assetPath);
                    // if (rsd == null)
                    // {
	                   //  rsd = new RoomShapeData();
                    // }

                    var rsd = new RoomShapeData();
                    
                    
                    rsd.data = roomData.ToArray();
                    rsd.shapeData.array = roomData.ToArray();

                    rsd.numDoors = roomData.Count(cell => cell == (byte)TileTypes.Door);
                    rsd.shape = r switch
                    {
	                    0 => RoomInstance.RoomShape.End,
	                    1 => RoomInstance.RoomShape.L,
	                    2 => RoomInstance.RoomShape.I,
	                    3 => RoomInstance.RoomShape.T,
                        4 => RoomInstance.RoomShape.Cross,
	                    _ => rsd.shape
                    };
                    rsd.roomImage = ImageTexture.CreateFromImage(roomImage);
                    var result = ResourceSaver.Save(rsd, assetPath);

                    // var rsd = AssetDatabase.LoadAssetAtPath<RoomShapeData>(assetPath);
                    // if (rsd == null)
                    // {
                    //     rsd = ScriptableObject.CreateInstance<RoomShapeData>();
                    //     AssetDatabase.CreateAsset(rsd, assetPath);
                    // }
                    //
                    // rsd.data = roomData.ToArray();
                    // rsd.numDoors = roomData.Count(cell => cell == door);
                    // rsd.shape = r switch
                    // {
                    //     0 => RoomInstance.RoomShape.Cross,
                    //     1 => RoomInstance.RoomShape.T,
                    //     2 => RoomInstance.RoomShape.I,
                    //     3 => RoomInstance.RoomShape.L,
                    //     4 => RoomInstance.RoomShape.End,
                    //     _ => rsd.shape
                    // };
                    // EditorUtility.SetDirty(rsd);

                    // // Load or create prefab
                    // string defaultPrefabPath = "Assets/Prefabs/DefaultPrefabs/RoomShapeController.prefab";
                    // string targetPrefabPath = $"Assets/Prefabs/RoomShapes/";
                    // string targetEndPath = $"{targetPrefabPath}{assetName.Replace("Data", "")}.prefab";
                    // var roomShapeController = AssetDatabase.LoadAssetAtPath<RoomShapeController>(targetEndPath);
                    // if (roomShapeController == null)
                    // {
                    //     AssetDatabase.CopyAsset(defaultPrefabPath, targetEndPath);
                    // }
                    //
                    // var rscPrefab = PrefabUtility.LoadPrefabContents(targetEndPath);
                    // var rsc = rscPrefab.GetComponent<RoomShapeController>();
                    // rsc.data = rsd;
                    // rsc.Generate();
                    // EditorUtility.SetDirty(rsc);
                    // PrefabUtility.SaveAsPrefabAsset(rscPrefab, targetEndPath);
                    // PrefabUtility.UnloadPrefabContents(rscPrefab);
                }
            }
        }


	}
}
#endif
