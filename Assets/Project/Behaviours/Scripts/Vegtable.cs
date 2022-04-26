using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegtable : MonoBehaviour
{

    public Vegetion type;
    public int x;
    public int y;


    public void eaten ()
    {
        vegation_manger.Instance.removeVeg(type, this);
        Destroy(gameObject);


    }
  
}
