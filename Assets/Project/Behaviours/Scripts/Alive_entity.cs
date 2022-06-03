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
    public bool dead = false;
    public bool isfemale;
    public float MatingUrge;
    public float matingTHreshold = 0.45f;
    public bool pregnant;
    public bool partner;
    public Alive_entity Mate;
    public int[] geneValues;
    public Actions CurrentAction;


    public  void Die(Death cause)
    {
        if (partner)
        {
            if (Mate != null)
            {
                Mate.partner = false;
                Mate.CurrentAction = Actions.Exploring;
            }
        }

        if (cause == Death.decompose)
        {
           // Debug.Log("destory");
            EntityTracker.Instance.RemoveEntity(Specie, this);
            Destroy(gameObject);
            
        }

        dead = true;
        Reason = cause;
        EntityTracker.Instance.UpdateDeath(cause);
    
    
    }

   public void eaten()
    {

        EntityTracker.Instance.RemoveEntity(Specie, this);
        Destroy(gameObject);
    }



    public bool requestMating(float desirebil)
    {
        if (desirebil * MatingUrge > matingTHreshold && partner == false && dead != true)
        {
            partner = true;
            CurrentAction = Actions.GoingToMate;
           // Debug.Log("FemaleMate");
            return true;
        }  
        else
            return false;
    }

}


