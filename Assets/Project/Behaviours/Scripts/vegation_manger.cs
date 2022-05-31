using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class vegation_manger : MonoBehaviour
{

    //Instance
    public static vegation_manger Instance { get; private set; }

    //Prefabs
    public Vegtable Vegation_Prefab;
    public GameObject Vegtables;


    public List<Vegetion> RabbitDiet;
    public List<Vegetion> FoxDiet;
    public List<Vegetion> deerDiet;
    public List<Vegetion> BoarDiet;
    public List<Vegetion> raccoonDiet;
    public List<Vegetion> SquriellDiet;
    public List<Vegetion> RhinoDiet;
    public List<Vegetion> BearDiet; //berries, grain, fish mammals
    public List<Vegetion> GoirralDiet;
    public List<Vegetion> FrogDiet;
    public List<Color> Vegcolors;

    public Text VegtablesT;
    int width =200;
    int height  =200;
    //Dictaronrys for vegtbales
    Dictionary<Species, List<Vegetion>> Eatablevegatblesbyspecies;
    Dictionary<Vegetion, List<Vegtable>> ListofVegtables;
    Dictionary<Season, List<Vegetion>> VegetionBySeason;
    public List<Vegetion> Vegtabless;
    //References to map
    Color[] Colour_Map;
    Color ColourWater;
    Color colourRock;

    //lists containing known vegtable and vegtable seed locations
    List<Vegtable> VegtableSeeds;
    List<Coords> Locations;

    //repeated data types that a reused to save memory
    Vector3 location;
    Coords VegCord;

    //Season data
    Season currentSeason;
    public int SpreadSeedChance = 5;

    //List of vegetion that spawns per season
    public List<Vegetion> Summer;
    public List<Vegetion> Augest;
    public List<Vegetion> Fall;
    public List<Vegetion> Winter;
    
    //Data for spawning vegtables, and real time updates
    public int SpawnAmount = 10;
    float LastWeek;
    float TimeBetweenWeeks = 30;
    float week = 0;
    List<Coords> GroupCentre;
    int Index = 0;
    void Start()
    {
        Instance = this;
    }


    public void init(Color[] Map_Colour, Color WaterColour, Color RockColour)
    {

        //Set references from map creation
        Colour_Map = Map_Colour;
        ColourWater = WaterColour;
        colourRock = RockColour;

        //Create lists
        Locations = new List<Coords>();
        VegtableSeeds = new List<Vegtable>();



        //for spread seed
        VegetionBySeason = new Dictionary<Season, List<Vegetion>>();
        VegetionBySeason.Add(Season.Summer, Summer);
        VegetionBySeason.Add(Season.fall, Fall);
        VegetionBySeason.Add(Season.autum, Augest);
        VegetionBySeason.Add(Season.winter, Winter);







        //Set up dictationiers
        Eatablevegatblesbyspecies = new Dictionary<Species, List<Vegetion>>();
        Eatablevegatblesbyspecies.Add(Species.Rabbit, RabbitDiet);
        Eatablevegatblesbyspecies.Add(Species.fox, FoxDiet);
        Eatablevegatblesbyspecies.Add(Species.bear, BearDiet);
        Eatablevegatblesbyspecies.Add(Species.boar, BoarDiet);
        Eatablevegatblesbyspecies.Add(Species.deer, deerDiet);
        Eatablevegatblesbyspecies.Add(Species.Frogs, FrogDiet);
        Eatablevegatblesbyspecies.Add(Species.gorrila, GoirralDiet);
        Eatablevegatblesbyspecies.Add(Species.lion, new List<Vegetion>());
        Eatablevegatblesbyspecies.Add(Species.raccoon, raccoonDiet);
        Eatablevegatblesbyspecies.Add(Species.Rhino, RhinoDiet);
        Eatablevegatblesbyspecies.Add(Species.squirrel,SquriellDiet);



        //set up liss in dictonaries
        ListofVegtables = new Dictionary<Vegetion, List<Vegtable>>();

        for (int i = 0; i < Vegtabless.Count; i++)
        {
            ListofVegtables.Add(Vegtabless[i], new List<Vegtable>());

        }
        
        

        //Create a carrot in the world (game wont work when I dlete this for some reason
        VegCord = new Coords(35, 95);
        location = EntityTracker.Instance.Coordtoworld(VegCord);
        Vegtable NewVeg = Instantiate(Vegation_Prefab, location, Quaternion.identity);
        NewVeg.type = Vegetion.carrot;

        //VEG EEEE TION == enum
        NewVeg.xy = VegCord;
        ListofVegtables[(Vegetion.carrot)].Add(NewVeg);
        Locations.Add(NewVeg.xy);



        //Create initial vegtables
        NewSeasonVegtables(currentSeason, 10);


    }


    void Update()
    {

        VegtablesT.text = "Vegtables: " + Locations.Count;


        float TimeSinceLastAction = Time.time - LastWeek;

        //7 minutes passed == one week in game
        if (TimeSinceLastAction > TimeBetweenWeeks)
        {

            Debug.Log("week passed");

            week++;

            if (week == 1)
                SpreadSeeds(currentSeason);


            else if (week == 3)
                spawnSeeds();


            else if (week > 3)
            {
                if (currentSeason == Season.winter)
                    currentSeason = Season.Summer;
                else 
                    currentSeason++;

                week = 0;
                NewSeasonVegtables(currentSeason, 2);


            }

            LastWeek = Time.time;
        }
    }




    public void NewSeasonVegtables(Season Type, int groups)
    {
        
        GroupCentre = new List<Coords>();

        int Amount;
        //Debug.Log(SpawnAmount);
        switch (Type)
        {
            case Season.Summer:
                
                for (int i = 0; i < Summer.Count; i++) {
                   Amount = Random.Range(SpawnAmount - 10, SpawnAmount);
                
                    SpawnVegtablesoftype(Amount, Summer[i], groups);
                    
                    }
                break;

            case Season.fall:

                for (int i = 0; i < Fall.Count; i++) {
                    Amount = Random.Range(SpawnAmount - 12, SpawnAmount - 5);
                SpawnVegtablesoftype(Amount, Fall[i], groups);
                }
                break;

            case Season.autum:

                for (int i = 0; i < Augest.Count; i++) {
                    Amount = Random.Range(SpawnAmount - 15, SpawnAmount - 6);
                    SpawnVegtablesoftype(Amount, Augest[i], groups);
                }
                break;

            case Season.winter:

                Amount = Random.Range(1, 10);
                for (int i = 0; i < Winter.Count; i++)
                    SpawnVegtablesoftype(Amount, Winter[i], groups);
                break;

        }


    }






    public void SpawnVegtablesoftype(int amount, Vegetion type, int groups)
    {


       

        int GroupINdex = 0;
        
        
        while (GroupINdex < groups)
        {

            int rInt = Random.Range(0, 190);
            int rYnt = Random.Range(0, 190);


            Coords tempC = new Coords(rInt, rYnt);

            if (GroupCentre.Count < 1)
            {
                GroupCentre.Add(tempC);
                GroupINdex++;

            }
            else
            {
                for (int j = 0; j < GroupCentre.Count; j++)
                {
                    float dist = EntityTracker.Instance.GetDistantance(rInt, rYnt, GroupCentre[j].x, GroupCentre[j].y);
                    if (dist >= 10)
                    {
                        GroupCentre.Add(tempC);
                        GroupINdex++;
                        j = 1000;
                    }


                }
            }

        }

        

        int lol = 0;

        //HONESTLY DONT CARE (BUG THAT SOME SPAWN ON TOP OF EACH OTHER)
        while (lol < groups)
        {
            int TemporyAmount = 0;
            
            while (TemporyAmount < amount)
            {
                
                //chose random location
                int rInt = Random.Range(GroupCentre[Index].x, GroupCentre[Index].x + 10);
                int rYnt = Random.Range(GroupCentre[Index].y, GroupCentre[Index].y + 10);

                if (Colour_Map[rInt * 200 + rYnt] != ColourWater && Colour_Map[rInt * 200 + rYnt] != colourRock)
                {

                    for (int i = 0; i < Locations.Count; i++)
                    {

                        if (rInt != Locations[i].x && rYnt != Locations[i].y)
                        {

                            
                            //Spawn vegtable if passed checks
                            VegCord.x = rInt;
                            VegCord.y = rYnt;

                            location = EntityTracker.Instance.Coordtoworld(VegCord);
                            location.y -= 4;

                            Vegtable NewVeg = Instantiate(Vegation_Prefab, location, Quaternion.identity);
                            NewVeg.type = type;
                            NewVeg.xy = new Coords(rInt, rYnt);
                            //Debug.Log(NewVeg.type);
                         //   Debug.Log(type);

                            //add to dictariony and location list
                            ListofVegtables[(type)].Add(NewVeg);
                            Locations.Add(NewVeg.xy);
                            TemporyAmount++;
                            NewVeg.transform.SetParent(Vegtables.transform);
                            NewVeg.GetComponentInChildren<Renderer>().material.color = Vegcolors[((int)type)];

                            break;

                        }


                    }

                }


            }
            Index++;
            lol++;
        }


    }


    //spawn seeds located on the map
    public void spawnSeeds()
    {

        for (int i = 0; i < VegtableSeeds.Count; i++)
        {
            location = EntityTracker.Instance.Coordtoworld(VegtableSeeds[i].xy);
            location.y -= 4;

            Vegtable NewVeg = Instantiate(Vegation_Prefab, location, Quaternion.identity);
            NewVeg.type = VegtableSeeds[i].type;
            NewVeg.xy = VegtableSeeds[i].xy;
            ListofVegtables[(NewVeg.type)].Add(NewVeg);
            //dont have to add to location since already has the coord
            

        }

    }



    public void SpreadSeeds(Season SeasonVeg)
    {

        List<Coords> tempVegLocations = new List<Coords>();
        List<Vegetion> TempEnumList = VegetionBySeason[SeasonVeg];
        List<Coords> PossibleSpawns = new List<Coords>();


        for (int i = 0; i < TempEnumList.Count; i++)
        {
            List<Vegtable> TempVegList = ListofVegtables[TempEnumList[i]];
           
            for (int j = 0; j < TempVegList.Count; j++)
            {

                int Chance = Random.Range(0, SpreadSeedChance); // random chance to spread seed
                if (Chance > 1)
                    continue;

                tempVegLocations = new List<Coords>();

                for (int k = 0; k < Locations.Count; k++)
                {
                    
                    float length = EntityTracker.Instance.GetDistantance(TempVegList[j].xy.x, TempVegList[j].xy.y, Locations[k].x, Locations[k].y);
                    if ( length < 2)
                    {
                        
                        tempVegLocations.Add(Locations[k]); //check to see if location already occupied by a vegtable

                    }
                }


                PossibleSpawns = new List<Coords>();
                if (tempVegLocations.Count < 8)
                {


                   

                    //loop through 3x3 square around vegtable
                    for (int x = TempVegList[j].xy.x - 1; x <= TempVegList[j].xy.x + 1; x++)
                    {
                        for (int y = TempVegList[j].xy.y - 1; y <= TempVegList[j].xy.y + 1; y++)
                        {

                            if (x < 0 || x > width - 2 || y > height - 2 || y < 0)
                            {
                                //do nothing
                            }
                            else
                            {
                                if (Colour_Map[x * 200 + y] != ColourWater && Colour_Map[x * 200 + y] != colourRock)
                                {



                                    for (int l = 0; l < tempVegLocations.Count; l++)
                                    {
                                        //Debug.Log(tempVegLocations.Count);
                                        //possible spawns
                                        if (tempVegLocations[l].x != x && tempVegLocations[l].y != y)
                                        {

                                            Coords possibleCoord = new Coords(x, y);
                                            PossibleSpawns.Add(possibleCoord);
                                            break;


                                        }
                                    }
                                }
                            }
                            
                           
                        }
                    }
                }

                if (PossibleSpawns.Count > 0)
                {
                    
                    int Rand = Random.Range(0, PossibleSpawns.Count); // randomly choose on the possible spawn locations for the seed

                    Vegtable SeededVeg; 
                    SeededVeg  = new Vegtable(PossibleSpawns[Rand], TempVegList[j].type);
                   
                    //add to locations and seed list
                    Locations.Add(PossibleSpawns[Rand]);
                    VegtableSeeds.Add(SeededVeg);

                }
            }
        }
    }




    public void removeVeg(Vegetion VegType, Vegtable veg, Coords Loca)
    {
        ListofVegtables[VegType].Remove(veg);
        Locations.Remove(Loca);

    }

    public Vegtable FindVegatble(int x, int y, int range, Species Specis)
    {

      //  Debug.Log("finding");

        Coords lol2 = new Coords(-1, -1);
        Vegtable returnvegtable = new Vegtable(lol2,Vegetion.carrot);

        returnvegtable.xy.x = -1;
        List<Vegetion> VegtebaleSpeciesList = Eatablevegatblesbyspecies[Specis];

        
            

        //VEGTABLE == class not Vegtable

        for (int i = 0; i < VegtebaleSpeciesList.Count; i++)
        {
            if (Specis == Species.lion)
                continue;
            // Debug.Log(i);
            List<Vegtable> VegtableList = ListofVegtables[VegtebaleSpeciesList[i]];

            for (int j = 0; j < VegtableList.Count; j++)
            {


                // Debug.Log(j);
                float distant = EntityTracker.Instance.GetDistantance(x, y, VegtableList[j].xy.x, VegtableList[j].xy.y);
                //Debug.Log("dist");

                //add check for if prey to another predator
                if (distant < range && distant > 0)
                {
                    returnvegtable = VegtableList[j];
                    //Debug.Log("distss");
                }
            }
        }


        return returnvegtable;
    }



}