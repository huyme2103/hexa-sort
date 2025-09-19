using NaughtyAttributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class GirdGenerator : MonoBehaviour
{
  [Header("Elements")]
  [SerializeField] private Grid gird;
  [SerializeField] private GameObject hexagon; // grid Hex
  
  [Header("setting")]
  [OnValueChanged("GenerateGird")]
  [SerializeField] private int gridSize;
  [SerializeField] private float hexSize;

  //private void GenerateGird()
  //{
  //  transform.Clear();

  //  for (int x = -gridSize; x <= gridSize; x++)
  //  {
  //    for (int y = -gridSize; y <= gridSize; y++)
  //    {
  //       Vector3 spawnPos = gird.CellToWorld(new Vector3Int(x,y,0));
  //       if (spawnPos.magnitude > gird.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * gridSize) 
  //         continue;

  //       GameObject gridHexInstance = (GameObject)PrefabUtility.InstantiatePrefab(hexagon);
  //       gridHexInstance.transform.position = spawnPos;
  //       gridHexInstance.transform.rotation = Quaternion.identity;
  //       gridHexInstance.transform.SetParent(transform);

  //       //Instantiate(hexagon, spawnPos, Quaternion.identity,transform);
  //    }
  //  }
  //}

    private void GenerateGird()
    {
        float height = hexSize * 2;
        float with = hexSize * Mathf.Sqrt(3);
        transform.Clear();

        for (int q = -gridSize; q <= gridSize; q++)
        {
            for (int r = -gridSize; r <= gridSize; r++)
            {
                for (int s = -gridSize; s <= gridSize; s++)
                {
                    if (q + r + s != 0)
                        continue;
                    Vector3 rDirection = Vector3.back;
                    Vector3 qDirection = Quaternion.Euler(0, 60, 0) * Vector3.right;
                    Vector3 sDirection = Quaternion.Euler(0, 120, 0) * Vector3.right;
                    Vector3 spawnPos = rDirection * r * height * 1.5f +
                                        qDirection * q *  with +
                                        sDirection * s *with;

                    GameObject gridHexInstance = (GameObject)PrefabUtility.InstantiatePrefab(hexagon);
                    gridHexInstance.transform.position = spawnPos;
                    gridHexInstance.transform.rotation = Quaternion.identity;
                    gridHexInstance.transform.SetParent(transform);
                }
            }
        }
    }
}
#endif