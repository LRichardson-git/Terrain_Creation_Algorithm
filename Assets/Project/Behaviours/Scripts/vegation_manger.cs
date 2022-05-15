using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vegation_manger : MonoBehaviour
{

    //Instance
    public static vegation_manger Instance { get; private set; }

    //Prefabs
    public Vegtable Vegation_Prefab;



    //Dictaronrys for vegtbales
    Dictionary<Species, List<Vegetion>> Eatablevegatblesbyspecies;
    Dictionary<Vegetion, List<Vegtable>> ListofVegtables;
    Dictionary<Season, List<Vegetion>> VegetionBySeason;

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
    public int SpawnAmount = 30;
    float LastWeek;
    float TimeBetweenWeeks = 30;
    float week = 0;


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
        VegetionBySeason.Add(Season.Summer, new List<Vegetion>());
        VegetionBySeason.Add(Season.fall, new List<Vegetion>());
        VegetionBySeason.Add(Season.autum, new List<Vegetion>());
        VegetionBySeason.Add(Season.winter, new List<Vegetion>());

        VegetionBySeason[Season.Summer] = Summer;
        VegetionBySeason[Season.fall] = Fall;
        VegetionBySeason[Season.autum] = Augest;
        VegetionBySeason[Season.winter] = Winter;





        //Set up dictationiers
        Eatablevegatblesbyspecies = new Dictionary<Species, List<Vegetion>>();
        Eatablevegatblesbyspecies.Add(Species.Rabbit, new List<Vegetion>());
        Eatablevegatblesbyspecies.Add(Species.fox, new List<Vegetion>());

        Eatablevegatblesbyspecies[(Species.Rabbit)].Add(Vegetion.carrot);
        Eatablevegatblesbyspecies[(Species.Rabbit)].Add(Vegetion.letuce);
        Eatablevegatblesbyspecies[(Species.Rabbit)].Add(Vegetion.apples);


        //set up liss in dictonaries
        ListofVegtables = new Dictionary<Vegetion, List<Vegtable>>();
        ListofVegtables.Add(Vegetion.carrot, new List<Vegtable>());
        ListofVegtables.Add(Vegetion.apples, new List<Vegtable>());
        ListofVegtables.Add(Vegetion.letuce, new List<Vegtable>());
        ListofVegtables.Add(Vegetion.tomatoes, new List<Vegtable>());


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
        NewSeasonVegtables(currentSeason);


    }


    void Update()
    {



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
                NewSeasonVegtables(currentSeason);


            }

            LastWeek = Time.time;
        }
    }




    public void NewSeasonVegtables(Season Type)
    {


        switch (Type)
        {
            case Season.Summer:
                
                for (int i = 0; i < Summer.Count; i++) {
                    SpawnAmount = Random.Range(SpawnAmount - 10, SpawnAmount);
                    SpawnVegtablesoftype(SpawnAmount, Summer[i]);
                    }
                break;

            case Season.fall:

                for (int i = 0; i < Fall.Count; i++) { 
                SpawnAmount = Random.Range(SpawnAmount - 12, SpawnAmount - 5);
                SpawnVegtablesoftype(SpawnAmount, Fall[i]);
                }
                break;

            case Season.autum:

                for (int i = 0; i < Augest.Count; i++) {
                    SpawnAmount = Random.Range(SpawnAmount - 15, SpawnAmount - 6);
                    SpawnVegtablesoftype(SpawnAmount, Augest[i]);
                }
                break;

            case Season.winter:

                SpawnAmount = Random.Range(1, 10);
                for (int i = 0; i < Winter.Count; i++)
                    SpawnVegtablesoftype(SpawnAmount, Winter[i]);
                break;

        }


    }






    public void SpawnVegtablesoftype(int amount, Vegetion type)
    {


        int TemporyAmount = 0;
        while (TemporyAmount < amount)
        {
            //chose random location
            int rInt = Random.Range(0, 200);
            int rYnt = Random.Range(0, 200);

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

                        //add to dictariony and location list
                        ListofVegtables[(type)].Add(NewVeg);
                        Locations.Add(NewVeg.xy);
                        TemporyAmount++;

                        i = 50000;

                    }


                }

            }


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

        Coords lol2 = new Coords(-1, -1);
        Vegtable returnvegtable = new Vegtable(lol2,Vegetion.carrot);

        returnvegtable.xy.x = -1;
        List<Vegetion> VegtebaleSpeciesList = Eatablevegatblesbyspecies[Specis];

        //VEGTABLE == class not Vegtable

        for (int i = 0; i < VegtebaleSpeciesList.Count; i++)
        {

            // Debug.Log(i);
            List<Vegtable> VegtableList = ListofVegtables[VegtebaleSpeciesList[i]];

            for (int j = 0; j < VegtableList.Count; j++)
            {


                // Debug.Log(j);
                float distant = EntityTracker.Instance.GetDistantance(x, y, VegtableList[j].xy.x, VegtableList[j].xy.y);


                //add check for if prey to another predator
                if (distant < range && distant > 0)
                {
                    returnvegtable = VegtableList[j];
                }
            }
        }


        return returnvegtable;
    }



}