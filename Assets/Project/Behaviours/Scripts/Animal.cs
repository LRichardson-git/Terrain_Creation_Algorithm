using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Alive_entity
{


    //General
    float LastActionTime;
    float TimeBetweenACtions = 4;
    public int range;

    //speeds
    float drinkSpeed = 7;
    float eatSpeed = 11;
    public int movespeed = 25;

    //Pathfinding
    int previousPos;
    int CurrentPos;
    Vector3 targetposition;
    private List<Vector3> PathList;
    int pathindex;
    public int Tarx;
    public int tary;

    //Death
    float MaxHunger = 200;
    float MaxThirst = 190;
    float TimetoDecompose = 90;

    //Other Entities
    List<Alive_entity> Predators;
    Alive_entity eating;
    Vegtable Fruit_target;

    //STATUS
    public float Hunger;
    public float Thirst;
    public float decompose;

    //Water
    public Coords WaterAdj;
    Coords waterDrink;

    //Enum
    public Actions CurrentAction;


    void Start()
    {
        Coordinate = new Coords(x, y);
        previousPos = x + y;
        CurrentPos = x + y;

    }

    private void init()
    {
        Vector3 pos = EntityTracker.Instance.Coordtoworld(Coordinate);
        transform.position = pos;
        //EntityTracker.Instance.SpeciesMap[(Species.Rabbit)].Add( this);
        
    }



    void Update()
    {

        
        //Check if dead
        if (dead == true)
        {
            decompose += Time.deltaTime * 1 / TimetoDecompose;

            if (decompose >= 1)
            {
                Die(Death.decompose);
            }
        }

        else {
            Thirst += Time.deltaTime * 1 / MaxThirst;
            Hunger += Time.deltaTime * 1 / MaxHunger;

            if (Input.GetKeyDown("down"))
        {
                init();
 
  
            }

            
           

            float TimeSinceLastAction = Time.time - LastActionTime;

        if (TimeSinceLastAction > TimeBetweenACtions)
        {

                //default behaviour is called exlporing
                //occurs when no predators, not hungry nor firsty

                //First thing Check thirst if above 0.5 get water
                //unless there is a predator
                //if above 0.8 get water no matter what

                //Same with food

                checkforPredators();

                if (Thirst > 0.5 && CurrentAction != Actions.GoingToWater && CurrentAction != Actions.Drinking)
                {

                    WaterAdj = EntityTracker.Instance.FindWater(x, y, 10);
                    if (WaterAdj.x != -1)
                    {
                        CurrentAction = Actions.GoingToWater;
                        Tarx = WaterAdj.x;
                        tary = WaterAdj.y;
                        PathList = EntityTracker.Instance.FindPath(Tarx, tary, x, y);


                    }
                   
                    //action

                    
                }

            LastActionTime = Time.time;
                //choost action

                

                
                

        }
            if (CurrentAction == Actions.GoingToWater && x == WaterAdj.x && y == WaterAdj.y)
            {
                CurrentAction = Actions.Drinking;

                //drink



            }


            if (Hunger > 0.4 && CurrentAction != Actions.Goingtofood && CurrentAction != Actions.Eating)
            {

                Fruit_target = vegation_manger.Instance.FindVegatble(x, y, 10, Specie);
                Debug.Log(Fruit_target.xy.x);
                if (Fruit_target.xy.x != -1)
                {
                    CurrentAction = Actions.Goingtofood;
                    Tarx = Fruit_target.xy.x;
                    tary = Fruit_target.xy.y;
                    PathList = EntityTracker.Instance.FindPath(Tarx, tary, x, y);


                }

                //action


            }

            LastActionTime = Time.time;
            //choost action






        
        if (CurrentAction == Actions.Goingtofood && x == Fruit_target.xy.x && y == Fruit_target.xy.y && Fruit_target != null)
        {
            CurrentAction = Actions.Eating;

            //drink



        }




















        UpdateStatus();
    
            if (Hunger >= 1)
            {
                Die(Death.Hunger);
            }
            else if (Thirst >= 1)
            {
                Die(Death.Thirst);
            }



            FindPath();
        
        }
    }


    void checkforPredators()
    {

        
            // AnimialNum = 4;

            Predators = new List<Alive_entity>(EntityTracker.Instance.CheckPredators(x, y, range, Specie));
            if (Predators.Count > 0)
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

    void UpdateStatus() { 
    
    if (CurrentAction == Actions.Drinking)
        {
            if (Thirst > 0)
            {
                Thirst -= Time.deltaTime * 1 / drinkSpeed;
                Thirst = Mathf.Clamp01(Thirst);

            }
        } 

    else  if (CurrentAction == Actions.Eating && Fruit_target != null)
        {

            if (Hunger > 0)
            {
                Hunger -= Time.deltaTime * 1 / drinkSpeed;
                Hunger = Mathf.Clamp01(Thirst);
                Fruit_target.eaten();
                CurrentAction = Actions.Exploring;
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

    void FindPath()
    {

        if (PathList != null)
        {
            //Debug.Log("test");
            targetposition = PathList[pathindex];


            // Vector3 Temp = EntityTracker.Instance.GetGrid(newPos);
            Vector2 Temp = EntityTracker.Instance.GetGrid(transform.position);

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
                    transform.position = targetposition;
                    Move();
                    //happens when reachin new tile...

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
