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
	private Tween _tween;

	private bool _isTweening => _tween != null && _tween.IsRunning();

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
			// RotationDegrees = Vector3.Up * _myGridObject.rotation;

			SyncRotation();
		}
		if (_inputHandler.RotateRight)
		{
			_myGridObject.RotateRight();
			//RotationDegrees = Vector3.Up * _myGridObject.rotation;
			
			SyncRotation();
		}

		if (_inputHandler.MoveForward)
		{
			_myGridObject.MoveForward();
			// Position = new Vector3(-_myGridObject.position.X, 0, _myGridObject.position.Y); 

			SyncPosition();
		}
		if (_inputHandler.MoveBack)
		{
			_myGridObject.MoveBack();
			// Position = new Vector3(-_myGridObject.position.X, 0, _myGridObject.position.Y); 
			
			SyncPosition();
		}
	}

	private void SyncPosition()
	{
		var targetPos = new Vector3(-_myGridObject.position.X, 0, _myGridObject.position.Y);
		_tween = CreateTween();
		_tween.TweenProperty(this, "position", targetPos, 0.2f);
		_tween.TweenCallback(Callable.From(SetTweenComplete));
		GameDebug.Log($"Player at {_myGridObject.position}, facing {_myGridObject.faceDirection}");
	}

	private void SetTweenComplete()
	{
		GameDebug.Log("Position Tweening Finished");
	}

	private void SyncRotation()
	{
		var targetRot = SetRotationBeforeTween();
		_tween = CreateTween();
		_tween.TweenProperty(this, "rotation_degrees", targetRot, 0.2f);

		GameDebug.Log(_myGridObject.ToString());
		GameDebug.Log($"Player at {_myGridObject.position}, facing {_myGridObject.faceDirection}");
	}

	private Vector3 SetRotationBeforeTween()
	{
		var beforeChange = RotationDegrees;
		RotationDegrees = _myGridObject.rotation switch
		{
			270 when Math.Abs(RotationDegrees.Y - 0) < 0.002f => Vector3.Up * 360,
			90 when Math.Abs(RotationDegrees.Y - 360) < 0.002f => Vector3.Zero,
			0 when Mathf.Abs(RotationDegrees.Y - 270) < 0.002f => Vector3.Up * -90,
			_ => RotationDegrees
		};

		var beforeTargetRotChange = _myGridObject.rotation;
		Vector3 targetRot = _myGridObject.rotation switch	
		{
			270 when RotationDegrees.Y is >= -90 and <= 0 => Vector3.Up * -90,
			0 when RotationDegrees.Y is >= 270 and <= 360 => Vector3.Up * 360,
			_ => Vector3.Up * _myGridObject.rotation
		};
		
		GameDebug.Log($"{(_isTweening ? "Mid" : "Before")} Tween Rotation Change from {beforeChange} to {RotationDegrees}, target from {beforeTargetRotChange} to {targetRot}");

		return targetRot;
	}
}
