using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Spawner : MonoBehaviour
{
    // Start is called before the first frame update


    public int SpawnNumb = 10;
    public GameObject test;
    public GameObject Meshh;

    public void SpawnAnimals(int width, int height, float[,] heightmap, int[] BuldingMap,
        AnimationCurve HeightCurve, float mesh_Height, int seed)
    {
        //add mesh position here
        Vector3 startPos = new Vector3(0, 0, 0);
        System.Random Ran_Seed = new System.Random(seed);

        int size = 0;
        for (int i = 0; i < SpawnNumb; i++)
        {
            bool spawnPoint = false;


            while (spawnPoint == false)
            {



                int OffSet_X = Ran_Seed.Next(0, 200);
                int OffSet_Y = Ran_Seed.Next(0, 200);

                if (heightmap[OffSet_X, OffSet_Y] > 0.35 && heightmap[OffSet_X, OffSet_Y] < 0.65)
                {




                    Vector3 RelativePosition = new Vector3(0, 0, 0);
                    RelativePosition.x = (-width * Meshh.transform.localScale.x) / 2 + 5;
                    RelativePosition.z = (height * Meshh.transform.localScale.z) / 2 - 5;



                    RelativePosition.x +=
                        OffSet_X * Meshh.transform.localScale.x + Meshh.transform.localScale.x / 2; //Get location on Map


                    RelativePosition.z -=
                        OffSet_Y * Meshh.transform.localScale.z +
                        Meshh.transform.localScale.z / 2; //Z is used for Y axis in the 3d world

                    RelativePosition.y =
                        HeightCurve.Evaluate(heightmap[OffSet_X, OffSet_Y]) * mesh_Height * Meshh.transform.localScale.y; // Calulate height


                    //generate
                    Instantiate(test, RelativePosition, Quaternion.identity);
                    spawnPoint = true;
                    // int buildingSize = 0;

                    //small buildings




                }
                //0,4 0.65

                size++;
                if (size > 500)
                {
                    size = 0;
                    spawnPoint = true;
                    Debug.Log("Could not find spawnpoint");

                }
            }
        }
    }
        
        //array for buildings to be put into, 
   
  
            void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
