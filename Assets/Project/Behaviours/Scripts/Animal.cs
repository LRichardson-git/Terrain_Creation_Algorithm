using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Alive_entity
{


    //General
    float LastActionTime;
    float TimeBetweenACtions = 2;
    float tired;

    //speeds
    float drinkSpeed = 7;
    float eatSpeed = 11;
    public float movespeed = 20;


    
    //Genes
    public bool herbivore = true;
    Color Matcolour;
    public int range;
    int repoduction;
    int desirabilty;
    int gestation;
    bool isfemale;
    bool colorChange;

    int[] geneValues; //speed. colour. range. reproduction urge. desribilty. gestation period. is female. hunger. thirst

    //Pathfinding
    int previousPos;
    int CurrentPos;
    Vector3 targetposition;
    private List<Vector3> PathList;
    int pathindex;
    public int Tarx;
    public int tary;
    float CriticalThirstHunger = 0.9f;
    public int speed = 4;

    //Death
    float MaxHunger = 45;
    float MaxThirst = 190;
    float TimetoDecompose = 90;

    //Other Entities
    Alive_entity Predator;
    Alive_entity eating;
    Vegtable Vegtable_target;

    //STATUS
    public float Hunger;
    public float Thirst;
    public float decompose;
    public float horny;
    public Species Ani;
    //Water
    public Coords WaterAdj;
    Coords waterDrink;
    //Enum
    public Actions CurrentAction;


 
    public void init(int[] GeneValues, Coords Position, Coords HomeCoord)
    {

        
        //value setups
        Vector3 pos = EntityTracker.Instance.Coordtoworld(Position);
        transform.position = pos;
        geneValues = new int[10];
        geneValues = GeneValues;
        Coordinate = Position;
        x = Coordinate.x;
        y = Coordinate.y;


        //Gene setup

       // if (!herbivore)
            //TimeBetweenACtions = 3;


        if (herbivore)
        {
            speed += geneValues[0] / 31;


            movespeed = speed / 2;
        }
       // else
       // {
         //   speed += geneValues[0] / 31;


         //   movespeed = speed / 3;
     //   }
        movespeed = movespeed * 10;
        if (geneValues[1] > 225) // randomize colour if this gene passes test
        {
            for (int i = 0; i < 4; i++)
            {
                Matcolour[i] = Random.Range(0f, 1f);

            }
        }


        range = geneValues[2] / 25 + 3 ;
        repoduction = geneValues[3];
        desirabilty = geneValues[4];
        gestation = geneValues[5];
        if (geneValues[6] > 125)
            isfemale = true;
        MaxHunger += geneValues[7] / 2;
        MaxThirst += geneValues[7] / 3;

        //hungerir and thirstier faster if quicker
        MaxHunger -= ((movespeed / 2) + (movespeed / 5));
        MaxThirst -= ((movespeed / 2) + (movespeed / 3));
        Specie = Ani;


        if (Specie == Species.fox)
        {
            Hunger = 0.39f;
            range = 50;
        }
    }



    void Update()
    {

        
        //Check if dead
        if (dead == true)
        {
            Decompose();

        }

        else {

            //Update time

            Thirst += Time.deltaTime * 1 / MaxThirst;
            Hunger += Time.deltaTime * 1 / (MaxHunger - movespeed / 2); // faster == hungier quicker
            float TimeSinceLastAction = Time.time - LastActionTime;

            

            if (TimeSinceLastAction > TimeBetweenACtions)
        {
               
               // Debug.Log(TimeBetweenACtions);
                

                ChooseAction();

                DoActions();
                

                LastActionTime = Time.time;
                //choost action

        }


          //  Debug.Log("================");
          //  Debug.Log(pathindex);
          //  Debug.Log(Specie);
          //  Debug.Log(PathList);

        UpdateStatus();
    
        FindPath();
        
        }
    }

    private void ChooseAction()
    {
        //IF TIRED == 1 INSTANTLY REST


        if (checkforPredators() && Thirst < CriticalThirstHunger && Hunger < CriticalThirstHunger)
        {
            return;
            
        }

        if (CurrentAction == Actions.chasing && EntityTracker.Instance.GetDistantance(x, y, eating.x, eating.y) <= range)
            return;




        if (CurrentAction == Actions.Eating || CurrentAction == Actions.Drinking || CurrentAction == Actions.GoingToWater || CurrentAction == Actions.Goingtofood)
            return;


        //Drink water
        if (CurrentAction == Actions.GoingToWater && x == WaterAdj.x && y == WaterAdj.y)
        {
            CurrentAction = Actions.Drinking;
            return;
        }


        if (Thirst > 0.5 && CurrentAction != Actions.GoingToWater && CurrentAction != Actions.Drinking)
        {
            if (!findwater())
                CurrentAction = Actions.Exploring;
            return;
        }



        if (Hunger >= 0.4 && CurrentAction != Actions.Goingtofood && CurrentAction != Actions.Eating && CurrentAction != Actions.Drinking)
        {
            bool temp = false;

            if (herbivore)
                temp = foodHerbivore();
            else
                temp = foodMeat();

            if (temp)
                return;

            
            

        }


        CurrentAction = Actions.Exploring;


        if (horny > 0.8)
        {
            //do thing
        }

       


    }


    public void goToRandomDirection()
    {

        if (PathList == null)
        {

            int lol = Random.Range(1, 5);
            int rInt = Random.Range(1, speed + 1);
            int rYnt = Random.Range(1, speed + 1);

            //OPTIMIZE THIS LATER BY CREATING NEW PATHLIST CREATOR FOR MOVING SLIGHTLY

            switch (lol)
            {

                case 1:
                    SetTargetLocation(x + rInt, y + rYnt);
                    break;
                case 2:
                    SetTargetLocation(x - rInt, y + rYnt);
                    break;
                case 3:
                    SetTargetLocation(x + rInt, y - rYnt);
                    break;
                case 4:
                    SetTargetLocation(x - rInt, y - rYnt);
                    break;
                default:
                    SetTargetLocation(x + rInt, y + rYnt);
                    break;
            }
            

            
            /*
            if (Random.Range(1, 2) == 2 && x +rInt !> 199 && y + rYnt)
            {
                SetTargetLocation(x + rInt, y + rYnt);
            }
            else
                SetTargetLocation(x - rInt, y - rYnt);
            */


        }
        //MIGHT HAVE TO LIMIT IT
        
    }

    bool findwater()
    {
        WaterAdj = EntityTracker.Instance.FindWater(x, y, range);
        if (WaterAdj.x != -1)
        {
            CurrentAction = Actions.GoingToWater;
            return true;
        }
        return false;
    }

    private bool FindVegtable()
    {
        Vegtable_target = vegation_manger.Instance.FindVegatble(x, y, range, Specie);
        if (Vegtable_target.xy.x != -1)
        {
            CurrentAction = Actions.Goingtofood;
            return true;
        }
        
        return false;
    }

    bool FindFood()
    {

        return false;
    }


    private bool foodHerbivore()
    {
        if (CurrentAction == Actions.Goingtofood && x == Vegtable_target.xy.x && y == Vegtable_target.xy.y && Vegtable_target != null)
        {
            CurrentAction = Actions.Eating;
            return true;
        }


        if ( CurrentAction != Actions.Goingtofood)
        {
            if (FindVegtable())
                return true;
        }


        return false;
    }
    private bool foodMeat()
    {

  
        if (CurrentAction != Actions.Goingtofood)
        {

            if (findMeat())
            {
                return true;
            }
        }
        return false;

    }

   
    
    bool findMeat()
    {
        eating = EntityTracker.Instance.CheckPray(x, y, range, Specie);
        if (eating != null)
        {
            CurrentAction = Actions.chasing;
            return true;
        }

        return false;

    }


    private void DoActions()
    {

        
        switch (CurrentAction)
        {
            case Actions.Exploring:
                goToRandomDirection();
                break;

            case Actions.Goingtofood:
                if (herbivore)
                    SetTargetLocation(Vegtable_target.xy.x, Vegtable_target.xy.y);
                
                break;

            case Actions.GoingToWater:
                SetTargetLocation(WaterAdj.x, WaterAdj.y);
                break;

            case Actions.escaping:
                
                SetTargetLocation(Tarx, tary);
                break;

            case Actions.chasing:

                break;


        }
    }

    void Decompose()
    {

        decompose += Time.deltaTime * 1 / TimetoDecompose;

        if (decompose >= 1)
        {
            Die(Death.decompose);
        }
    }


    void SetTargetLocation(int Lx, int Ly)
    {
        Tarx = Lx;
        tary = Ly;

        PathList = EntityTracker.Instance.FindPath(Tarx, tary, x, y);
        
    }




    bool checkforPredators()
    {

        
            // AnimialNum = 4;

            Predator = EntityTracker.Instance.CheckPredators(x, y, range, Specie);
            if (Predator != null)
            {
            //run away
            CurrentAction = Actions.escaping;


            //round movespeed to 10s 30 movespeed == top

            Tarx = x;
            tary = y;

            if (Predator.x > x)
                Tarx -= speed;
            else if (Predator.x < x)
                Tarx += speed;

            if (Predator.y > y)
                tary -= speed;
            else if (Predator.y < y)
                tary += speed;

            return true;
            }
            else
            {
            // Debug.Log("no pred");
            return false;
            }

        
            
    }

    void UpdateStatus() {

        if (Hunger >= 1)
        {
            Die(Death.Hunger);
        }
        else if (Thirst >= 1)
        {
            Die(Death.Thirst);
        }




        if (CurrentAction == Actions.Drinking)
        {
            if (Thirst > 0.05)
            {
                Thirst -= Time.deltaTime * 1 / drinkSpeed;
                Thirst = Mathf.Clamp01(Thirst);

            }
            else
                CurrentAction = Actions.Exploring;
        } 

    else  if (CurrentAction == Actions.Eating)
        {

            if (Hunger > 0.05)
            {


                Hunger -= Time.deltaTime * 1 / eatSpeed;
                Hunger = Mathf.Clamp01(Hunger);


            }
            else
            {
                CurrentAction = Actions.Exploring;

                if (herbivore)
                    Vegtable_target.eaten();
                else
                    eating.eaten();
                
            }
        }
    
    
    if (CurrentAction == Actions.chasing) // maybe just put this in going to food???
        {
            
            if (EntityTracker.Instance.GetDistantance(x, y, eating.x, eating.y) < 1.5) {
                CurrentAction = Actions.Eating;
                eating.Die(Death.Killed);
                
            }

            else  
            {
                //Debug.Log("newPath");
                SetTargetLocation(eating.x, eating.y);

            }
            
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

            Coordinate.x = x;
            Coordinate.y = y;
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
