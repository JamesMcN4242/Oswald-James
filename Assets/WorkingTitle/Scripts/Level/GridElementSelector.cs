using UnityEngine;
using Unity.Mathematics;

public struct SelectedGridElement
{
    public bool CursorOverElement => m_arrayPos.x >= 0;

    public bool m_selected;
    public int2 m_arrayPos;
}

public static class GridElementSelector
{
    private static readonly int k_levelLayer = LayerMask.NameToLayer("Map");

    public static SelectedGridElement UpdateGridSelection(Tile[,] gridElements, float3 mousePosition)
    {
        SelectedGridElement selectedGridElement = new SelectedGridElement { m_selected = false, m_arrayPos = new int2(int.MinValue)};
        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            for(int i = 0, firstDimension = gridElements.GetLength(0); i < firstDimension; i++)
            {
                for(int j = 0, secondDimension = gridElements.GetLength(1); j < secondDimension; j++)
                {
                    if(hit.collider.gameObject == gridElements[i,j])
                    {
                        selectedGridElement.m_arrayPos = new int2(i, j);
                        return selectedGridElement;
                    }
                }
            }
        }

        return selectedGridElement;
    }
}
