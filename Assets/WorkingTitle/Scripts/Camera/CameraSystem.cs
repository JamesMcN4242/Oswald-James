using UnityEngine;
using Unity.Mathematics;

public class CameraSystem
{
    private CameraSettings m_cameraSettings;
    private Transform m_cameraTransform;

    public CameraSystem()
    {
        m_cameraSettings = Resources.Load<CameraSettings>("Settings/CameraSettings");
        m_cameraTransform = Camera.main.transform;
        m_cameraTransform.position = m_cameraSettings.m_startPosition;
    }

    public void UpdateCamera(float3 postionToLookAt, float deltaTime)
    {
        postionToLookAt += m_cameraSettings.m_entityOffset;
        float distance = math.distance(m_cameraTransform.position, postionToLookAt);
        deltaTime = math.min(deltaTime, distance / m_cameraSettings.m_cameraSpeed);

        if (deltaTime > 0.0f)
        {
            float3 distanceToCover = (float3)m_cameraTransform.position - postionToLookAt;
            float3 direction = math.normalize(distanceToCover);

            float3 distanceToTravel = direction * deltaTime * m_cameraSettings.m_cameraSpeed;
            m_cameraTransform.position -= (Vector3)distanceToTravel;
        }
    }
}
