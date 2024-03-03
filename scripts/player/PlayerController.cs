using Godot;
using System;
using ProcGenDungeon.scripts;
using ToolShed.Debug;

public partial class PlayerController : Node3D
{
	private InputHandler _inputHandler;
	private GridManager _gridManager;
		
	private GridObject _myGridObject;

	private Camera3D _myCam;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inputHandler = GetNode<InputHandler>("/root/InputHandler");
		_gridManager = GetNode<GridManager>("/root/GridManager");
		
		_gridManager.RegisterGridObject(ref _myGridObject, Vector2I.Zero);
		_myCam = GetChild<Camera3D>(0);
		if (_myCam == null)
		{
			GameDebug.LogWarning($"Unable to get child camera");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_inputHandler.RotateLeft)
		{
			_myGridObject.RotateLeft();
			RotationDegrees = Vector3.Up * _myGridObject.rotation;
			GameDebug.Log(_myGridObject.ToString());
			GameDebug.Log($"Player at {_myGridObject.position}, facing {_myGridObject.faceDirection}");
		}
		if (_inputHandler.RotateRight)
		{
			_myGridObject.RotateRight();
			RotationDegrees = Vector3.Up * _myGridObject.rotation;
			GameDebug.Log(_myGridObject.ToString());
			GameDebug.Log($"Player at {_myGridObject.position}, facing {_myGridObject.faceDirection}");
		}

		if (_inputHandler.MoveForward)
		{
			_myGridObject.MoveForward();
			Position = new Vector3(-_myGridObject.position.X, 0, _myGridObject.position.Y);
			// Tween tween = CreateTween();
			// tween.TweenProperty(this, "position", _myGridObject.position, 0.2f);
			GameDebug.Log($"Player at {_myGridObject.position}, facing {_myGridObject.faceDirection}");
		}
		if (_inputHandler.MoveBack)
		{
			_myGridObject.MoveBack();
			Position = new Vector3(-_myGridObject.position.X, 0, _myGridObject.position.Y);
			// Tween tween = CreateTween();
			// tween.TweenProperty(this, "position", _myGridObject.position, 0.2f);
			GameDebug.Log($"Player at {_myGridObject.position}, facing {_myGridObject.faceDirection}");
		}
	}
}
