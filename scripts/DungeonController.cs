using Godot;
using System;
using ToolShed.Debug;
using ToolShed.MazeGeneration;

public partial class DungeonController : Node
{
	[Export] public bool randomizeSeed;
	[Export] public int seed;

	private Random ProcRandom;
	private Flattened3DArray<RoomInstance> _currentRooms;
	
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
		
		_currentRooms = new Flattened3DArray<RoomInstance>(5, 5, 4);

	}

	private void InitDungeonGrid()
	{
		
	}

	private void InitRoomInstances()
	{
		
	}

	private void InitRoomVisuals()
	{
		
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