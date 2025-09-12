using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{
   public List<Hexagon> Hexagons {get; private set;}
   
   public void MyAdd(Hexagon hexagon)
   {
      if (Hexagons == null)
         Hexagons = new List<Hexagon>();

      Hexagons.Add(hexagon);
      hexagon.SetParent(transform);
   }

    public Color GetTopHexagonColor()
    {
        return Hexagons[^1].Color; // lay phan tu mau cuoi cung  
    }

    public void Plane()
   {
      foreach (Hexagon hexagon in Hexagons)
      {
         hexagon.DisableCollider();
      }
   }

    public bool MyContains(Hexagon hexagon) => Hexagons.Contains(hexagon); // kiểm tra hexagon của phuong thuc, neu co trong list tra ve true

    public void MyRemove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon); // Remove trong list

        if (Hexagons.Count <= 0)
            //DestroyImmediate(gameObject);
            Destroy(gameObject);
    }
}
