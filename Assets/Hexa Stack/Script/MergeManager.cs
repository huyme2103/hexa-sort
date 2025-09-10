using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    private void Awake()
    {
        StackController.onStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }
    private void StackPlacedCallback(GridCell gridCell)
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer;

        List<GridCell> neighborGridCells = new List<GridCell>();//

        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position,2, gridCellMask );

        foreach(Collider girdCellCollider in neighborGridCellColliders)
        {
            GridCell neighborGridCell = girdCellCollider.GetComponent<GridCell>();

            if (neighborGridCell.IsOccupied)
                continue;
            if (neighborGridCell == gridCell)
                continue;

            neighborGridCells.Add(neighborGridCell);
        }

        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor(); //lay danh sach Hexagon tu HexStack ở GridCell.cs

        Debug.Log(gridCellTopHexagonColor);
    }
}
