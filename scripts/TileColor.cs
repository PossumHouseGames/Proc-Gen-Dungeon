using Godot;

[Tool]
[GlobalClass]
public partial class TileColor : Resource
{    
    [Export] public string Name;
    [Export] public TileTypes TileType;
    [Export] public Color Key;
    [Export] public PackedScene TileScene;
}