using Godot;

public partial class RoomData : Resource
{
    public enum RoomFloor
    {
        Basement = -1,
        Ground = 0,
        Upper = 1,
        Roof = 2
    }

    [ExportGroup("Floor")] 
    public bool Roof;
    public bool Upper;
    public bool Ground;
    public bool Basement;
}

/*
public static class RoomShapeHelper
{
    [MenuItem("Tools/Generate RoomShapeData")]
    public static void GenerateRoomShapeData()
    {
        string shapeDataImage = "Assets/Art/RoomShapes.png";
        // string shapeDataImage = "Assets/Art/Test Room.png";
        string targetDirectory = "Assets/Data/RoomShapeData";

        Texture2D tex = ConvertFileToTexture2D($"{Application.dataPath}{shapeDataImage.Replace("Assets", "")}");
        Color[] imageData = tex.GetPixels();
        Debug.Log("Got Pixels");

        Vector2Int roomSize = new Vector2Int(5, 5);
        Vector2Int imageRoomSize = new Vector2Int(roomSize.x * 2 + 1, roomSize.y * 2 + 1);
        int totalColumns = tex.width / imageRoomSize.x;
        int totalRows = tex.height / imageRoomSize.y;
        byte wall = 1 << 0;
        byte door = 1 << 1;
        byte walkable = 1 << 2;
        Debug.Log($"Found {totalColumns} Columns and {totalRows} Rows in texture");
        List<byte> roomData = new List<byte>();
        for (int c = 0; c < totalColumns; c++)
        {
            for (int r = 0; r < totalRows; r++)
            {
                roomData.Clear();
                int xOffset = c * imageRoomSize.x;
                int yOffset = r * imageRoomSize.y;
                Vector2Int bottomLeftCorner = new Vector2Int(xOffset, yOffset);
                Debug.Log($"Processing Shape at ({totalColumns},{totalRows}) with bottomLeftCorner at {bottomLeftCorner}.");
                for (int y = 0; y < imageRoomSize.y; y++)
                {
                    for (int x = 0; x < imageRoomSize.x; x++)
                    {
                        var xCoord = x + bottomLeftCorner.x;
                        var yCoord = y + bottomLeftCorner.y;
                        Color color = tex.GetPixel(xCoord, yCoord);
                        if (color.a == 0)
                        {
                            roomData.Add(0);
                            continue;
                        }
                        
                        if (Color.black == color)
                        {
                            roomData.Add(wall);
                            Debug.Log($"Found wall ({c},{r})-({xCoord},{yCoord})");
                        }
                        else if ( color == Color.red)
                        {
                            roomData.Add(door);
                            Debug.Log($"Found door ({c},{r})-({xCoord},{yCoord})");
                        }
                        else if (color == Color.green)
                        {
                            roomData.Add(walkable);
                            Debug.Log($"Found walkable space ({c},{r})-({xCoord},{yCoord})");
                        }
                        else
                        {
                            roomData.Add(1 << 6);
                            Debug.Log($"Found color({c},{r})-({x},{yCoord}) {color}");
                        }
                    }
                }

                if (roomData.Any(val => val != 0))
                {
                    string label = r switch
                    {
                        0 => "Cross",
                        1 => "T",
                        2 => "I",
                        3 => "L",
                        4 => "End",
                        _ => ""
                    };

                    var assetName = $"RoomShapeData_{label}_{c}";
                    var assetPath = $"{targetDirectory}/{assetName}.asset";
                    var rsd = AssetDatabase.LoadAssetAtPath<RoomShapeData>(assetPath);
                    if (rsd == null)
                    {
                        rsd = ScriptableObject.CreateInstance<RoomShapeData>();
                        AssetDatabase.CreateAsset(rsd, assetPath);
                    }
                    
                    rsd.data = roomData.ToArray();
                    rsd.numDoors = roomData.Count(cell => cell == door);
                    rsd.shape = r switch
                    {
                        0 => RoomInstance.RoomShape.Cross,
                        1 => RoomInstance.RoomShape.T,
                        2 => RoomInstance.RoomShape.I,
                        3 => RoomInstance.RoomShape.L,
                        4 => RoomInstance.RoomShape.End,
                        _ => rsd.shape
                    };
                    EditorUtility.SetDirty(rsd);
                    
                    // Load or create prefab
                    string defaultPrefabPath = "Assets/Prefabs/DefaultPrefabs/RoomShapeController.prefab";
                    string targetPrefabPath = $"Assets/Prefabs/RoomShapes/";
                    string targetEndPath = $"{targetPrefabPath}{assetName.Replace("Data", "")}.prefab";
                    var roomShapeController = AssetDatabase.LoadAssetAtPath<RoomShapeController>(targetEndPath);
                    if (roomShapeController == null)
                    {
                        AssetDatabase.CopyAsset(defaultPrefabPath, targetEndPath);
                    }

                    var rscPrefab = PrefabUtility.LoadPrefabContents(targetEndPath);
                    var rsc = rscPrefab.GetComponent<RoomShapeController>();
                    rsc.data = rsd;
                    rsc.Generate();
                    EditorUtility.SetDirty(rsc);
                    PrefabUtility.SaveAsPrefabAsset(rscPrefab, targetEndPath);
                    PrefabUtility.UnloadPrefabContents(rscPrefab);
                }
            }
        }

        AssetDatabase.Refresh();
    }
    
    [MenuItem("Tools/Generate Entryway RoomShapeData")]
    public static void GenerateRoomShapeData_Entryways()
    {
        string shapeDataImage = "Assets/Art/Entryway.png";
        // string shapeDataImage = "Assets/Art/Test Room.png";
        string targetDirectory = "Assets/Data/RoomShapeData";

        Texture2D tex = ConvertFileToTexture2D($"{Application.dataPath}{shapeDataImage.Replace("Assets", "")}");
        Color[] imageData = tex.GetPixels();
        Debug.Log("Got Pixels");

        Vector2Int roomSize = new Vector2Int(5, 5);
        Vector2Int imageRoomSize = new Vector2Int(roomSize.x * 2 + 1, roomSize.y * 2 + 1);
        int totalColumns = tex.width / imageRoomSize.x;
        int totalRows = tex.height / imageRoomSize.y;
        byte wall = 1 << 0;
        byte door = 1 << 1;
        byte walkable = 1 << 2;
        Debug.Log($"Found {totalColumns} Columns and {totalRows} Rows in texture");
        List<byte> roomData = new List<byte>();
        for (int c = 0; c < totalColumns; c++)
        {
            for (int r = 0; r < totalRows; r++)
            {
                roomData.Clear();
                int xOffset = c * imageRoomSize.x;
                int yOffset = r * imageRoomSize.y;
                Vector2Int bottomLeftCorner = new Vector2Int(xOffset, yOffset);
                Debug.Log($"Processing Shape at ({totalColumns},{totalRows}) with bottomLeftCorner at {bottomLeftCorner}.");
                for (int y = 0; y < imageRoomSize.y; y++)
                {
                    for (int x = 0; x < imageRoomSize.x; x++)
                    {
                        var xCoord = x + bottomLeftCorner.x;
                        var yCoord = y + bottomLeftCorner.y;
                        Color color = tex.GetPixel(xCoord, yCoord);
                        if (color.a == 0)
                        {
                            roomData.Add(0);
                            continue;
                        }
                        
                        if (Color.black == color)
                        {
                            roomData.Add(wall);
                            Debug.Log($"Found wall ({c},{r})-({xCoord},{yCoord})");
                        }
                        else if ( color == Color.red)
                        {
                            roomData.Add(door);
                            Debug.Log($"Found door ({c},{r})-({xCoord},{yCoord})");
                        }
                        else if (color == Color.green)
                        {
                            roomData.Add(walkable);
                            Debug.Log($"Found walkable space ({c},{r})-({xCoord},{yCoord})");
                        }
                        else
                        {
                            roomData.Add(1 << 6);
                            Debug.Log($"Found color({c},{r})-({x},{yCoord}) {color}");
                        }
                    }
                }

                if (roomData.Any(val => val != 0))
                {
                    string label = c switch
                    {
                        0 => "Entryway",
                        1 => "Foyer",
                        2 => "EntrywayUpper",
                        3 => "FoyerUpper",
                        _ => ""
                    };

                    var assetName = $"RoomShapeData_{label}";
                    var assetPath = $"{targetDirectory}/{assetName}.asset";
                    var rsd = AssetDatabase.LoadAssetAtPath<RoomShapeData>(assetPath);
                    if (rsd == null)
                    {
                        rsd = ScriptableObject.CreateInstance<RoomShapeData>();
                        AssetDatabase.CreateAsset(rsd, assetPath);
                    }
                    
                    rsd.data = roomData.ToArray();
                    rsd.numDoors = roomData.Count(cell => cell == door);
                    rsd.shape = r switch
                    {
                        0 => RoomInstance.RoomShape.Cross,
                        1 => RoomInstance.RoomShape.T,
                        2 => RoomInstance.RoomShape.I,
                        3 => RoomInstance.RoomShape.L,
                        4 => RoomInstance.RoomShape.End,
                        _ => rsd.shape
                    };
                    EditorUtility.SetDirty(rsd);
                    
                    // Load or create prefab
                    string defaultPrefabPath = "Assets/Prefabs/DefaultPrefabs/RoomShapeController.prefab";
                    string targetPrefabPath = $"Assets/Prefabs/RoomShapes/";
                    string targetEndPath = $"{targetPrefabPath}{assetName.Replace("Data", "")}.prefab";
                    var roomShapeController = AssetDatabase.LoadAssetAtPath<RoomShapeController>(targetEndPath);
                    if (roomShapeController == null)
                    {
                        AssetDatabase.CopyAsset(defaultPrefabPath, targetEndPath);
                    }

                    var rscPrefab = PrefabUtility.LoadPrefabContents(targetEndPath);
                    var rsc = rscPrefab.GetComponent<RoomShapeController>();
                    rsc.data = rsd;
                    rsc.painter.ReGenerate();
                    EditorUtility.SetDirty(rsc);
                    PrefabUtility.SaveAsPrefabAsset(rscPrefab, targetEndPath);
                    PrefabUtility.UnloadPrefabContents(rscPrefab);
                }
            }
        }

        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// Loads a file stored at a path and returns it as a Texure2D in Unity.
    /// </summary>
    /// <returns>The file to texture2D.</returns>
    /// <param name="path">Path to file.</param>
    public static Texture2D ConvertFileToTexture2D(string path)
    {
        Texture2D texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        byte[] readBytes;
        try
        {
            readBytes = File.ReadAllBytes(path);
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("Caught exception when trying to load texture from file: " + e.ToString());
            return null;
        }

        texture.LoadImage(readBytes);
        return texture;
    }
}
*/