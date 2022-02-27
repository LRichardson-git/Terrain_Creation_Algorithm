using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

   

    public Material Material;
    public int movespeed = 3;
    public int HP = 10;
    public Coords Coordinate;
    public int x;
    public int y;






                // Start is called before the first frame update
                void Start()
    {

        Coordinate.x = x;
        Coordinate.y = y;

        Vector2 Pos = Coordinate.Coordtoworld(Coordinate);


        transform.position = new Vector3(Pos.x, 10f, Pos.y);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
