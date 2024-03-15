using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
[GlobalClass, Tool]
public partial class RoomsList : Resource
{
    [Export] private RoomData[] _rooms;
    [Export] public PackedScene[] _roomShapes;
    
    public RoomData[] Rooms => _rooms;
    public PackedScene[] RoomShapes => _roomShapes;
    
    public RoomData GetRandom(System.Random rng)
    {
        return _rooms[rng.Next(0, _rooms.Length)];
    }
    
    public RoomShapeController GetRandomShape(System.Random rng, RoomInstance.RoomShape targetShape)
    {
        var roomGroup = _roomShapes.Where(rd => ((RoomShapeController)rd.Instantiate()).data.shape == targetShape).ToList();

        return roomGroup.Count == 0 ? null :  (RoomShapeController)roomGroup[rng.Next(0, roomGroup.Count)].Instantiate();
    }
}