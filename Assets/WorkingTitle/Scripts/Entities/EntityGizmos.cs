#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class EntityGizmos : MonoBehaviour
{
    public float ViewDistance { get; set; }

    private void OnDrawGizmosSelected()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, ViewDistance);
    }
}

#endif