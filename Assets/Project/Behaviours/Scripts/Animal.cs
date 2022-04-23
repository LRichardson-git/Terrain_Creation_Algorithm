using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Alive_entity
{

   

    public int movespeed = 25;

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
    //Constant once initiatied values

    float MaxHunger = 200;
    float MaxThirst = 200;
    float TimetoDecompose = 90;
    Actions CurrentAction;

    //speed

    float drinkSpeed = 7;
    float eatSpeed = 11;
    Alive_entity eating;
    //==================
    //STATUS

    public float Hunger;
    public float Thirst;
    public float decompose;
    





    // Start is called before the first frame update
    void Start()
    {

        Coordinate = new Coords( x, y);

        previousPos = x + y;
        CurrentPos = x + y;
        //Debug.Log("test");
        Animal = AnimialNum;
        //transform.position = new Vector3(Pos.x, 15f, Pos.z);
        Specie = Species.Rabbit;
        dead = false;
        food = 10;
    }

    private void init()
    {
        //EntityTracker.Instance.SpeciesMap.Add(Species.Rabbit,)
        EntityTracker.Instance.SpeciesMap[(Species.Rabbit)].Add(Coordinate);

        Vector3 pos = EntityTracker.Instance.Coordtoworld(Coordinate);
        transform.position = pos;
       
        //Coords lol12 = new Coords(x, y);
        //EntityTracker.Instance.SpeciesMap[(Species.Rabbit)].Add(lol12);
        //Debug.LogError("sdas");
    }




    // Update is called once per frame
    void Update()
    {

        //update huner and thirst

        if (dead == true)
        {
            decompose += Time.deltaTime * 1 / TimetoDecompose;

            if (decompose >= 1)
            {
                Die(Death.decompose);
            }
        }
        else { 



        Coordinate.x = x;
        Coordinate.y = y;
        
        Thirst += Time.deltaTime * 1 / MaxThirst;
        Hunger += Time.deltaTime * 1 / MaxHunger;



        if (Hunger >= 1)
        {
            Die(Death.Hunger);
        } else if (Thirst >= 1) {
            Die(Death.Thirst);
        }

        if (Input.GetKeyDown("up"))
        {
            // if ( once < 1) { 
            //PathList = EntityTracker.Instance.FindPath(Tarx, tary, x, y) ;
            Coords Temp = EntityTracker.Instance.FindWater(x, y, 20);
            Debug.Log(Temp.x);
            Debug.Log(Temp.y);
                tary = Temp.y;
                Tarx = Temp.x;
                PathList = EntityTracker.Instance.FindPath(Tarx, tary, x, y);
                //Debug.Log(PathList[0]);
                //  }
            }
        if (Input.GetKeyDown("down"))
        {
            //Vector3 Pos = EntityTracker.Instance.Coordtoworld(Coordinate);

            //// EntityTracker.Instance.UpdateMap(0, 0, x, y, GetComponent<Alive_entity>());
            // transform.position = new Vector3(Pos.x, 15f, Pos.z);
            init();
        }
        if (Input.GetKeyDown("left"))
        {
           List<Coords> lol1 = EntityTracker.Instance.SpeciesMap[Species.Rabbit];
                Debug.Log("lest");
                Debug.Log(lol1.Count);
                for (int x = 0; lol1.Count > x ; x++)
            {
                    Debug.Log("new");
                    Debug.Log(lol1[x].x);
                Debug.Log(lol1[x].y);
            }
        }


        //CheckForPredators





        float TimeSinceLastAction = Time.time - LastActionTime;

        if (TimeSinceLastAction > TimeBetweenACtions)
        {

                if (Thirst > 0.8)
                {
                    Coords Temp = EntityTracker.Instance.FindWater(x, y, 20);
                    PathList = EntityTracker.Instance.FindPath(Temp.x, Temp.y, x, y);

                }

            LastActionTime = Time.time;
            //choost action
            if (AnimialNum < 2)
            {
               // AnimialNum = 4;

                Predators = new List<Alive_entity>(EntityTracker.Instance.CheckPredators(x, y, range, Animal));
                if (Predators.Count != 0)
                {
                        //run away
                        
                        Debug.Log(Predators[0].x);
                    Debug.Log(Predators[0].y);
                }
                else
                {
                   // Debug.Log("no pred");
                }

            }

        }



        //put into function

        if (PathList != null)
        {
                //Debug.Log("test");
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
    }


    void UpdateStatus() { 
    
    if (CurrentAction == Actions.Drinking)
        {
            if (Thirst > 0)
            {
                Thirst -= Time.deltaTime * 1 / drinkSpeed;
                Thirst = Mathf.Clamp01(Thirst);

            }
        } 

    else  if (CurrentAction == Actions.Eating)
        {

            //
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
