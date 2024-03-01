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

        // if (controller == null)
        //     controller = GameObject.Instantiate(RoomPrefab, worldPosition, Quaternion.Euler(0, rotation, 0)).gameObject.AddComponent<RoomController>();
        
        // controller.transform.localRotation = Quaternion.Euler(0, rotation, 0);
        // controller.PostGenerationHandler();
    }
    
    public void DestroyRoom()
    {
        if (controller != null)
            controller.QueueFree();
    }
}