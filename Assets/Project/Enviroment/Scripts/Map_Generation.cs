using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map_Generation : MonoBehaviour
{



    public static Map_Generation mapGen;

    //Values for map generation
    public int Width;
    public int Height;
    public float Scale_Noise;

    //Disable or enable these noise types
    public bool Perlin_Noise;
    public bool value_noise;
    public bool Simplex_noise;

    public int randomsA = 100;

    //Type of map to draw
    public enum Draw_Mode
    {
        NoiseMap,
        ColourMap,
        Mesh
    };

    public Draw_Mode DrawMap;

    //type of noise to use as the base noise map
    public enum Noise_Type
    {
        Perlin,
        Value,
        simplex
    }

    public Noise_Type Base_NoiseType;

    //Settings that affect overall outcome of terrain
    [Space(25)] public int Octaves;
    [Range(0, 1)] public float Amplitude;
    public float Frequency;
    public int Seed;
    public Vector2 OffSet; //move around map
    public float MeshHeight;
    public AnimationCurve MeshHeightCurve;


    [Space(25)]
    //enable showing bulding location and spawning them
    public bool Buildings;
    public bool Animalss;
    public bool BuildingsPrefabs;
    public bool Tree_prefabs;
    public bool erosion = false;
    public int Rain_iterations = 30000;
    public double FlatLand = 0.0008;

    public bool random = false;
    public bool Auto_Update;
    public bool screnshot = false;
    float[,] Map_Noise;
    float[,] Map_Noise2;
    Color[] Map_Colour;
    float[] bulidingMap;

    //Used in editor to create the colours of the map
    public Terrain[] Biomes;

    [System.Serializable]
    public struct Terrain //Struct for sorting colours on map
    {
        public string name;
        public float height;
        public Color colour;
    }

    public void Awake()
    {
        if (mapGen != null)
            Destroy(mapGen);
        else
            mapGen = this;

        Generate_Map();
    }

    public void Generate_Map()
    {
        //Set base type of map
        int type = 1;
        if (Base_NoiseType == Noise_Type.Value)
            type = 2;
        else if (Base_NoiseType == Noise_Type.simplex)
            type = 3;

        //Generate a noise map
        Map_Noise = Noise_Maps.GenNoiseMap(Width, Height, Seed, Scale_Noise, Octaves, Amplitude, Frequency, OffSet,
            Perlin_Noise, value_noise, Simplex_noise, type);


        Map_Noise2 = Noise_Maps.GenNoiseMap(Width, Height, Seed, Scale_Noise, Octaves, Amplitude, Frequency, OffSet,
            Perlin_Noise, value_noise, Simplex_noise, type);
        //colouring the map
        Map_Colour = new Color[Width * Height];


        if (random == true)
        {
            System.Random Ran_Seed = new System.Random();

            int truee = (Ran_Seed.Next(0, 2));

            if (truee == 1)
            {
                erosion = true;
            }
            else
                erosion = false;
        }

        if (erosion == true)
        {
            erode();
        }


        ColourMap();
        //FindBuildings();
        FindBuildings();
        
        //Create references to scripts that generate buildings, map and vegation
        Display_Map Display = FindObjectOfType<Display_Map>();
        
        Vegation veg = FindObjectOfType<Vegation>();



        //Just noise map
        if (DrawMap == Draw_Mode.NoiseMap)
        {
            Display.Drawtextures(Textures.textureHeightMap(Map_Noise));
        }

        //just colour map
        else if (DrawMap == Draw_Mode.ColourMap)
            Display.Drawtextures(Textures.TextureFromMap(Map_Colour, Width, Height));

        //Mesh with possible buildings and vegation
        else if (DrawMap == Draw_Mode.Mesh)
        {
            
            //Display script activates creation of mesh
            Display.DrawMesh(MapMeshGenertion.GenerateMeshTerrain(Map_Noise, MeshHeight, MeshHeightCurve),
                Textures.TextureFromMap(Map_Colour, Width, Height));

            

            if (Animalss == true)
            {
               //animalS.SpawnAnimals(Width, Height, Map_Noise, bulidingMap, MeshHeightCurve, MeshHeight, Seed);

            }


            if (BuildingsPrefabs == true)
            {
               // Buildings_gen.ClearBuildings();
              //  Buildings_gen.GenerateBuildings(Width, Height, Map_Noise, bulidingMap, MeshHeightCurve, MeshHeight);
            }
            else
                //Buildings_gen.ClearBuildings();

            if (Tree_prefabs == true)
            {
                //veg.ClearVegation();
                // veg.GenerateVegation(Width, Height, Map_Noise, bulidingMap, MeshHeightCurve, MeshHeight, Seed);
               // veg.GenerateVegation();
            }
            //else
            // veg.ClearVegation();
        }


        if (screnshot == true)
        {
            Erosion lol = FindObjectOfType<Erosion>();

            //todo fix data bugs

            //Debug.Log(Amplitude);
            var stampString = string.Format("Noises_{0}-{1:00}-{2:00}-", Perlin_Noise, value_noise, Simplex_noise);
            string sString;
            string ErodeeS = "";
            if (erosion == true)
            {
                ErodeeS = "Erosion/";
                sString = string.Format(
                    "{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}-{6:00}-{7:00}-{8:00}-{9:00}-{10:00}-{11:00}-{12:00}",
                    erosion, Rain_iterations, lol.inertia, lol.erosionRadius, lol.sediment_amount_capicty,
                    lol.sediment_amount_capicty_min, lol.disolve_rate, lol.deposit, lol.evaportion_rate, lol.gravity,
                    lol.max_DropletLife, lol.rain_rate, lol.inital_speed, lol.erodeSpeed);
            }
            else
                sString = string.Format("{0}", erosion);

            switch (type)
            {
                case 1:
                    ScreenCapture.CaptureScreenshot("ScreenShots/Perlin/" + ErodeeS + stampString + Amplitude + "-" +
                                                    Frequency + "- Erosion -" + sString + ".png");
                    break;
                case 2:
                    ScreenCapture.CaptureScreenshot("ScreenShots/Value/" + ErodeeS + stampString + Amplitude + "-" +
                                                    Frequency + "- Erosion -" + sString + ".png");
                    break;
                case 3:
                    ScreenCapture.CaptureScreenshot("ScreenShots/Simplex/" + ErodeeS + stampString + Amplitude + "-" +
                                                    Frequency + "- Erosion -" + sString + ".png");
                    break;
                default:
                    Debug.Log("taset");
                    break;
            }
        }
    }


    //make sure editor values are valide
    private void OnValidate()
    {
        //Make sure editor values are not invalid or simulation breaking
        if (Width < 1)
            Width = 1;

        if (Height < 1)
            Height = 1;

        if (Frequency < 1)
            Frequency = 1;

        if (Octaves < 1)
            Octaves = 1;

        if (Perlin_Noise == false && Simplex_noise == false && value_noise == false)
            Perlin_Noise = true;

        if (Octaves > 20)
            Octaves = 20;

        if (Height != Width)
            Height = Width;
    }

    void erode()
    {
        float[] heightmap = new float[Width * Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                heightmap[y * Width + x] = Map_Noise[x, y];
            }
        }

        Erosion lol = FindObjectOfType<Erosion>();

        if (random == true)
        {
            System.Random Ran_Seed = new System.Random();

            lol.inertia = (float) Ran_Seed.Next(0, 100) / 100;
            lol.erosionRadius = (Ran_Seed).Next(3, 15);
            lol.sediment_amount_capicty = (float) (Ran_Seed).Next(1, 140) / 100;
            lol.sediment_amount_capicty_min = (float) (Ran_Seed).Next(0, 30) / 10;
            lol.disolve_rate = (float) (Ran_Seed).Next(0, 100) / 100;
            lol.deposit = (float) (Ran_Seed).Next(0, 30) / 10;
            lol.evaportion_rate = (float) (Ran_Seed).Next(0, 30) / 10;
            lol.gravity = (Ran_Seed).Next(1, 50);
            lol.max_DropletLife = (float) (Ran_Seed).Next(10, 50);
            lol.rain_rate = (Ran_Seed).Next(1, 10);
            lol.inital_speed = (Ran_Seed).Next(1, 10);
            lol.erodeSpeed = (float) (Ran_Seed).Next(1, 100) / 100;
        }


        lol.erosion(Seed, heightmap, Rain_iterations, Width);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Map_Noise[x, y] = heightmap[y * Width + x];
            }
        }
    }


    void ColourMap()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float CurrentHeight = Map_Noise[x, y];

                //loop through each biome to see what this current height falls within
                for (int i = 0; i < Biomes.Length; i++)
                {
                    if (CurrentHeight <= Biomes[i].height) //xD
                    {
                        Map_Colour[y * Width + x] = Biomes[i].colour;
                        //found biome so can move one
                        break;
                    }
                }
            }
        }
    }


    void Update()
    {
       

        if (Input.GetKeyDown("down"))
        {
            
            EntityTracker.Instance.Init(Map_Colour, Biomes[1].colour, Width, Map_Noise2);
            vegation_manger.Instance.init(Map_Colour, Biomes[1].colour, Biomes[5].colour);
            
            
        }
    }

        //changed to flatten the map
        void FindBuildings()
    {
        bulidingMap = new float[Width * Height];

        int l = 0;
        for (int y = 0; y < Height - 1; y++)
        {
            for (int x = 0; x < Width - 1; x++)
            {
                //loop through for building sizes
                float CurrentHeight = Map_Noise[x, y];
               // bulidingMap[x * y] = CurrentHeight;
                //  if (CurrentHeight > Biomes[2].height) //add max height
                // {

                float Check = Map_Noise[x + 1 , y];
                float check2 = Map_Noise[x , y + 1];
                float Check3 = Map_Noise[x + 1 , y + 1];


                float average = Map_Noise[x, y] + Check + check2 + Check3;
                average = average / 4;




                for (float i = 0; i < 10; i++)
                {
                    float finalHeight = 1;
                    finalHeight -= (i / 10);
                    if ( average < 0.21)
                    {
                        finalHeight = 0.15f;
                        average = finalHeight;
                        i = 20;
                        //Debug.Log(average);
                    }

                    else if (average < finalHeight && average > (finalHeight - 0.1))
                    {
                        average = finalHeight;
                        i = 20;
                        // Debug.Log(average);
                    }
                }

                if (l < 10)
                {
                    //Debug.Log(Check);
                   // Debug.Log(average);
                    l++;
                }
                //}

                Map_Noise[x, y] = average;
                Map_Noise[x + 1, y + 1] = average;
                Map_Noise[x + 1, y] = average;
                Map_Noise[x, y + 1] = average;

                /*
                if (Map_Colour[y * Width + x] == Biomes[1].colour)
                {

                    if (average > 0.41)
                    {
                        Map_Colour[y * Width + x] = Biomes[2].colour;
                       // Debug.Log(average);
                    }
                }
                */




                }
        }
        Map_Colour[28 * Width + 93] = Color.red;
        /*
        
        Color[] mapcolours = Map_Colour;
        for (int y = 0; y < Height - Height /2; y++)
        {
            for (int x = 0; x < Width - Width / 2; x++)
            {
                Map_Colour[y * Width + x] = mapcolours[Height - y - 1 * Width + Width - x -1];


                    }

        }*/



    }
        


    public void randomgen()
    {
        System.Random Ran_Seed = new System.Random();

        int xd = Ran_Seed.Next(1, 3);

        if (xd == 1)
            Perlin_Noise = true;
        else
            Perlin_Noise = false;

        xd = Ran_Seed.Next(3, 5);

        if (xd == 3)
            value_noise = true;
        else
            value_noise = false;

        xd = Ran_Seed.Next(4, 6);

        if (xd == 4)
            Simplex_noise = true;
        else
            Simplex_noise = false;


        xd = Ran_Seed.Next(1, 4);
        Rain_iterations = (Ran_Seed).Next(20000, 65000);
        switch (xd)
        {
            case 1:
                Base_NoiseType = Noise_Type.Perlin;

                break;

            case 2:
                Base_NoiseType = Noise_Type.simplex;

                break;

            case 3:
                Base_NoiseType = Noise_Type.Value;
                break;
            default:
                break;
        }

        Amplitude = (float) Ran_Seed.Next(15, 80) / 100;
        Frequency = (float) Ran_Seed.Next(5, 25) / 10;

        Generate_Map();
    }
}