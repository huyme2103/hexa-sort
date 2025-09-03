using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{
   public List<Hexagon> Hexagons {get; private set;}
   
   public void Add(Hexagon hexagon)
   {
      if (Hexagons == null)
         Hexagons = new List<Hexagon>();
      Hexagons.Add(hexagon);
   }
   public void Plane()
   {
      foreach (Hexagon hexagon in Hexagons)
      {
         hexagon.DisableCollider();
      }
   }
}
