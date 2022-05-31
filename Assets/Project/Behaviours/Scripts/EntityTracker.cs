using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//envir
public class EntityTracker : MonoBehaviour
{

    public static EntityTracker Instance { get; private set; }

    //general
    public int seed;
    public int width;
    public int height;
    Color[] Colour_Map;
    List<Coords> GroupCentre;
    int Deaths;
    public List<int> DeathNumbers;
    //debuuggin
    public GameObject Animals;

    public Text DeathText;
    public Text Births;
    bool started;
    //Pathfinding
    public static bool[,] walkable;
    private const int m_Move_Straight_Cost = 10;
    private const int m_Move_Diagonal_cost = 14;
    private int Debugg;
    public Coords[,] MapIndex;
    public List<Death> REASONS;
    int births;
    int AmountPerGroup = 20;
    int Groups1 = 10;
    
    public Text Spawning1;
    public Text Spawning2;
    
    //water
    List<Coords> WaterTiles;
    List<Coords> WaterTilesAdjacent;

    //List of what things eat

    public List<Species> FoxDietMeat;
    public List<Species> raccoonDietMeat;
    public List<Species> SquriellDietMeat;
    public List<Species> LionDiet;//deer boar rhino raccon (if really hungry BEAR GOIRRLA)
    public List<Species> BearDietmeat;
    public List<Species> IncludedSpecies;
    public List<Species> IncludedSpeciesnonmeat;

    //storing lists o
    public Dictionary<Species, List<Alive_entity>> SpeciesMap;
    public Dictionary<Species, List<Species>> PreySpecies;
    public Dictionary<Species, List<Species>> PredatorSpecies;


    //Prefabs
    public List<Animal> Alive_Entities_Prefabs;
    List<Species> SpeciesTypeList;
    Vector3 TempLoc;





    void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        DeathText.text = "Deaths: " + Deaths;
        Births.text = "Births: " + births;

        if (started == false)
        {

            if (Input.GetKeyDown("5"))
            {
                AmountPerGroup--;

            }

            if (Input.GetKeyDown("6"))
            {
                AmountPerGroup++;

            }
            //lol

            if (Input.GetKeyDown("7"))
            {
                Groups1--;

            }

            if (Input.GetKeyDown("8"))
            {
                Groups1++;

            }

            Spawning1.text = "AnimalsPerGroup: " + AmountPerGroup;
            Spawning2.text = "Groups of animals: " + Groups1;


        }
    }

        public void RemoveEntity(Species speciy, Alive_entity Entity)
    {
        SpeciesMap[speciy].Remove(Entity);

    }



    public float GetDistantance(int ax, int ay, int bx, int by)
    {
        return (float)System.Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by));

    }


    public Alive_entity CheckPredators(int x, int y, int Range, Species Specis)
    {

        Alive_entity Predat = null;

        // Debug.Log(Specis);

        float maxdist = Range;

        List<Species> PredatorSpeciesList = PredatorSpecies[Specis];

        for (int i = 0; i < PredatorSpeciesList.Count; i++)
        {


            List<Alive_entity> PredSpecieL = SpeciesMap[PredatorSpeciesList[i]];

            for (int j = 0; j < PredSpecieL.Count; j++)
            {

                float distant = GetDistantance(x, y, PredSpecieL[j].x, PredSpecieL[j].y);


                //add check for if prey to another predator
                if (distant < maxdist && PredSpecieL[j].dead == false)
                {
                    Predat = PredSpecieL[j];
                    maxdist = distant;
                }
            }
        }


        return Predat;


    }

    public void UpdateDeath(Death Reason){
        Deaths++;
        DeathNumbers[((int)Reason)]++;
        }

    public void DebugPring()
    {
        Debug.Log(Deaths);
        for (int i = 0; i < DeathNumbers.Count; i++)
        {
            Debug.Log(REASONS[i]);
            Debug.Log(DeathNumbers[i]);

        }

        
    }

    public Alive_entity CheckPray(int x, int y, int Range, Species Specis)
    {

        Alive_entity Predat = null;

        // Debug.Log(Specis);

        float maxdist = Range;

        List<Species> PreyspecisList = PreySpecies[Specis];

  
        
        for (int i = 0; i < PreyspecisList.Count; i++)
        {

            List<Alive_entity> PredSpecieL = SpeciesMap[PreyspecisList[i]];
            for (int j = 0; j < PredSpecieL.Count; j++)
            {

                float distant = GetDistantance(x, y, PredSpecieL[j].x, PredSpecieL[j].y);
          
                //add check for if prey to another predator
                if (distant < maxdist)
                {
                    Predat = PredSpecieL[j];
                    maxdist = distant;
                }
            }
        }


        return Predat;


    }

    public virtual void Init(Color[] Map_Colour, Color Biome2, int width, float[,] Noise)
    {

        MapIndex = new Coords[width, height];
        WaterTiles = new List<Coords>();
        WaterTilesAdjacent = new List<Coords>();
        Coords watertile;
        
        Colour_Map = Map_Colour;
        GroupCentre = new List<Coords>();

        SpeciesTypeList = new List<Species>();
        SpeciesTypeList.Add(Species.Rabbit);
        SpeciesTypeList.Add(Species.fox);
        SpeciesTypeList.Add(Species.deer);
        SpeciesTypeList.Add(Species.boar);
        SpeciesTypeList.Add(Species.raccoon);
        SpeciesTypeList.Add(Species.squirrel);
        SpeciesTypeList.Add(Species.Rhino);
        SpeciesTypeList.Add(Species.lion);
        SpeciesTypeList.Add(Species.bear);
        SpeciesTypeList.Add(Species.gorrila);
        SpeciesTypeList.Add(Species.Frogs);
        


        //Debugg = 85;
        //lopp through water
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                MapIndex[x, y] = new Coords(x, y);

                if (Map_Colour[x * 200 + y] == Biome2)
                {

                    //Debug.Log(x);
                   // Debug.Log(y);
                    MapIndex[x, y].IsWalkable = false;
                    watertile = new Coords(x, y);
                    WaterTiles.Add(watertile);

                    if (x > 0 && x < 200 && y > 0 && y < 200) { 
                    
                        if (Map_Colour[x + 1 * 200 + y] != Biome2)
                        {
                             
                            Coords AdjancetTile = new Coords(x + 1, y);
                        WaterTilesAdjacent.Add(AdjancetTile);
                        }
                       // Debug.Log(x - 1 * 200 + y);
                       // Debug.Log( y);
                        //Debug.Log((x - 1) * 200 + y);
                        if (Map_Colour[(x - 1) * 200 + y] != Biome2)
                        {
                            Coords AdjancetTile = new Coords(x - 1, y);
                        WaterTilesAdjacent.Add(AdjancetTile);

                    }
                        if (Map_Colour[x  * 200 + y + 1] != Biome2)
                        {
                            Coords AdjancetTile = new Coords(x, y + 1);
                        WaterTilesAdjacent.Add(AdjancetTile);

                    }

                        if (Map_Colour[x * 200 + y - 1] != Biome2)
                        {
                            Coords AdjancetTile = new Coords(x, y-1);
                        WaterTilesAdjacent.Add(AdjancetTile);

                    }

                    if (Map_Colour[x + 1 * 200 + y + 1] != Biome2)
                    {

                            Coords AdjancetTile = new Coords(x + 1, y + 1);
                        WaterTilesAdjacent.Add(AdjancetTile);
                    }
                    if (Map_Colour[(x - 1) * 200 + y - 1] != Biome2)
                    {

                            Coords AdjancetTile = new Coords(x - 1, y - 1);
                        WaterTilesAdjacent.Add(AdjancetTile);
                    }
                    if (Map_Colour[(x - 1) * 200 + y + 1] != Biome2)
                    {
                            Coords AdjancetTile = new Coords(x - 1, y + 1);
                        WaterTilesAdjacent.Add(AdjancetTile);

                    }
                    if (Map_Colour[x + 1 * 200 + y - 1] != Biome2)
                    {
                            Coords AdjancetTile = new Coords(x + 1, y - 1);
                        WaterTilesAdjacent.Add(AdjancetTile);


                    }
                    }


                }

            }
        }



        //create dictionarys for tracking entitiers and getting species
    
        SpeciesMap = new Dictionary<Species, List<Alive_entity>>();

        for (int i = 0; i < IncludedSpecies.Count; i++)
        {
            SpeciesMap.Add(IncludedSpecies[i], new List<Alive_entity>());
        }

        PreySpecies = new Dictionary<Species, List<Species>>();

        for (int i = 0; i < IncludedSpeciesnonmeat.Count; i++)
        {
            PreySpecies.Add(IncludedSpeciesnonmeat[i], new List<Species>());

        }
        PreySpecies.Add(Species.fox,  FoxDietMeat);
        PreySpecies.Add(Species.raccoon, raccoonDietMeat);
        PreySpecies.Add(Species.squirrel, SquriellDietMeat);
        PreySpecies.Add(Species.lion, LionDiet);
        PreySpecies.Add(Species.bear, BearDietmeat);

        
    //for now hardcode until otherwise
        PredatorSpecies = new Dictionary<Species, List<Species>>();

        for (int i = 0; i < IncludedSpecies.Count; i++)
        {
            PredatorSpecies.Add(IncludedSpecies[i], new List<Species>());
            for (int j = 0; j < IncludedSpecies.Count; j++)
            {

                if (PreySpecies[IncludedSpecies[j]] != null)
                {

                    for (int k = 0; k < PreySpecies[IncludedSpecies[j]].Count; k++)
                    {

                        if (PreySpecies[IncludedSpecies[j]][k] == IncludedSpecies[i])
                        {
                            PredatorSpecies[(IncludedSpecies[i])].Add(IncludedSpecies[j]);

                        }
                    }

                }

            }
        }

     
        SpawnInitalAnimals(10, Biome2, 20);
        started = true;

        Debug.Log("inition done");


        
      
    }

  







    public void SpawnInitalAnimals(int amount, Color Biome2, int groups)
    {
        int[] genevalues = new int[10];

        Color Empty = new Color() ;
        int Index = 0;

        //check for max size here

        while (Index < groups)
        {

            int rInt = Random.Range(0, 190);
            int rYnt = Random.Range(0, 190);


            Coords tempC = new Coords(rInt, rYnt);

            if (GroupCentre.Count < 1)
            {
                GroupCentre.Add(tempC);
                Index++;

            }
            else
            {
                for (int j = 0; j < GroupCentre.Count; j++)
                {
                    float dist = GetDistantance(rInt, rYnt, GroupCentre[j].x, GroupCentre[j].y);
                    if (dist >= 10)
                    {
                        GroupCentre.Add(tempC);
                        Index++;
                        j = 1000;
                    }


                }
            }

        }

        Index = 0;

        for (int j = 0; j < GroupCentre.Count; j++)
        {
            
            if (Index >= Alive_Entities_Prefabs.Count)
                Index = Random.Range(0, Alive_Entities_Prefabs.Count - 1);

            bool Placed = false;
            int checker = 0;
            int Spawned = 0;

            while (Placed == false )
            {

                if (checker > 1000)
                    Placed = true;


                //bottom left is centre point for spawn group
                int rInt = Random.Range(GroupCentre[j].x, GroupCentre[j].x + 10);
                int rYnt = Random.Range(GroupCentre[j].y, GroupCentre[j].y + 10);

                Coords tempCD = new Coords(rInt, rYnt);

                checker++;
                if (Colour_Map[rInt * 200 + rYnt] != Biome2)
                {

                    //Spawn vegtable if passed checks

                    for (int k = 0; k < genevalues.Length; k++)
                    {
                        genevalues[k] = Random.Range(0, 255);
                    }

                    spawnAnimalAt(tempCD, genevalues, Index, j, Empty,true);
                    Spawned++;
                    if (Spawned >= amount)
                        Placed = true;

                }
            }
            Index++;
            
        }
    
        }









    public void spawnAnimalAt(Coords Spawn, int[] GeneValues, int specie, int index, Color MatColor, bool Initial)
    {
       // Spawn.x = 30;
     //   Spawn.y = Debugg;
        TempLoc = EntityTracker.Instance.Coordtoworld(Spawn);
        Animal NewEntity = Instantiate(Alive_Entities_Prefabs[specie], TempLoc, Alive_Entities_Prefabs[specie].transform.rotation );
        NewEntity.init(GeneValues, Spawn, GroupCentre[index],MatColor, Initial);

        SpeciesMap[(SpeciesTypeList[specie])].Add(NewEntity);
        NewEntity.transform.SetParent(Animals.transform);
        NewEntity.transform.position = TempLoc;
        //  Debugg -= 10;
        //add to dictariony and location list


    }




    public void Spawnchild(int[]Genes, int gestation, Coords spawn, int species, Color ChildColour)
    {
        //Debug.Log(species);

        //genes
        int debuff = -15;
        Genes[3] += debuff + (gestation * 2);
        Genes[4] += debuff + gestation;
        Genes[7] += debuff + gestation;
            
        for (int i = 3; i < 8; i++)
        {
            if (Genes[i] > 255)
                Genes[i] = 255;

            if (i == 4)
                i = 6;
        }

        


        //may change index since not accounts for spawn home
        spawnAnimalAt(spawn, Genes, species, 0, ChildColour,false);
        births++;

    }


    public Color Newcolour(Color c1, Color c2)
    {
         Color Newcolor = (c1 + c2) / 2;
        return Newcolor;
    }


    public int[] breed(int[] GenesFemale, int[] GenesMale)
    {
        int[] NewGenes = new int[10];

        for (int i = 0; i < NewGenes.Length; i++)
        {
            int chance = Random.Range(1, 3);

            if (chance == 1)
                NewGenes[i] = GenesMale[i];
            else
                NewGenes[i] = GenesFemale[i];



        }

        NewGenes[1] = Random.Range(1, 255);
        NewGenes[5] = GenesFemale[5];

        //might need to chance
        return NewGenes;
    }

    public Alive_entity FindMate(Species Specie, int range, Coords Position, int desirebile)
    {

        for (int i = 0; i < SpeciesMap[Specie].Count; i++)
        {

            float Length = GetDistantance(Position.x, Position.y, SpeciesMap[Specie][i].x, SpeciesMap[Specie][i].y);

            if (Length < range && SpeciesMap[Specie][i].isfemale == true)
            {
                if (SpeciesMap[Specie][i].requestMating(desirebile)) ;
                    return SpeciesMap[Specie][i];
            }
        }

        return null;

    }
































    //Find nearest Water source
    public Coords closestWater(int x, int y)
    {
        Coords Tile = new Coords(-1, -1);
        float Dist = 2;



        for (int i = 0; i < WaterTilesAdjacent.Count; i++)
        {
            float Length = GetDistantance(x, y, WaterTiles[i].x, WaterTiles[i].y);

            if (Length < Dist)
            {
                Dist = Length;
                Tile.x = WaterTiles[i].x;
                Tile.y = WaterTiles[i].y;
            }
        }

        return Tile;

    }


    public Coords FindWater(int x, int y, int range)
    {
        Coords Tile = new Coords(-1, -1);
        float Dist = range;



        for (int i = 0; i < WaterTilesAdjacent.Count; i++)
        {
            float Length = GetDistantance(x, y, WaterTilesAdjacent[i].x, WaterTilesAdjacent[i].y);

            if (Length < range)
            {
                Dist = Length;
                Tile.x = WaterTilesAdjacent[i].x;
                Tile.y = WaterTilesAdjacent[i].y;
            }
        }

        return Tile;

    }
    public Coords FindWaterAgain(int x, int y, int range, List<Coords> Unuseable)
    {
        Coords Tile = new Coords(-1, -1);
        float Dist = range;



        for (int i = 0; i < WaterTilesAdjacent.Count; i++)
        {
            float Length = GetDistantance(x, y, WaterTilesAdjacent[i].x, WaterTilesAdjacent[i].y);

            if (Length < range)
            {
                for (int j = 0; j < Unuseable.Count; j++)
                {
                    if (WaterTilesAdjacent[i].x != Unuseable[j].x && WaterTilesAdjacent[i].y != Unuseable[j].y)
                    {
                        Dist = Length;
                        Tile.x = WaterTilesAdjacent[i].x;
                        Tile.y = WaterTilesAdjacent[i].y;

                    }

                }

            }
        }

        return Tile;

    }














































    //PathFinding
    
    public Vector2 GetGrid(Vector3 Pos)
    {
        Vector2 Grid;
        Grid.x = Pos.x / 10;
        Grid.x -= 0.5f;
        Grid.y = Pos.z / 10;
        Grid.y -= 0.5f;
        return Grid;
    }


    public List<Vector3> FindPath(int xEnd, int yEnd, int xSt, int ySt)
    {
        //Debug.Log("PATH");
        if (xEnd < 1 || xEnd > 198 || yEnd < 1 || yEnd > 198)
            return null;

        if (MapIndex[xEnd, yEnd].IsWalkable == false)
        {
            

            return null;
        }
            


        Coords StarCoord = MapIndex[xSt, ySt];
        Coords EndCoord = MapIndex[xEnd, yEnd];
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
                if (neighbourN.IsWalkable == false)
                {
                    ClosedList.Add(neighbourN);
                    // Debug.Log("notWalkable");
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
        Debug.Log(xSt);
        Debug.Log(ySt);
        Debug.Log(xEnd);
        Debug.Log(yEnd);
        Debug.Log(MapIndex[xEnd, yEnd].IsWalkable);
        MapIndex[xEnd, yEnd].IsWalkable = false;


        Debug.Log("null");
        return null;

    }

    public Alive_entity SelectClosestEntity(Vector3 Position)
    {
        Vector2 lol = GetGrid(Position);

        for (int i = 0; i < IncludedSpecies.Count; i++)
        {
            for (int j = 0; j < SpeciesMap[IncludedSpecies[i]].Count; j++)
            {
                if (GetDistantance((int)lol.x, (int)lol.y, SpeciesMap[IncludedSpecies[i]][j].x, SpeciesMap[IncludedSpecies[i]][j].y) < 1.5)
                {


                    return SpeciesMap[IncludedSpecies[i]][j];
                }

            }


        }


        return null;

    }




    public Vector3 Coordtoworld(Coords Transcoord)
    {

        Vector3 Trans;
        Trans.x = Transcoord.x * 10 + 5;
        Trans.z = Transcoord.y * 10 + 5;
        Trans.y = 7f;
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
