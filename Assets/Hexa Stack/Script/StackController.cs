using UnityEngine;
using System;
public class StackController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    private HexStack currentStack;
    private Vector3 currentStackInitialPos;
    
    [Header("Data")]
    private GridCell targetCell;
    
    [Header("Actions")]
    public static Action<GridCell> onStackPlaced;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManagerControl();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ManagerControl()
    {
        if (Input.GetMouseButtonDown(0))
            ManagerMouseDown();
        else if (Input.GetMouseButton(0) && currentStack !=null)
            ManagerMouseDrag();
        else if (Input.GetMouseButtonUp(0) && currentStack !=null)
            ManagerMouseUp();
    } 

    private void ManagerMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit,500,hexagonLayerMask);

        if (hit.collider == null) return;
       
        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitialPos = currentStack.transform.position;
    }
  
    private void ManagerMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);
        
        if (hit.collider == null)
            DraggingAboveGround();
        else
            DraggingAboveGirdCell(hit);
    }
    private void ManagerMouseUp()
    {
        if (targetCell == null)
        {
            currentStack.transform.position = currentStackInitialPos;
            currentStack = null;
            return;
        }

        currentStack.transform.position = targetCell.transform.position.With(y: .2f);
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Plane();
        
        targetCell.AssignStack(currentStack);
        
        onStackPlaced?.Invoke(targetCell);
        
        targetCell = null;
        currentStack = null;
    }
    //
    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit,500,groundLayerMask);
        if (hit.collider == null) return;
     

        Vector3 currentStackTargetPos = hit.point.With(y: 2); // nang vi tri y len 2 (x,2,z)
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, 
            currentStackTargetPos, Time.deltaTime*30);

        targetCell = null;
    }
    private void DraggingAboveGirdCell(RaycastHit hit)
    {
        GridCell gridCell = hit.collider.GetComponent<GridCell>();

        if (gridCell.IsOccupied)
            DraggingAboveGround();
        else
            DraggingAboveNonOccupiedGridCell(gridCell);
    }

    private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
    {
        Vector3 currentStackTargetPos = gridCell.transform.position.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, 
            currentStackTargetPos, Time.deltaTime*30);
        
        targetCell =  gridCell;
    }

    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
