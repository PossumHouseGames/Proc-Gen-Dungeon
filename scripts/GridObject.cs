using Godot;

namespace ProcGenDungeon.scripts;

public struct GridObject
{
    public Vector2I position;
    public int rotation;
    public FaceDirection faceDirection;

    private GridManager _grid;
    public enum FaceDirection{ North, East, Up, South, West, Down }

    public override string ToString()
    {
        return $"GridObject {position}:{rotation}:{faceDirection}";
    }

    public void Init(GridManager gridManager, Vector2I position)
    {
        _grid = gridManager;
        this.position = position;
    }

    public void RotateLeft()
    {
        SetRotation(rotation + 90);
    }
    public void RotateRight()
    {
        SetRotation(rotation - 90);
    }

    private void SetRotation(int newRotation)
    {
        rotation = (newRotation + 720) % 360;

        faceDirection = rotation switch
        {
            0 => FaceDirection.North,
            90 => FaceDirection.West,
            180 => FaceDirection.South,
            270 => FaceDirection.East,
            _ => faceDirection
        };
    }

    public void MoveForward()
    {
        _grid.TryMove(ref this, GetForwardOffset());
    }

    public void MoveBack()
    {
        _grid.TryMove(ref this, -GetForwardOffset());
    }

    private Vector2I GetForwardOffset()
    {
        return rotation switch
        {
            0 => -Vector2I.Up, // GODOT Y up is negative
            90 => -Vector2I.Right,
            180 => -Vector2I.Down, // GODOT Y up is negative
            270 => -Vector2I.Left,
            _ => Vector2I.Zero
        };
    }
}