using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vegation_manger : MonoBehaviour
{
    public static vegation_manger Instance { get; private set; }
    Dictionary<Species, List<Vegetion>> Eatablevegatblesbyspecies;
    Dictionary<Vegetion, List<Vegtable>> ListofVegtables;
    public Vegtable Vegation_Prefab;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }


    public void init()
    {
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


        Coords coordinate = new Coords(35,95);
        Vector3 location = EntityTracker.Instance.Coordtoworld(coordinate);
        Vegtable NewVeg = Instantiate(Vegation_Prefab, location, Quaternion.identity);
        NewVeg.type = Vegetion.carrot;
        NewVeg.x = 35;
        NewVeg.y = 94;
        //VEG EEEE TION == enum

        ListofVegtables[(Vegetion.carrot)].Add(NewVeg);
 


    }

    public void removeVeg(Vegetion VegType, Vegtable veg)
    {
        ListofVegtables[VegType].Remove(veg);

    }
    public Vegtable FindVegatble(int x, int y, int range, Species Specis)
    {
        

        Vegtable returnvegtable = new Vegtable();

        returnvegtable.x = -1;
        List<Vegetion> VegtebaleSpeciesList = Eatablevegatblesbyspecies[Specis];

        //VEGTABLE == class not Vegtable

        for (int i = 0; i < VegtebaleSpeciesList.Count; i++)
        {

           // Debug.Log(i);
            List<Vegtable> VegtableList = ListofVegtables[VegtebaleSpeciesList[i]];

            for (int j = 0; j < VegtableList.Count; j++)
            {


               // Debug.Log(j);
                float distant = EntityTracker.Instance.GetDistantance(x, y, VegtableList[j].x, VegtableList[j].y);


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
