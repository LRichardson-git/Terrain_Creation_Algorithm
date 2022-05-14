using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vegation_manger : MonoBehaviour
{
    public static vegation_manger Instance { get; private set; }
    Dictionary<Species, List<Vegetion>> Eatablevegatblesbyspecies;
    Dictionary<Vegetion, List<Vegtable>> ListofVegtables;
    Dictionary<Season, List<Vegetion>> VegetionBySeason;
    public Vegtable Vegation_Prefab;
    Color[] Colour_Map;
    Color ColourWater;
    Color colourRock;
    List<Vegtable> VegtableSeeds;
    //PUT PREFABS FOR ALL VEGTABLES HERE
    public int tiles = 40000;

    Coords VegCord;
    List<Vegetion> Vegtbles;
    List<Coords> Locations;
    Vector3 location;
    Season currentSeason;

    // Coords possibleCoord;







    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }


    public void init(Color[] Map_Colour, Color WaterColour, Color RockColour)
    {
        Colour_Map = Map_Colour;
        ColourWater = WaterColour;
        colourRock = RockColour;

        Locations = new List<Coords>();
        




        VegtableSeeds = new List<Vegtable>();

        Eatablevegatblesbyspecies = new Dictionary<Species, List<Vegetion>>();
        Eatablevegatblesbyspecies.Add(Species.Rabbit, new List<Vegetion>());
        Eatablevegatblesbyspecies.Add(Species.fox, new List<Vegetion>());

        Eatablevegatblesbyspecies[(Species.Rabbit)].Add(Vegetion.carrot);
        Eatablevegatblesbyspecies[(Species.Rabbit)].Add(Vegetion.letuce);
        Eatablevegatblesbyspecies[(Species.Rabbit)].Add(Vegetion.apples);


        ListofVegtables = new Dictionary<Vegetion, List<Vegtable>>();
        ListofVegtables.Add(Vegetion.carrot, new List<Vegtable>());
        ListofVegtables.Add(Vegetion.apples, new List<Vegtable>());
        ListofVegtables.Add(Vegetion.letuce, new List<Vegtable>());


        VegCord = new Coords(35, 95);
        location = EntityTracker.Instance.Coordtoworld(VegCord);
        Vegtable NewVeg = Instantiate(Vegation_Prefab, location, Quaternion.identity);
        NewVeg.type = Vegetion.carrot;

        //VEG EEEE TION == enum
        NewVeg.xy = VegCord;
        ListofVegtables[(Vegetion.carrot)].Add(NewVeg);
        Locations.Add(NewVeg.xy);


        SpawnVegtablesoftype(10, Vegetion.carrot);

    }

    //spawn inital vegtables function
    //seeds fall function
    //spawn seeds function
    //NEW season, spawn new new types of vegtables
    // none season vegtables die function 

    //season enum

    public void NewSeasonVegtables(Season Type)
    {
        switch (Type)
        {
            case Season.Summer:


                break;



        }




    }






    public void SpawnVegtablesoftype(int amount, Vegetion type)
    {


        int TemporyAmount = 0;
        while (TemporyAmount < amount)
        {
            int rInt = Random.Range(0, 200);
            int rYnt = Random.Range(0, 200);

            if (Colour_Map[rInt * 200 + rYnt] != ColourWater && Colour_Map[rInt * 200 + rYnt] != colourRock)
            {
                for (int i = 0; i < Locations.Count; i++)
                {

                    if (rInt != Locations[i].x && rYnt != Locations[i].y)
                    {
                        //SPAWN THE THING
                        VegCord.x = rInt;
                        VegCord.y = rYnt;
                        location = EntityTracker.Instance.Coordtoworld(VegCord);
                        location.y -= 4;
                        Vegtable NewVeg = Instantiate(Vegation_Prefab, location, Quaternion.identity);
                        NewVeg.type = type;

                        //VEG EEEE TION == enum
                        NewVeg.xy = new Coords(rInt, rYnt);
                        ListofVegtables[(type)].Add(NewVeg);
                        Locations.Add(NewVeg.xy);
                        TemporyAmount++;

                        i = 50000;

                    }


                }

            }


        }




    }



    public void SpreadSeeds()
    {
        //Season season  << {put in the thingy at top
        List<Coords> tempVegLocations = new List<Coords>();
        //List<Vegetion> TempEnumList = VegetionBySeason[season];
        List<Vegetion> TempEnumList = new List<Vegetion>();
        TempEnumList.Add(Vegetion.carrot);
        List<Coords> PossibleSpawns = new List<Coords>();

        

        for (int i = 0; i < TempEnumList.Count; i++)
        {

            List<Vegtable> TempVegList = ListofVegtables[TempEnumList[i]];
           

            for (int j = 0; j < TempVegList.Count; j++)
            {

                //spawn random location in area that isnt occupied
               tempVegLocations = new List<Coords>();

                for (int k = 0; k < Locations.Count; k++)
                {
                    
                    float length = EntityTracker.Instance.GetDistantance(TempVegList[j].xy.x, TempVegList[j].xy.y, Locations[k].x, Locations[k].y);
                    if ( length < 2)
                    {
                        
                        tempVegLocations.Add(Locations[k]);

                    }
                }
                //  Debug.Log("here");
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
                    
                    int Rand = Random.Range(0, PossibleSpawns.Count);
                 //   Debug.Log(PossibleSpawns.Count);
                  //  Debug.Log(Rand);
                   // Debug.Log(PossibleSpawns[Rand].x);
                   // Debug.Log(TempVegList[j].type);
                    Vegtable SeededVeg; 
                      SeededVeg  = new Vegtable(PossibleSpawns[Rand], TempVegList[j].type);
                   
                    //add to locations and seed list

                    Locations.Add(PossibleSpawns[Rand]);
                    VegtableSeeds.Add(SeededVeg);

                }







            }







        }
    }

    public void Debuging()
    {
        for (int i = 0; i < VegtableSeeds.Count; i++)
        {
            Debug.Log("vegSeed");
            Debug.Log(VegtableSeeds[i].xy.x);
            Debug.Log(VegtableSeeds[i].xy.y);

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






    // Update is called once per frame
    void Update()
    {

    }

}