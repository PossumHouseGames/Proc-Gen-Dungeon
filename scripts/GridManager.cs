using System.Collections.Generic;
using Godot;
using ToolShed.MazeGeneration;

namespace ProcGenDungeon.scripts;

public partial class GridManager : Node
{
    private Grid grid;

    private Dictionary<int, List<GridObject>> _gridMap = new Dictionary<int, List<GridObject>>();
    public override void _Ready()
    {
        Init();
    }

    public void Init()
    {
        grid = new Grid(100,100);
    }

    public void RegisterGridObject(ref GridObject gridObject, Vector2I position)
    {
        if (!IsPositionValid(position))
        {
            position = Vector2I.Zero;
        }

        gridObject.Init(this, position);
    }

    public bool IsPositionValid(Vector2I position)
    {
        return true;
        
        return position.X >= 0 && position.X < grid.Columns &&
               position.Y >= 0 && position.Y < grid.Rows;
    }

    public void TryMove(ref GridObject gridObject, Vector2I offset)
    {
        var targetPosition = gridObject.position + offset;
        if (!IsPositionValid(targetPosition))
            return;
        
        // check for obstacles 
        
        gridObject.position = targetPosition;
    }
}