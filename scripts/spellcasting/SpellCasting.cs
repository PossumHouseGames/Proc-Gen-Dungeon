using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ToolShed.Debug;
using ToolShed.Utilities;

public partial class SpellCasting : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}


    [Export] private Node _spellCircle;
    [Export] private float _castingRadius = 1f;
    public float CastingRadius => _castingRadius;
    [Export] private Transform3D _spellCursor;
    [Export] private SpellNode[] _spellNodes;
    [Export] private Line2D _symbolLine;

    [ExportGroup("Broadcasting On")]
    // [Export] private IntEventChannelSO _spellCastingEnabled;
    private bool _spellCasting;
    private bool _drawing;
    private bool _triggered;
    private List<int> _symbolPath = new List<int>();

    public List<int> SymbolPath => _symbolPath;
    [Export] private float _inputThreshold = 0.3f;

    [Export] private int _cursorIndex = 8;
    [Export] private float _cursorSnapSpeed = 0.2f;

    private Rune _currentRune;

    [Export] private RuneController _runeVisualizerPrefab;
    private bool _canChangeNode = true;

    private void HandleNodeEntered()
    {
        if (_drawing || _triggered)
        {
            // Debug.Log($"Node entered: {nodeIndex}");

            bool clearToAddNode = true;

            // Make sure last entry isnt this new index
            if (_symbolPath.Count >= 1 && _symbolPath[_symbolPath.Count - 1] == _cursorIndex)
                clearToAddNode = false;

            if (clearToAddNode && _symbolPath.Count > 1 && _symbolPath[_symbolPath.Count - 2] == _cursorIndex)
                clearToAddNode = false;

            // if (clearToAddNode)
            // {
            //     _symbolPath.Add(_cursorIndex);
            //
            //     _symbolLine.positionCount++;
            //
            //     Vector3 pointPos = Utilities.GetPositionInCircle(Vector3.zero, _cursorIndex * 45f, 0.4f);
            //     pointPos.z = _symbolLine.transform.localPosition.z;
            //
            //     if (_triggered)
            //         _symbolLine.SetPosition(_symbolLine.positionCount - 1, pointPos);
            //     else
            //         _symbolLine.SetPosition(_symbolLine.positionCount - 2, pointPos);
            //
            //     if (_symbolLine.positionCount == 1)
            //         _symbolLine.positionCount++;
            //
            //     _spellNodes[_cursorIndex].Select();
            // }

            _triggered = false;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ProcessInput();

        // if (_spellCasting)
        // {
        //     Vector2 inputDirection = InputManager.InputActions.LookInput;
        //     //inputDirection.y *= -1;
        //     //inputDirection /= 300f; // make it 0-1
        //
        //     if (inputDirection == Vector2.zero && !_canChangeNode)
        //         _canChangeNode = true;
        //
        //     if (inputDirection.magnitude > _inputThreshold && _canChangeNode)
        //     {
        //         float inputAngle = Vector3.Angle(Vector3.right, inputDirection);
        //         // Debug.Log($"Input angle: {Mathf.Sign(inputDirection.y)*inputAngle}");
        //         if (Mathf.Sign(inputDirection.y) == -1f)
        //             inputAngle = 360 - inputAngle;
        //
        //         if (inputAngle > 359)
        //             inputAngle -= 360;
        //
        //         int targetNodeIndex = Mathf.RoundToInt(inputAngle / 45f);
        //         if (targetNodeIndex > 7)
        //             targetNodeIndex -= 8;
        //
        //         // Debug.Log($"Angel {inputAngle}, Node: {targetNodeIndex}");
        //
        //         if (targetNodeIndex != _cursorIndex)
        //         {
        //             if (Mathf.Abs(_cursorIndex - targetNodeIndex) == 4 && _cursorIndex != 8)
        //             {
        //                 _spellCursor
        //                     .DOLocalMove(Vector3.zero, _cursorSnapSpeed)
        //                     .OnComplete(CursorMoveComplete);
        //                 if (_cursorIndex < _spellNodes.Length)
        //                     _spellNodes[_cursorIndex].Normal();
        //                 _spellNodes[8].Highlight();
        //                 _cursorIndex = 8;
        //                 _canChangeNode = false;
        //             }
        //             else
        //             {
        //                 _spellCursor
        //                     .DOLocalMove(Utilities.GetPositionInCircle(Vector3.zero, targetNodeIndex * 45f, 0.4f), _cursorSnapSpeed)
        //                     .OnComplete(CursorMoveComplete);
        //                 if (_cursorIndex < _spellNodes.Length)
        //                     _spellNodes[_cursorIndex].Normal();
        //                 else
        //                     _spellNodes[8].Normal();
        //                 _cursorIndex = targetNodeIndex;
        //                 if (_cursorIndex < _spellNodes.Length)
        //                     _spellNodes[_cursorIndex].Highlight();
        //             }
        //
        //             if (_drawing) // RT is still pressed
        //             {
        //                 HandleNodeEntered();
        //             }
        //         }
        //     }
        //
        //     if (_symbolLine.positionCount > 1)
        //     {
        //         var pointPos = _spellCursor.localPosition;
        //         pointPos.z = _symbolLine.transform.localPosition.z;
        //         _symbolLine.SetPosition(_symbolLine.positionCount - 1, pointPos);
        //     }
        // }
    }

    private void ProcessInput()
    {
        // if (InputManager.InputActions.AimInput)
        // {
        //     UndoPreviousNode();
        // }
        //
        // if (InputManager.InputActions.FireInput)
        // {
        //     AcceptNode();
        // }
        // else _drawing = false;
        //
        // if (InputManager.InputActions.ThrowInput)
        // {
        //     BeginSpell();
        // }
    }

    private void CursorMoveComplete()
    {
        if (_drawing) // RT is still pressed
        {
            // HandleNodeEntered());
        }
    }

    private void AcceptNode()
    {
        _drawing = true;

        if (!_spellCasting)
        {
            return;
        }

        _triggered = true;
        HandleNodeEntered();
        
    }

    private void UndoPreviousNode()
    {
        // if (_symbolPath.Count > 0)
        // {
        //     if (_symbolLine.positionCount <= 2)
        //     {
        //         _symbolLine.positionCount = 0;
        //         _spellNodes[_symbolPath[0]].Deselect();
        //         _symbolPath.Clear();
        //     }
        //     else
        //     {
        //         _symbolLine.positionCount--;
        //         _spellNodes[_symbolPath[_symbolPath.Count - 1]].Deselect();
        //         _symbolPath.RemoveAt(_symbolPath.Count - 1);
        //     }
        // }
    }

    private void BeginSpell()
    {
        GameDebug.Log("Starting Spell");
        // _spellCircle.transform.DOScale(_spellCasting ? 0 : 0.5f, 0.25f);
        // _spellCircle.transform.rotation = Quaternion.LookRotation(_spellCircle.transform.position - Camera.main.transform.position);
        // _spellCasting = !_spellCasting;
        //
        // _spellCastingEnabled?.RaiseEvent(_spellCasting ? 1 : 0);        
    }

    public void Clear()
    {
        // _symbolPath.Clear();
        // _symbolLine.positionCount = 0;
        // foreach ( SpellNode s in _spellNodes)
        // {
        //     s.Deselect();
        // }
    }

    public RuneController GetRune()
    {
        // RuneController rune = Instantiate(_runeVisualizerPrefab, transform.position, transform.rotation);
        // rune.transform.localScale = _spellCircle.transform.localScale;
        // rune.Radius = _castingRadius;
        // rune.Set(new Rune(_symbolPath.ToArray()));
        return null;
    }
}

