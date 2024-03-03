using Godot;
using System;
using ToolShed.Debug;

public partial class InputHandler : Node
{
	public const string MOVE_FORWARD = "Move Forward";
	public const string MOVE_BACK = "Move Back";
	public const string ROTATE_LEFT = "Rotate Left";
	public const string ROTATE_RIGHT = "Rotate Right";
	public const string TOGGLE_SPELL_CASTING = "Toggle Spell Casting";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(MOVE_FORWARD))
		{
			GameDebug.Log("Move Forward Input just pressed");
		}
		
		if (Input.IsActionJustPressed(MOVE_BACK))
		{
			GameDebug.Log("Move Back Input just pressed");
		}
	}

	public bool RotateLeft => Input.IsActionJustPressed(ROTATE_LEFT);
	public bool RotateRight => Input.IsActionJustPressed(ROTATE_RIGHT);
	public bool MoveForward => Input.IsActionJustPressed(MOVE_FORWARD);
	public bool MoveBack => Input.IsActionJustPressed(MOVE_BACK);
}
