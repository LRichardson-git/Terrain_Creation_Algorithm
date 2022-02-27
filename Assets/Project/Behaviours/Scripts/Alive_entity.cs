using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Species
{
    Rabbit = 0,
    fox = 1 

};
public class Alive_entity : MonoBehaviour
{
    // Start is called before the first frame update



    protected bool dead;
    public Species Animal;
    public Material material;
    public Coords coord;
    public int Index;

    public virtual void Initate(Coords coord)
    {
        this.coord = coord;
        transform.position = this.coord.Coordtoworld(this.coord);


        //change material
       
    }





}
