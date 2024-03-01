using Godot;
using System;
using ToolShed.Debug;

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

	public void ClearMaze()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}