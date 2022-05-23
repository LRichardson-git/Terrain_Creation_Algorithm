using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegtable : MonoBehaviour
{

    public Vegetion type;
    public Coords xy;

    public Vegtable(Coords xy, Vegetion type)
    {
        this.xy = xy;
        this.type = type;
  

    }

    public void Init(Coords Pos, Vegetion VegType)
    {
        
    }

    public void eaten ()
    {
        vegation_manger.Instance.removeVeg(type, this, xy);
        Debug.Log(xy.x);
        Debug.Log(xy.y);
        Debug.Log("EATEN");
        Destroy(gameObject);
        

    }
  
}
