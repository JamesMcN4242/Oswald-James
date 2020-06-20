using UnityEngine;
using Unity.Mathematics;

public class CameraSettings : ScriptableObject
{
    public float3 m_startPosition = new float3();
    public float3 m_entityOffset = new float3();
    public float m_cameraSpeed = 1.0f;
}
