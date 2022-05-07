using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegtable : MonoBehaviour
{

    public Vegetion type;
    public Coords xy;


    public void eaten ()
    {
        vegation_manger.Instance.removeVeg(type, this, xy);
        Destroy(gameObject);


    }
  
}
