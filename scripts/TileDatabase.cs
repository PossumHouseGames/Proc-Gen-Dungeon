using Godot;
using System;
using System.Linq;

[Tool]
[GlobalClass]
public partial class TileDatabase : Resource
{
    [Export] public TileColor[] Tiles;

    public bool TryGetTile(TileTypes type, out PackedScene targetTile)
    {
        if (Tiles.Length == 0)
        {
            GD.Print($"TileCount {Tiles.Length}");
            targetTile = default;
            return false;
        }
        
        targetTile = Tiles.FirstOrDefault(t => t.TileType == type, Tiles[0]).TileScene;
        return targetTile != null;
    }
}