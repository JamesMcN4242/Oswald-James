using UnityEngine;

[CreateAssetMenu(fileName = "EquipableData", menuName = "ScriptableData/EquipableData", order = 0)]
public class EquipableData : ScriptableObject
{
    public string m_className = null;
    public int m_health = 0;
    public float m_accuracy = 0.0f;
    public float m_attackDamage = 0.0f;
    public float m_defence = 0.0f;
    public float m_speed = 0.0f;
}
