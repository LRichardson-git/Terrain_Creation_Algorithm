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
    public bool isfemale;
    public float MatingUrge;
    public float matingThresholf = 0.4f;
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
                Mate.partner = false;
            Mate.CurrentAction = Actions.Exploring;
        }

        if (cause == Death.decompose || food <= 0)
        {
           // Debug.Log("destory");
            Debug.Log(cause);
            EntityTracker.Instance.RemoveEntity(Specie, this);
            Destroy(gameObject);
            
        }

        dead = true;
        Reason = cause;  
        
    
    
    }

   public void eaten()
    {

        EntityTracker.Instance.RemoveEntity(Specie, this);
        Destroy(gameObject);
    }



    public bool requestMating(float desirebil)
    {
        if (desirebil * MatingUrge > matingThresholf && partner == false)
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


