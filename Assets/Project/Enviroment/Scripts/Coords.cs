using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coords
{
    // Start is called before the first frame update

    public int x;
    public int y;



    public int m_gCost; //distance from start
    public int m_hCost; //distance if not account for  blockers
    public int m_fCost; //fcost distance to node


    public Coords LastCoord;
    public bool IsWalkable;
    public bool IsWater;
    public bool Vegation;
    // public Alive_entity Creature;
    public bool Creature ;
    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
        IsWalkable = true;
        LastCoord = null;
        Creature = false;
        
    }
    



    public void CalculateFCost()
    {
        m_fCost = m_gCost + m_hCost;
    }


    
    //get world coord

    //return world coord


}