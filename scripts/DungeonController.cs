using Godot;
using System;
using ToolShed.Debug;
using ToolShed.MazeGeneration;

public partial class DungeonController : Node
{
	[Export] public bool randomizeSeed;
	[Export] public int seed;
	[Export] public RoomsList _roomsList;


	private Random ProcRandom;
	private Flattened3DArray<RoomInstance> _currentRooms;
	public DungeonGrid dungeonGrid;
	
	[Export] Vector3 _roomSize = new(5, 2, 5);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get Maze
		// Place rooms
		GameDebug.Log("Gonna make a dungeon");
		Init();
		
	}
	
	public void ClearMaze()
	{
		
	}

	private void Init()
	{
		ClearMaze();

		if (randomizeSeed)
		{
			var r = new Random();
			seed = r.Next();
		}
		
		ProcRandom = new Random(seed);

		_currentRooms = new Flattened3DArray<RoomInstance>(5, 5, 4);
		InitDungeonGrid();
		InitRoomInstances();
		InitRoomVisuals();
	}

	private void InitDungeonGrid()
	{
		dungeonGrid = new DungeonGrid(5, 5);
		// dungeonGrid.AddLevel(false);
		// dungeonGrid.AddLevel(false);
		// dungeonGrid.AddLevel(false);

		// var basementStairsCellCoord = (2, 0, 1);
		// var entryWayCellCoord = (2, 1, 0);
		// var entryWayUpperCellCoord = (2, 2, 0);
		// var foyerCellCoord = (2, 1, 1);
		// var foyerUpperCellCoord = (2, 2, 1);
		// var roofLandingCellCoord = (2, 3, 1);
		//
		// ref var basementStairsCell = ref dungeonGrid.GetRoomCellRef(basementStairsCellCoord);
		// ref var entryWayCell = ref dungeonGrid.GetRoomCellRef(entryWayCellCoord);
		// ref var entryWayUpperCell = ref dungeonGrid.GetRoomCellRef(entryWayUpperCellCoord);
		// ref var foyerCell = ref dungeonGrid.GetRoomCellRef(foyerCellCoord);
		// ref var foyerUpperCell = ref dungeonGrid.GetRoomCellRef(foyerUpperCellCoord);
		// ref var roofLandingCell = ref dungeonGrid.GetRoomCellRef(roofLandingCellCoord);
		//
		// // Link vertical neighbors 
		// basementStairsCell.AddNeighbor(foyerCell, Cell.NeighborDirection.Up);
		// foyerCell.AddNeighbor(foyerUpperCell, Cell.NeighborDirection.Up);
		// foyerUpperCell.AddNeighbor(roofLandingCell, Cell.NeighborDirection.Up);

		HuntAndKill.Run<DungeonGrid, Cell>(ref dungeonGrid, ProcRandom);
  //       
		// _exclusionCells.Add(entryWayCell);
		// _exclusionCells.Add(foyerCell);
		// _exclusionCells.Add(entryWayUpperCell);
		// _exclusionCells.Add(foyerUpperCell);
		// _exclusionCells.Add(roofLandingCell);
	}

	private void InitRoomInstances()
	{
		for (int level = 0; level < dungeonGrid.Levels; level++)
		{
			for (int row = 0; row < dungeonGrid.Rows; row++)
			{
				for (int column = 0; column < dungeonGrid.Columns; column++)
				{
					var cell = dungeonGrid[column, level, row];
					RoomInstance ri = new RoomInstance();
					// ri.data = _roomsList.GetRandom(ProcRandom);

					RoomInstance.RoomShape targetShape = RoomInstance.RoomShape.Cross;

					if (cell.IsEndCell())
						targetShape = RoomInstance.RoomShape.End;
					else if (cell.IsICell())
						targetShape = RoomInstance.RoomShape.I;
					else if (cell.IsLCell())
						targetShape = RoomInstance.RoomShape.L;
					else if (cell.IsTCell())
						targetShape = RoomInstance.RoomShape.T;
					else if (cell.IsCrossCell())
						targetShape = RoomInstance.RoomShape.Cross;

					var randomRoom = _roomsList.GetRandomShape(ProcRandom, targetShape);
					ri.RoomPrefab = randomRoom;
					ri.rotation = cell.GetRotation();
                    
					_currentRooms[column, level, row] = ri;
				}
			}
		}
	}

	private void InitRoomVisuals()
	{
		for (int roomIndex = 0; roomIndex < _currentRooms.Length; roomIndex++)
        {
            (int x, int y, int z) cellCoord = _currentRooms.IndexToCoord(roomIndex);
            var worldPosition = new Vector3(-cellCoord.x * _roomSize.X, cellCoord.y * _roomSize.Y,
                cellCoord.z * _roomSize.Z);
            ref var room = ref _currentRooms[roomIndex];
            room.RenderRoom(worldPosition);
            room.controller.Name = $"({cellCoord.x},{cellCoord.y},{cellCoord.z})_{room.controller.Name}";
            AddChild(room.controller);
            // room.controller.UpdateHalfWalls(cellCoord.y == 1);
            // room.controller._rotation = room.rotation;
            // switch ((RoomData.RoomFloor)cellCoord.y - 1)
            // {
            //     case RoomData.RoomFloor.Basement:
            //         room.controller.pare.SetParent(_basementContainer);
            //         break;
            //     case RoomData.RoomFloor.Ground:
            //         room.controller.transform.SetParent(_groundContainer);
            //         break;
            //     case RoomData.RoomFloor.Upper:
            //         room.controller.transform.SetParent(_upperContainer);
            //         break;
            //     case RoomData.RoomFloor.Roof:
            //         room.controller.transform.SetParent(_roofContainer);
            //         break;
            // }
            
            // Check Right for shared walls
            // var rightNeighborCoord = cellCoord;
            // rightNeighborCoord.x++;
            // if (_currentRooms.CoordIsValid(rightNeighborCoord))
            // {
            //     var neighbor = _currentRooms[rightNeighborCoord];
            //
            //     var thisRoomGrid = room.RoomPrefab.data.GetRoomGrid();
            //     var neighborGrid = neighbor.RoomPrefab.data.GetRoomGrid();
            //
            //     thisRoomGrid.RotateGrid(room.rotation);
            //     neighborGrid.RotateGrid(neighbor.rotation);
            //     
            //     foreach (var gridCell in thisRoomGrid.EachCellInColumn(thisRoomGrid.Columns - 1))
            //     {
            //         if (gridCell == null)
            //             continue;
            //
            //         if (neighborGrid[0, gridCell.Level, gridCell.Row] != null)
            //         {
            //             // Debug.Log($"{room.controller.gameObject.name} Room Cell {gridCell} shares wall with neighbor at {neighborGrid[0, gridCell.Level, gridCell.Row]}", room.controller.gameObject);
            //             // Disable those walls in 
            //             Vector3I originalCoord = new Vector3I(gridCell.Column, gridCell.Level, gridCell.Row);
            //             if (room.rotation != 0)
            //             {
            //                 Vector3I rotatedCoord = new Vector3I(gridCell.Column, gridCell.Level, gridCell.Row);
            //                 int reverseRotation = (360 - room.rotation) % 360;
            //                 originalCoord = thisRoomGrid.MapFromLocalCoordinateWithRotationAndOffset(rotatedCoord, reverseRotation);
            //
            //                 // Debug.Log($"Room Rotation {room.rotation} reverse Rot {reverseRotation} Looking for {originalCoord}, from rotated {rotatedCoord}");
            //             }
            //             
            //             if (room.controller.TryGetWall(originalCoord.x, originalCoord.z, RoomController.CardinalDirection.East, out var walls))
            //             {
            //                 foreach (var wall in walls)
            //                 {
            //                     wall.wall.gameObject.ToggleActive(false);
            //                 }
            //             }
            //         }
            //     }
            // }

            // Check up for shared walls
            // var upNeighborCoord = cellCoord;
            // upNeighborCoord.z++;
            // if (_currentRooms.CoordIsValid(upNeighborCoord))
            // {
            //     var neighbor = _currentRooms[upNeighborCoord];
            //
            //     var thisRoomGrid = room.RoomPrefab.data.GetRoomGrid();
            //     var neighborGrid = neighbor.RoomPrefab.data.GetRoomGrid();
            //
            //     thisRoomGrid.RotateGrid(room.rotation);
            //     neighborGrid.RotateGrid(neighbor.rotation);
            //     
            //     foreach (var gridCell in thisRoomGrid.EachCellInRow(thisRoomGrid.Rows - 1))
            //     {
            //         if (gridCell == null)
            //             continue;
            //
            //         if (neighborGrid[gridCell.Column, gridCell.Level, 0] != null)
            //         {
            //             // Debug.Log($"{room.controller.gameObject.name} Room Cell {gridCell} shares wall with neighbor at {neighborGrid[0, gridCell.Level, gridCell.Row]}", room.controller.gameObject);
            //             // Disable those walls in 
            //             Vector3I originalCoord = new Vector3I(gridCell.Column, gridCell.Level, gridCell.Row);
            //             if (room.rotation != 0)
            //             {
            //                 Vector3I rotatedCoord = new Vector3I(gridCell.Column, gridCell.Level, gridCell.Row);
            //                 int reverseRotation = (360 - room.rotation) % 360;
            //                 originalCoord = thisRoomGrid.MapFromLocalCoordinateWithRotationAndOffset(rotatedCoord, reverseRotation);
            //
            //                 // Debug.Log($"Room Rotation {room.rotation} reverse Rot {reverseRotation} Looking for {originalCoord}, from rotated {rotatedCoord}");
            //             }
            //             
            //             if (room.controller.TryGetWall(originalCoord.x, originalCoord.z, RoomController.CardinalDirection.North, out var walls))
            //             {
            //                 foreach (var wall in walls)
            //                 {
            //                     wall.wall.gameObject.ToggleActive(false);
            //                 }
            //             }
            //         }
            //     }
            // }
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

public class DungeonGrid : Grid<Cell>
{
	public DungeonGrid(Mask mask) : base(mask)
	{
	}

	public DungeonGrid(int rows, int columns, int levels = 1) : base(rows, columns, levels)
	{
	}

	public DungeonGrid(Grid<Cell> grid) : base(grid)
	{
	}
}