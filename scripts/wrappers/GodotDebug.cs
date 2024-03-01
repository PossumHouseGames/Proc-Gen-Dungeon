using Godot;
using System;
using ToolShed.Debug;

public partial class GodotDebug : Node, IDebugWrapper
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameDebug.RegisterDebugWrapper(this);
	}

	public new string Name => "Godot Debug";

	public void Log(string message)
	{
		GD.Print(message);
	}

	public void LogWarning(string message)
	{
		// GD.Print($"WARNING: {message}");
		GD.PushWarning($"WARNING: {message}");

	}

	public void LogError(string message)
	{
		// GD.PrintErr($"ERROR: {message}");
		GD.PushError($"ERROR: {message}");
	}
}
