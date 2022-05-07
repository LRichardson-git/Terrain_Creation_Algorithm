using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vegation_manger : MonoBehaviour
{
    public static vegation_manger Instance { get; private set; }
    Dictionary<Species, List<Vegetion>> Eatablevegatblesbyspecies;
    Dictionary<Vegetion, List<Vegtable>> ListofVegtables;
    public Vegtable Vegation_Prefab;
    Color[] Colour_Map;
    Color ColourWater;
    Color colourRock;
    //PUT PREFABS FOR ALL VEGTABLES HERE
    public int tiles = 40000;

    Coords VegCord;
    List<Vegetion> Vegtbles;
    List<Coords> Locations;
    Vector3 location;
    Season currentSeason;









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


        VegCord = new Coords(35,95);
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
                for (int i = 0; i < Locations.Count; i++) {

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
                        NewVeg.xy = new Coords(rInt,rYnt);
                        ListofVegtables[(type)].Add(NewVeg);
                        Locations.Add(NewVeg.xy);
                        TemporyAmount++;
                        Debug.Log(colourRock);
                        Debug.Log(Colour_Map[rInt * 200 + rYnt]);
                        Debug.Log(NewVeg.xy.x);
                        Debug.Log(NewVeg.xy.y);

                        i = 50000;

                    }
                        
                    
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
        

        Vegtable returnvegtable = new Vegtable();

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
