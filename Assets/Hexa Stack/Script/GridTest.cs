using UnityEngine;
using System.Collections;
using NaughtyAttributes;
public class GridTest : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Grid grid;
    [Header("Settings")]
    [OnValueChanged("UpdateGridPos")]
    [SerializeField] private Vector3Int gridPos;
    private void UpdateGridPos() => transform.position = grid.CellToWorld(gridPos);
}
