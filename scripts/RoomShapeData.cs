using System.Collections.Generic;
using Godot;
using ToolShed.Debug;
using Grid = ToolShed.MazeGeneration.Grid;

public partial class RoomShapeData : Resource
{
    public int numDoors;
    public RoomInstance.RoomShape shape;
    public byte[] data;
    public Flattened2DArray<byte> shapeData;
    
    int neighborIndex = 0;
    int neighborIndex2 = 0;

    private bool neighborIsValid => neighborIndex >= 0 && neighborIndex < data.Length;
    private bool neighborEmpty => neighborIsValid && data[neighborIndex] == 0;
    private bool neighborIsEmptyOrInvalid => !neighborIsValid || neighborEmpty;
    bool neighbor2IsValid => neighborIndex2 >= 0 && neighborIndex2 < data.Length;
    bool neighbor2Empty => neighbor2IsValid && data[neighborIndex2] == 0;
    private bool neighbor2IsEmptyOrInvalid => !neighbor2IsValid || neighbor2Empty;
    
    private const byte WALL = 1 << 0;
    private const byte DOOR = 1 << 1;
    private const byte WALKABLE = 1 << 2;
    
    bool neighborIsWall => data[neighborIndex] == WALL;
    bool neighborIsDoor => data[neighborIndex] == DOOR;
    bool neighborIsWalkable => data[neighborIndex] == WALKABLE;
    bool neighbor2IsWalkable => neighbor2IsValid && data[neighborIndex2] == WALKABLE;

    public void RunGetRoomGrid()
    {
        var g = GetRoomGrid();
        GameDebug.Log(g.ToString());
    }
    
    public Grid GetRoomGrid()
    {
        int x = 0;
        int z = 0;
        neighborIndex = 0;
        neighborIndex2 = 0;
        int dataSize = Mathf.RoundToInt(Mathf.Sqrt(data.Length));
        int size = Mathf.RoundToInt((dataSize - 1) * 0.5f);
        Grid grid = new Grid(size, size);
        for (int dataIndex = 0; dataIndex < data.Length; dataIndex++)
        {
            GetXZFromFlatArray(dataIndex, dataSize, ref x, ref z);
            
            int gridX = Mathf.FloorToInt((x - 1) * 0.5f);
            int gridY = Mathf.FloorToInt((z - 1) * 0.5f);
            
            bool isGridCoord = x % 2 == 1 && z % 2 == 1;    // Both are odd
            if (!isGridCoord) continue;
            bool isWalkable = data[dataIndex] == WALKABLE;
            if (!isWalkable) grid.RemoveCell(gridX, gridY);
            // Debug.Log($"[{dataIndex}] - ({x},{z}) - Grid ({gridX},{gridY})");
            
            // Check up
            GetIndexFromXZ(x, z + 1, dataSize, ref neighborIndex);
            GetIndexFromXZ(x, z + 2, dataSize, ref neighborIndex2);

            if (isWalkable && !neighborIsWall && neighbor2IsWalkable)
            {
                grid[gridX, gridY].Link(grid[gridX, gridY + 1]);
            }

            // Check Right
            GetIndexFromXZ(x + 1, z, dataSize, ref neighborIndex);
            GetIndexFromXZ(x + 2, z, dataSize, ref neighborIndex2);
            
            if (isWalkable && !neighborIsWall && neighbor2IsWalkable)
            {
                grid[gridX, gridY].Link(grid[gridX + 1, gridY]);
            }
        }
        
        return grid;
    }
    
    // public List<GridPainter.PainterCell> ConvertToPainterData(ref List<SpawnInstructionGuide> sig)
    // {
    //     List<GridPainter.PainterCell> pCells = new List<GridPainter.PainterCell>();
    //     Vector3Int offset = new Vector3Int(-2, 0, -2);
    //
    //     int dataSize = Mathf.RoundToInt(Mathf.Sqrt(data.Length));
    //     int size = Mathf.RoundToInt((dataSize - 1) * 0.5f);
    //     
    //     int x = 0;
    //     int z = 0;
    //     neighborIndex = 0;
    //     neighborIndex2 = 0;
    //     
    //     for (int dataIndex = 0; dataIndex < data.Length; dataIndex++)
    //     {
    //         GetXZFromFlatArray(dataIndex, dataSize, ref x, ref z);
    //         
    //         int gridX = Mathf.FloorToInt((x - 1) * 0.5f);
    //         int gridY = Mathf.FloorToInt((z - 1) * 0.5f);
    //
    //         bool isGridCoord = x % 2 == 1 && z % 2 == 1;    // Both are odd
    //         if (!isGridCoord) continue;
    //         
    //         var gridPos = new Vector3Int(gridX + offset.x, 0, gridY + offset.z);
    //         //Debug.Log($"[{dataIndex}] - ({x},{z}) - Grid ({gridPos.x - offset.x},{gridPos.z - offset.z})");
    //
    //         bool isWalkable = data[dataIndex] == WALKABLE;
    //         bool isDoor = data[dataIndex] == DOOR;
    //         if (isWalkable)
    //         {
    //             var painterCell = new GridPainter.PainterCell
    //             {
    //                 pos = gridPos,
    //                 inGrid = true
    //             };
    //             pCells.Add(painterCell);
    //         }
    //
    //         int addWallInstructionId = 2;
    //         int clearWallInstructionId = 1;
    //         int doorInstructionId = 0;
    //         
    //         Quaternion upRotation = Quaternion.identity;
    //         Quaternion rightRotation = Quaternion.Euler(0, 90, 0);
    //         Quaternion downRotation = Quaternion.Euler(0, 180, 0);
    //         Quaternion leftRotation = Quaternion.Euler(0, 270, 0);
    //         
    //         SpawnInstructionGuide instruction;
    //         
    //         // Check up
    //         GetIndexFromXZ(x, z + 1, dataSize, ref neighborIndex);
    //         GetIndexFromXZ(x, z + 2, dataSize, ref neighborIndex2);
    //
    //         if (isWalkable && neighborIsEmptyOrInvalid && !neighbor2IsWalkable)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = clearWallInstructionId;
    //             instruction.rot = upRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         if ((neighborIsWall && data[dataIndex] == 0 && !neighbor2IsWalkable) ||
    //             (neighborIsWall && isWalkable && neighbor2IsWalkable))
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = addWallInstructionId;
    //             instruction.rot = upRotation;
    //             sig.Add(instruction);
    //         }
    //
    //         if (neighborIsValid && neighborIsDoor)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = doorInstructionId;
    //             instruction.rot = upRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         // Check Right
    //         GetIndexFromXZ(x + 1, z, dataSize, ref neighborIndex);
    //         GetIndexFromXZ(x + 2, z, dataSize, ref neighborIndex2);
    //
    //         if (gridX == 0 && gridY == 2)
    //         {
    //             Debug.Log("Stop");
    //         }
    //         if (isWalkable && neighborIsEmptyOrInvalid && !neighbor2IsWalkable)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = clearWallInstructionId;
    //             instruction.rot = rightRotation;
    //             sig.Add(instruction);
    //         }
    //
    //         if ((neighborIsWall && data[dataIndex] == 0 && !neighbor2IsWalkable) ||
    //             (neighborIsWall && isWalkable && neighbor2IsWalkable))
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = addWallInstructionId;
    //             instruction.rot = rightRotation;
    //             sig.Add(instruction);
    //         }
    //
    //         if (neighborIsValid && neighborIsDoor)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = doorInstructionId;
    //             instruction.rot = rightRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         // Check left
    //         GetIndexFromXZ(x - 1, z, dataSize, ref neighborIndex);
    //         GetIndexFromXZ(x - 2, z, dataSize, ref neighborIndex2);
    //         
    //         if (isWalkable && neighborIsEmptyOrInvalid && !neighbor2IsWalkable)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = clearWallInstructionId;
    //             instruction.rot = leftRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         if (neighborIsWall && data[dataIndex] == 0 && !neighbor2IsWalkable && gridX == 0)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = addWallInstructionId;
    //             instruction.rot = leftRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         if (neighborIsValid && neighborIsDoor && (neighbor2IsValid || data[neighborIndex2] != WALKABLE) && gridX == 0)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = doorInstructionId;
    //             instruction.rot = leftRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         // Check down
    //         GetIndexFromXZ(x, z - 1, dataSize, ref neighborIndex);
    //         GetIndexFromXZ(x, z - 2, dataSize, ref neighborIndex2);
    //         
    //         if (isWalkable && neighborIsEmptyOrInvalid && !neighbor2IsWalkable)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = clearWallInstructionId;
    //             instruction.rot = downRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         if (neighborIsWall && data[dataIndex] == 0 && !neighbor2IsWalkable && gridY == 0)
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = addWallInstructionId;
    //             instruction.rot = downRotation;
    //             sig.Add(instruction);
    //         }
    //         
    //         if (neighborIsValid && neighborIsDoor && (!neighbor2IsValid|| neighbor2Empty))
    //         {
    //             instruction = new SpawnInstructionGuide { pos = gridPos, UseDirection = true };
    //             instruction.Id = doorInstructionId;
    //             instruction.rot = downRotation;
    //             sig.Add(instruction);
    //         }
    //     }
    //     
    //     return pCells;
    // }

    public void GetXZFromFlatArray(int index, int arrayLength, ref int x, ref int y)
    {
        x = index % arrayLength;
        y = index / arrayLength;
    }

    public void GetIndexFromXZ(int x, int y, int width, ref int index)
    {
        index = x + y * width;
    }
}