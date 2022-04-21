using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Species
{
    Rabbit = 0,
    fox = 1 

};

public enum Death
{
    Hunger,
    Thirst,
    Age,
    Eaten,
    Killed,
    decompose
}

public class Alive_entity : MonoBehaviour
{
    // Start is called before the first frame update

    public Coords Coordinate;

    protected bool dead;
    public int Animal;
    public Material material;
    public int Index;
    public int HP = 10;
    public int x;
    public int y;
    public Death Reason;
    public int food;
    public Species Specie;
    public  void Die(Death cause)
    {

        if (cause == Death.decompose || food <= 0)
        {
            Debug.Log("destory");
            Debug.Log(cause);
            EntityTracker.Instance.RemoveEntity(Specie, Coordinate);
            Destroy(gameObject);
            
        }

        dead = true;
        //decompe
        
        Reason = cause;
         

        
    }

    }
