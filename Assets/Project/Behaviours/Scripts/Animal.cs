using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Alive_entity
{

   

    public int movespeed = 3;

    public Coords Coordinate;
    float TimeBetweenACtions = 4;
    int previousPos;
    int CurrentPos;

    Vector3 targetposition;

    public int range;
    public int Tarx;
    public int tary;
    private List<Vector3> PathList;
    int pathindex;
    List<Alive_entity> Predators;
    int AnimialNum;

    float LastActionTime;



                // Start is called before the first frame update
                void Start()
    {

        Coordinate = new Coords( x, y);

        previousPos = x + y;
        CurrentPos = x + y;
        //Debug.Log("test");
        Animal = AnimialNum;
        //transform.position = new Vector3(Pos.x, 15f, Pos.z);

    }

  
  



    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown("up"))
        {
           // if ( once < 1) { 
            PathList = EntityTracker.Instance.FindPath(Tarx, tary, x, y) ;
            //Debug.Log(PathList[0]);
              //  }
        }
        if (Input.GetKeyDown("down"))
        {
            Vector3 Pos = EntityTracker.Instance.Coordtoworld(Coordinate);

            EntityTracker.Instance.UpdateMap(0, 0, x, y, GetComponent<Alive_entity>());
            transform.position = new Vector3(Pos.x, 15f, Pos.z);
           
        }


        
        //CheckForPredators
       




        float TimeSinceLastAction = Time.time - LastActionTime;

        if (TimeSinceLastAction > TimeBetweenACtions)
        {

            LastActionTime = Time.time;
            //choost action
            if (AnimialNum < 2)
            {
               // AnimialNum = 4;

                Predators = new List<Alive_entity>(EntityTracker.Instance.CheckPredators(x, y, range, Animal));
                if (Predators != null)
                {
                    //run away
                    Debug.Log(Predators[0].x);
                }


            }

        }



        //put into function

        if (PathList != null)
        {
            targetposition = PathList[pathindex];


            // Vector3 Temp = EntityTracker.Instance.GetGrid(newPos);
            Vector3 Temp = EntityTracker.Instance.GetGrid(transform.position);
            
            x = (int)Temp.x;
            y = (int)Temp.y;
            CurrentPos = x + y;

            if (previousPos != CurrentPos)
            {

                //update movementmap
            }


            //transform.position = PathList[0];
            if (Vector3.Distance(transform.position, targetposition) > 0.2f)
            {
                Vector3 moveD = (targetposition - transform.position).normalized;
                transform.position = transform.position + moveD * movespeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, targetposition) < 0.2f)
                {
                    
                        Move();
                  

                }
            }
            else
            {
                pathindex++;

                if (pathindex >= PathList.Count)
                    PathList = null;

            }
        }
    }


    public bool Move()
    {
        //Will loop thorough the path list by removeing the pathnode the gameobject is on when you move
        //Debug.Log("test");
       
           // movepoints--;
            pathindex = 0;
            PathList = new List<Vector3>();
            PathList = EntityTracker.Instance.FindPath(Tarx, tary, x,y);
      //  Debug.Log(PathList[pathindex]);

            if (PathList != null && PathList.Count > 1)
            {
                PathList.RemoveAt(0);
            }

            //Returns bool to check if already at position - Thomas
            if (PathList != null && PathList.Count <= 1)
            {
                return true;
            }

            //If player is stuck return true
            if (PathList == null)
            {
                return true;
            }
            return false;
        




    }
}
