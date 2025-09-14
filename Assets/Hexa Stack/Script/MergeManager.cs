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
        //Tìm neighbors 
        List<GridCell> neighborGridCells = GetNeighborGridCells(gridCell); // phuong thuc duoc trien khai ben duoi
       
        if(neighborGridCells.Count <= 0)
        {
            Debug.Log("khong co neighbors o gan cell nay");
            return;
        }

        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor(); //lay danh sach Hexagon tu HexStack ở GridCell.cs observer
        //tim cung mau
        List<GridCell> similarNeighborGridCells = GetSimilarNeighborGridCells(gridCellTopHexagonColor,neighborGridCells.ToArray());
        

        if (similarNeighborGridCells.Count <= 0)
        {
            Debug.Log("khong co similar neighbors o gan cell nay");
            return;
        }

        //Gom hexagon từ neighbor cùng màu
        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());


        // Remove hexagons from their stacks
        RemoveHexagonsFromStack(hexagonsToAdd, similarNeighborGridCells.ToArray());

        // Move hexagon
        MoveHexagons(gridCell, hexagonsToAdd );

        //
        CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
    }

    private List<GridCell> GetNeighborGridCells(GridCell gridCell)
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer; // chi chon layer có cùng layer girdCell, 1 la chọn , 0 la bo qua

        List<GridCell> neighborGridCells = new List<GridCell>();//

        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, gridCellMask); // trong bán kính 2, tra ve mang collider thuộc cùng layer với gridCell

        foreach (Collider girdCellCollider in neighborGridCellColliders)//gridCellCollider = từng collider mà OverlapSphere tìm thấy
        {
            GridCell neighborGridCell = girdCellCollider.GetComponent<GridCell>();

            if (!neighborGridCell.IsOccupied)
                continue;// bo qua o trong
            if (neighborGridCell == gridCell)
                continue;

            neighborGridCells.Add(neighborGridCell);//
        }

        return neighborGridCells;
    }

    private List<GridCell> GetSimilarNeighborGridCells(Color gridCellTopHexagonColor, GridCell[] neighborGridCells)
    {

        List<GridCell> similarNeighborGridCells = new List<GridCell>();

        foreach (GridCell neighborGridCell in neighborGridCells)
        {
            Color neighborGridCellTopHexagonColor = neighborGridCell.Stack.GetTopHexagonColor();

            if (gridCellTopHexagonColor == neighborGridCellTopHexagonColor)
                similarNeighborGridCells.Add(neighborGridCell);
        }

        return similarNeighborGridCells;
    }

    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor, GridCell[] similarNeighborGridCells)
    {
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
        return hexagonsToAdd;
    }

    private void RemoveHexagonsFromStack(List<Hexagon> hexagonsToAdd, GridCell[] similarNeighborGridCells)
    {
        foreach (GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack stack = neighborCell.Stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.ContainsHexagon(hexagon)) // if true
                    stack.RemoveHexagon(hexagon);
            }

        }
    }

    private void MoveHexagons(GridCell gridCell, List<Hexagon> hexagonsToAdd)
    {
        float initialY = gridCell.Stack.Hexagons.Count * 0.2f;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];

            float targetY = initialY + i * 0.2f;
            Vector3 targetlocalPosition = Vector3.up * targetY;

            gridCell.Stack.AddHexagon(hexagon);
            hexagon.transform.localPosition = targetlocalPosition;
        }
    }

    private void CheckForCompleteStack(GridCell gridCell, Color topColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            return;

        List<Hexagon> similarHexagons = new List<Hexagon>();

        for (int i = gridCell.Stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hexagon = gridCell.Stack.Hexagons[i];
            if (hexagon.Color != topColor)
                break;

            similarHexagons.Add(hexagon);
        }


        // at this point, have a list of similar hexagons
        if (similarHexagons.Count < 10)
            return;
        while(similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            DestroyImmediate(similarHexagons[0].gameObject);
            gridCell.Stack.RemoveHexagon(similarHexagons[0]);
            similarHexagons.RemoveAt(0);
        }
    } 
}
