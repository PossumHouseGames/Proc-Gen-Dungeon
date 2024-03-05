using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class RoomsList : Resource
{
    [Export] private RoomData[] _rooms;
    [Export] private RoomShapeController[] _roomShapes;
    
    public RoomData[] Rooms => _rooms;
    public RoomShapeController[] RoomShapes => _roomShapes;
    
    public RoomData GetRandom(System.Random rng)
    {
        return _rooms[rng.Next(0, _rooms.Length)];
    }
    
    public RoomShapeController GetRandomShape(System.Random rng, RoomInstance.RoomShape targetShape)
    {
        var roomGroup = _roomShapes.Where(rd => rd.data.shape == targetShape).ToList();

        return roomGroup.Count == 0 ? null :  roomGroup[rng.Next(0, roomGroup.Count)];
    }
}