using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private void StackPlacedCallback(GridCell gridCell) // truyen targetCell từ onStackPlaced?.Invoke(targetCell); 
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer; // chi chon layer girdCell, 1 la chọn , 0 la bo qua

        List<GridCell> neighborGridCells = new List<GridCell>();//

        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position,2, gridCellMask ); // trong bán kính 2, tra ve mang collider thuộc cùng layer với gridCell

        foreach (Collider girdCellCollider in neighborGridCellColliders)//gridCellCollider = từng collider mà OverlapSphere tìm thấy
        {
            GridCell neighborGridCell = girdCellCollider.GetComponent<GridCell>();

            if (! neighborGridCell.IsOccupied)
                continue;// bo qua o trong
            if (neighborGridCell == gridCell)
                continue;

            neighborGridCells.Add(neighborGridCell);//
        }

        if(neighborGridCells.Count <= 0)
        {
            Debug.Log("khong co neighbors o gan cell nay");
            return;
        }
        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor(); //lay danh sach Hexagon tu HexStack ở GridCell.cs observer

        //tim cung mau
        List<GridCell> similarNeighborGridCells = new List<GridCell>();

        foreach (GridCell neighborGridCell in neighborGridCells)
        {
            Color neighborGridCellTopHexagonColor = neighborGridCell.Stack.GetTopHexagonColor();

            if (gridCellTopHexagonColor == neighborGridCellTopHexagonColor)
                similarNeighborGridCells.Add(neighborGridCell);
        }

        if (similarNeighborGridCells.Count <= 0)
        {
            Debug.Log("khong co similar neighbors o gan cell nay");
            return;
        }

        //Gom hexagon từ neighbor cùng màu
        List<Hexagon> hexagonsToAdd = new List<Hexagon>();

        foreach (GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack neighborCellHexStack = neighborCell.Stack;
            for (int i = neighborCellHexStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neighborCellHexStack.Hexagons[i];

                if (hexagon.Color != gridCellTopHexagonColor) 
                    break;

                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);// tách hexagon ra khỏi stack gốc
            }
        }
        //Debug.Log($"we need to add {hexagonsToAdd.Count}");

        foreach(GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack stack = neighborCell.Stack;
            foreach(Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.MyContains(hexagon)) // if true
                    stack.MyRemove(hexagon);
            }

        }

        float initialY = gridCell.Stack.Hexagons.Count * 0.2f;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];

            float targetY = initialY + i * 0.2f;
            Vector3 targetlocalPosition = Vector3.up * targetY;

            gridCell.Stack.MyAdd(hexagon);
            hexagon.transform.localPosition = targetlocalPosition;
        }
    }

   
}
