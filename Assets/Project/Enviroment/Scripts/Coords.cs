using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[System.Serializable]

public struct Coords
{
    // Start is called before the first frame update

    public int x;
    public int y;
    


    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public float GetDistantance(Coords a, Coords b)
    {
        return (float)System.Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));

    }

    public Vector3 Coordtoworld(Coords Transcoord)
    {

        Vector3 Trans;
        Trans.x = Transcoord.x * 10 + 5;
        Trans.y = Transcoord.y * 10 - 5;
        Trans.z = 10;
        return Trans;
    }




    //get world coord

    //return world coord


}