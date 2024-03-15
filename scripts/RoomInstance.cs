using Godot;

public struct RoomInstance
{
    public RoomShapeController RoomPrefab;
    public RoomData data;
    public int rotation;
    public RoomController controller;
    
    public enum RoomShape {None, End, I, L, T, Cross}

    public void RenderRoom(Vector3 worldPosition)
    {
        if (controller == null)
            controller = new RoomController();
        controller.Position = worldPosition;
        controller.RotationDegrees = new Vector3(0, rotation, 0);
        controller.AddChild(RoomPrefab);
    }
    
    public void DestroyRoom()
    {
        controller?.QueueFree();
    }
}