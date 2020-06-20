using Unity.Mathematics;
using UnityEngine;

using static UnityEngine.Debug;

public class Entity
{
    private enum MovementStatus
    {
        IDLE,
        MOVING
    }

    private const float k_movementSpeed = 4.0f; //Tiles per second

    private static readonly Color k_highlightColour = Color.yellow;
    private readonly Color k_startingColour;
    private readonly Material k_characterMaterial;
    private readonly Transform m_transform;

    private CharacterData m_characterData;
    private EquipableData m_equipableData;
    private float m_timeToTravel = 0.0f;

    private MovementStatus m_movementStatus = MovementStatus.IDLE;
  
    public bool IsMoving => m_movementStatus == MovementStatus.MOVING;
    public int SpeedStat => (int)m_characterData.m_speed;
    public int2 CurrentTile { get; private set; }

    public Entity(GameObject go, CharacterData charData, EquipableData equipableData, int2 startingTile)
    {
        m_transform = go.transform;
        k_characterMaterial = go.GetComponent<Renderer>().material;
        k_startingColour = k_characterMaterial.color;

        m_characterData = charData;
        m_equipableData = equipableData;
        CurrentTile = startingTile;
    }

    public bool CanMoveToNewPosition(int2 newPosition)
    {
        return m_characterData.m_speed >= GetMovementDistance(newPosition);
    }

    public void SetNewPosition(int2 newPosition)
    {
        Assert(m_movementStatus != MovementStatus.MOVING, "Trying to set new position for character when already moving");
        Assert(CanMoveToNewPosition(newPosition), 
            $"Can't move to new position {newPosition} from current tile {CurrentTile}. " +
            $"Speed ({m_characterData.m_speed.ToString("N0")}) is too slow");
        
        m_timeToTravel = GetMovementDistance(newPosition) / k_movementSpeed;
        m_movementStatus = MovementStatus.MOVING;
        CurrentTile = newPosition;
    }

    public void UpdateEntityMovement(float deltaTime)
    {
        switch (m_movementStatus)
        {
            case MovementStatus.IDLE:
                break;

            case MovementStatus.MOVING:
                UpdateMovingStatus(deltaTime);
                break;
        }
    }
    
    public void SetHighlightStatus(bool highlight)
    {
        k_characterMaterial.color = highlight ? k_highlightColour : k_startingColour;
    }
    
    private int GetMovementDistance(int2 newTile)
    {
        int2 subtractedPositions = math.abs(CurrentTile - newTile);
        return subtractedPositions.x + subtractedPositions.y;
    }

    private void UpdateMovingStatus(float deltaTime)
    {
        float travelingTime = math.min(m_timeToTravel, deltaTime);      
        m_timeToTravel -= travelingTime;
        if (m_timeToTravel <= 0.0f)
        {
            m_movementStatus = MovementStatus.IDLE;
            m_transform.position = new Vector3(CurrentTile.x, m_transform.position.y, CurrentTile.y);
        }
        else
        {
            float3 direction = (new Vector3(CurrentTile.x, 0f, CurrentTile.y) - m_transform.position).normalized;
            float3 movement = direction * k_movementSpeed * travelingTime;
            float3 newPosition = movement + (float3)m_transform.position;
            newPosition.y = m_transform.position.y;
            m_transform.position = newPosition;
        }
    }
}
