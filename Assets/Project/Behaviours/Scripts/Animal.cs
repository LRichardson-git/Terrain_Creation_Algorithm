using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Alive_entity
{


    //General
    float LastActionTime;
    float TimeBetweenACtions = 4;
    

    //speeds
    float drinkSpeed = 7;
    float eatSpeed = 11;
    public float movespeed = 20;


    
    //Genes
    bool herbivore = true;
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

    //Death
    float MaxHunger = 45;
    float MaxThirst = 190;
    float TimetoDecompose = 90;

    //Other Entities
    List<Alive_entity> Predators;
    Alive_entity eating;
    Vegtable Vegtable_target;

    //STATUS
    public float Hunger;
    public float Thirst;
    public float decompose;
    public float horny;

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
        movespeed +=  geneValues[0] * 0.2f ;
        if (geneValues[1] > 225) // randomize colour if this gene passes test
        {
            for (int i = 0; i < 4; i++)
            {
                Matcolour[i] = Random.Range(0f, 1f);

            }
        }


        range = geneValues[2] / 25 ;
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
                checkforPredators();

                ChooseAction();

                DoActions();
                

                LastActionTime = Time.time;
                //choost action

        }

        

        UpdateStatus();
    
            



            FindPath();
        
        }
    }

    private void ChooseAction()
    {
        //IF TIRED == 1 INSTANTLY REST


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



        if (Hunger >= 0.4)
        {
            bool temp = false;

            if (herbivore)
                temp = foodHerbivore();
            else
                temp = foodMeat();

            if (temp)
                return;

            CurrentAction = Actions.Exploring;
            return;

        }

        

        if (horny > 0.8)
        {
            //do thing
        }

        if (CurrentAction != Actions.Eating && CurrentAction != Actions.Drinking)
            CurrentAction = Actions.Exploring;


    }


    public void goToRandomDirection()
    {

        if (PathList == null)
        {

            int lol = Random.Range(1, 5);
            int rInt = Random.Range(1, 3);
            int rYnt = Random.Range(1, 3);

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
        return true;
    }
    


    private void DoActions()
    {

        
        switch (CurrentAction)
        {
            case Actions.Exploring:
                goToRandomDirection();
                break;

            case Actions.Goingtofood:
                if(herbivore)
                    SetTargetLocation(Vegtable_target.xy.x, Vegtable_target.xy.y);
                else//food
                    SetTargetLocation(Vegtable_target.xy.x, Vegtable_target.xy.y);//Change
                break;

            case Actions.GoingToWater:
                SetTargetLocation(WaterAdj.x, WaterAdj.y);
                break;

            case Actions.escaping:

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

    else  if (CurrentAction == Actions.Eating && Vegtable_target != null)
        {

            if (Hunger > 0.05)
            {


                Hunger -= Time.deltaTime * 1 / eatSpeed;
                Hunger = Mathf.Clamp01(Hunger);


            }
            else
            {
                CurrentAction = Actions.Exploring;
                Vegtable_target.eaten();
                Debug.Log("eaten");
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
