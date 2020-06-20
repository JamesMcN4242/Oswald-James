using UnityEngine;
using Unity.Mathematics;

public class InputSystem
{
    public float3 CurrentMousePosition { get; private set; }
    public bool ClickComplete { get; private set; }
    public bool InputToProcess { get; private set; }

    public InputSystem()
    {
        CurrentMousePosition = new float3();
    }

    public void UpdateInput()
    {
        float3 newCursorPos = Input.mousePosition;
        ClickComplete = Input.GetMouseButtonUp(0);
        InputToProcess = !newCursorPos.Equals(CurrentMousePosition) || ClickComplete;
        CurrentMousePosition = newCursorPos;
    }
}
