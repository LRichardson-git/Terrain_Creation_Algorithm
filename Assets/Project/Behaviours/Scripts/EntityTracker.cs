using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//envir
public class EntityTracker : MonoBehaviour
{
    // Start is called before the first frame update
    public int seed;

    public static bool[,] walkable;

    private const int m_Move_Straight_Cost = 10;
    private const int m_Move_Diagonal_cost = 14;

    public Alive_entity[] index;

    public static EntityTracker Instance { get; private set; }
    public int width;
    public int height;

    public Coords[,] MapIndex;
    public float[,]  NoiseMap;

    int[] MapIndexI;

    public List<Animal> Predators;

    void Start()
    {
        Instance = this;
        Init();
    }

    //change to use species ENUM
    public void UpdateMap(int Prevx, int Prevy, int x, int y, Alive_entity Entiy)
    {
        MapIndex[Prevx, Prevy].Creature = null;
        //add creatures to list of things to avoid in pathfinding
        MapIndex[x, y].Creature = Entiy;
        MapIndexI = new int[width * height];
        Debug.Log(width * height);
         MapIndexI[24 * 24] = 5;
       
    }

    public float GetDistantance(Vector2 a, Vector2 b)
    {
        return (float)System.Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));

    }

    public List<Alive_entity> CheckPredators(int x, int y, int Range, int Specis)
    {

        List<Alive_entity> PredatList = new List<Alive_entity>();

        for (int i = 0; i < Predators.Count; i++)
        {
            Vector2 a2;
            a2.x = Predators[i].x;
            a2.y = Predators[i].y;

            Vector2 a1;
            a1.x = x;
            a1.y = y;

            float distant = GetDistantance(a1, a2);


            //add check for if prey to another predator
            if (distant < Range)
            {
                PredatList.Add(Predators[i]);
            }

        }

        return PredatList;












        /*

  
        List<int> PredatList = new List<int>();

        int combin = x + y;

        for (int Sx = x - Range; Sx <= x + Range; x++)
        {
            for (int Yx = y + Range; Yx >= y - Range; y--)
            {
                if (MapIndexI[Sx * Yx] != 0 && Sx + Yx != combin)
                {
                    if (MapIndexI[Sx * Yx] > Specis)
                    {

                        PredatList.Add(Sx * Yx);
                        // Debug.Log("added");
                    }

                }
                else
                {
                    // Debug.Log("null");
                }

            }

        }

        return PredatList;











        
        List<Alive_entity> PredatList = new List<Alive_entity>();

        int combin = x + y;
        
        for (int Sx = x - Range; Sx <= x + Range; x++)
        {
            for  (int Yx = y + Range; Yx >= y - Range; y--)
            {
               if ( MapIndex[Sx,Yx].Creature != null && Sx + Yx != combin )
                {
                    if (MapIndex[Sx, Yx].Creature.Animal > Specis)
                    {

                        PredatList.Add(MapIndex[Sx, Yx].Creature);
                       // Debug.Log("added");
                    }
                    
                    }
                else
                {
                   // Debug.Log("null");
                }
                
            }

        }

        return PredatList;
        */
    }



    /*
    Coords CheckPrey()
    {


    }
   
 

    Coords FindVegation()
    {



    } */
    Vector2 FindWater(int x, int y)
    {


    }

    public virtual void Init()
    {
        //spawn species

        MapIndex = new Coords[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                MapIndex[x, y] = new Coords(x,y);

               // if ( NoiseMap[x,y]  < 0.35)
               // {

                 //   MapIndex[x, y].IsWalkable = false;
               // }

            }
        }
    }

    ///moemnt shjould be handled in entity
    ///


    public Vector2 GetGrid(Vector3 Pos)
    {
        Vector2 Grid;
        Grid.x = Pos.x / 10;
        Grid.x -= 0.5f;
        Grid.y = Pos.z / 10;
        Grid.y -= 0.5f;
        return Grid;
    }




    public List<Vector3> FindPath (int xEnd, int yEnd,int xSt, int ySt)
    {
        //Debug.Log("PATH");
        Coords StarCoord = MapIndex[xSt,ySt];
        Coords EndCoord = MapIndex[xEnd,yEnd];
      //  List<Coords> FinalPath;

        List<Coords> OpenList = new List<Coords> { StarCoord };

        List<Coords> ClosedList = new List<Coords>();

       



        //calulate offset for position in world
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Coords Cord = MapIndex[x, y];
                Cord.m_gCost = int.MaxValue;
                Cord.CalculateFCost();
                Cord.LastCoord = null;

            }
        }
        

        //now start calucalting the value of the startnode 
        //calulates cost between start node and node the object is trying to reach
        StarCoord.m_gCost = 0;
        StarCoord.m_hCost = CalculateDistanceCost(StarCoord, EndCoord);
        StarCoord.CalculateFCost();

        //Algorthim cycle---- A*


        //while we still have nodes on the open list
        while (OpenList.Count > 0)
        {
            Coords CurrentCoord = GetLowestFCostCoord(OpenList);

            if (EndCoord == CurrentCoord)
            {
                //got to final node

                return CalcuatePath(EndCoord);

            }

            //remove from openlist since checked already and put into closed list
            //closed list means that the node is not apart of the path
            OpenList.Remove(CurrentCoord);
            ClosedList.Add(CurrentCoord);

            foreach (Coords neighbourN in GetNeighbourList(CurrentCoord))
            {

                //if neighbour node on closed list it means we have already searched it so we dont care about it

                if (ClosedList.Contains(neighbourN)) continue;

                //checks if can walk past terrain
                if (!neighbourN.IsWalkable && !neighbourN.Creature)
                {
                    ClosedList.Add(neighbourN);
                    continue;
                }

                //check to see if have faster path on the current node than the neighbouring nodes
                int tentativeGCost = CurrentCoord.m_gCost + CalculateDistanceCost(CurrentCoord, neighbourN);

                if (tentativeGCost < neighbourN.m_gCost)
                {

                    //update values of neigbour and make sure on open list since it is a faster path towards endnode
                    neighbourN.LastCoord = CurrentCoord;
                    neighbourN.m_gCost = tentativeGCost;
                    neighbourN.m_hCost = CalculateDistanceCost(neighbourN, EndCoord);
                    neighbourN.CalculateFCost();

                    if (!OpenList.Contains(neighbourN))
                    {
                        OpenList.Add(neighbourN);
                    }
                }
            }
        }

        //out of nodes on list (searched through whole map and cant find path
        Debug.Log("null");
        return null;

    }

    public Vector3 Coordtoworld(Coords Transcoord)
    {
        
        Vector3 Trans;
        Trans.x = Transcoord.x * 10 + 5;
        Trans.z = Transcoord.y * 10 + 5;
        Trans.y = 15;
      //  Debug.Log(Transcoord.x);
      //  Debug.Log(Transcoord.y);
      //  Debug.Log(Trans);
        return Trans;
    }


    private int CalculateDistanceCost(Coords a, Coords b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        //returns the cost between the two distances
        return m_Move_Diagonal_cost * Mathf.Min(xDistance, yDistance) + m_Move_Straight_Cost * remaining;
    }

    private Coords GetLowestFCostCoord(List<Coords> pathnodeList)
    {
        //loops through noods list to find lowest F cost
        Coords LowestFCostNode = pathnodeList[0];
        for (int i = 1; i < pathnodeList.Count; i++)
        {
            if (pathnodeList[i].m_fCost < LowestFCostNode.m_fCost)
            {
                LowestFCostNode = pathnodeList[i];
            }
        }
        return LowestFCostNode;
    }


    private List<Vector3> CalcuatePath(Coords EndNode)
    {
        List<Coords> path = new List<Coords>();
        path.Add(EndNode);
        Coords currentNode = EndNode;
        //came from node will have the node that the algorithm deicided was the fastest
        //we simply go backwards from endnode to get out path
        while (currentNode.LastCoord != null)
        {
            path.Add(currentNode.LastCoord);
            currentNode = currentNode.LastCoord;
           // Debug.Log(currentNode.LastCoord);
        }
        //since we work our way back we reverse the path list to get it form our starting point
        path.Reverse();


        if (path == null)
        {
            return null;
        }

        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Coords coord in path)
            {
                
                vectorPath.Add(Coordtoworld(coord));

              // return vectorPath;
            }
            //Debug.Log("retunr");
            return vectorPath;
        }

        //return path;
    }

    private List<Coords> GetNeighbourList(Coords currentNode)
    {
        //only check up left down right right now
        //make list to hold neighbours
        List<Coords> neighbourList = new List<Coords>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(MapIndex[currentNode.x - 1, currentNode.y]);
            //// Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(MapIndex[currentNode.x - 1, currentNode.y - 1]);
            // Left Up
            if (currentNode.y + 1 < height) neighbourList.Add(MapIndex[currentNode.x - 1, currentNode.y + 1]);
        }
        if (currentNode.x + 1 < width)
        {
            // Right
            neighbourList.Add(MapIndex[currentNode.x + 1, currentNode.y]);
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(MapIndex[currentNode.x + 1, currentNode.y - 1]);
            // Right Up
            if (currentNode.y + 1 < height) neighbourList.Add(MapIndex[currentNode.x + 1, currentNode.y + 1]);
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(MapIndex[currentNode.x, currentNode.y - 1]);
        // Up
        if (currentNode.y + 1 < height) neighbourList.Add(MapIndex[currentNode.x, currentNode.y + 1]);

        return neighbourList;
    }


    //map or prey?
    //map of predators
}
