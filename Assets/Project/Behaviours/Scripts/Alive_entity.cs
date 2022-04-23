using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Alive_entity : MonoBehaviour
{


    //Locations
    public Coords Coordinate;
    public int x;
    public int y;

    //DataTypes
    public Death Reason;
    public Material material;
    public Species Specie;

    //Status
    public int food = 10;
    protected bool dead = false;
    public int HP = 10;

   
    public  void Die(Death cause)
    {
        if (cause == Death.decompose || food <= 0)
        {
            Debug.Log("destory");
            Debug.Log(cause);
            EntityTracker.Instance.RemoveEntity(Specie, this);
            Destroy(gameObject);
            
        }

        dead = true;
        Reason = cause;  }

    }
