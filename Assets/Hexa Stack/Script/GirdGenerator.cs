using NaughtyAttributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class GirdGenerator : MonoBehaviour
{
  [Header("Elements")]
  [SerializeField] private Grid gird;
  [SerializeField] private GameObject hexagon;
  
  [Header("setting")]
  [OnValueChanged("GenerateGird")]
  [SerializeField] private int gridSize;

  private void GenerateGird()
  {
    transform.Clear();

    for (int x = -gridSize; x <= gridSize; x++)
    {
      for (int y = -gridSize; y <= gridSize; y++)
      {
         Vector3 spawnPos = gird.CellToWorld(new Vector3Int(x,y,0));
         if (spawnPos.magnitude > gird.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * gridSize) 
           continue;

         GameObject gridHexInstance = (GameObject)PrefabUtility.InstantiatePrefab(hexagon);
         gridHexInstance.transform.position = spawnPos;
         gridHexInstance.transform.rotation = Quaternion.identity;
         gridHexInstance.transform.SetParent(transform);

         //Instantiate(hexagon, spawnPos, Quaternion.identity,transform);
      }
    }
  }
}
#endif