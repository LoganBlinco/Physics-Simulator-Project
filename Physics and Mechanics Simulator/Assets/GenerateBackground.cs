using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBackground : MonoBehaviour {

    //Reference to the prefabs in resouces folder.
    public static GameObject[] prefabs = new GameObject[] { (Resources.Load("4x4 White") as GameObject), (Resources.Load("4x4 Black") as GameObject) };

    //Additional prefabs which will be generated on the left and up of background
    private static int offset = 3;
    //Lenght of each size in pixels such as 4x4
    private static int sizeOfSprite = 4;


    //Vector storing minimum values in simulation for each dimentions
    private static Vector3 min;
    //Vector storing maximum values in simulation for each dimentions
    private static Vector3 max;
    //Number of instances in each dimention of the prefab which must be created.
    private static Vector3 NumberOfInstances;

    //Creates the background
    public static void CreateBackground()
    {
        //Clears variable values to be reused.
        ClearVariables();
        //Destroy active Background_prefab objects.
        DestroyObjects();
        for (int dimentions = 0;dimentions<3;dimentions++)
        {
            //Gets max and min values
            GetVetex(dimentions);
        }
        //Adds the offset 
        min = min - Vector3.one * offset * sizeOfSprite;
        max = max + Vector3.one * offset * sizeOfSprite;
        CalculateNumberOfInstances();
        //Creates the objects to the scene
        InstatiatePrefabs();
    }

    //Sets variable data to 0
    private static void ClearVariables()
    {
        //Vector3.zero creates an Vector3 with all 0 elements
        min = Vector3.zero;
        max = Vector3.zero;
        NumberOfInstances = Vector3.zero;
    }
    //Destroygs pre-existing background prefabs
    private static void DestroyObjects()
    {
        string tag = "Background_prefab";
        DestroyObejctsWithTag(tag);
    }
    //Gets max and min values of particles for simulation
    private static void GetVetex(int dimention)
    {
        float initialPos;
        float newMin;
        float newMax;
        //acceleration
        float a;
        //initial velocity
        float u;
        //max time
        float t;
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            newMin = 0;
            newMax = 0;

            initialPos = Particle.Instances[i].InitialPosition[dimention];
            a = Particle.Instances[i].Acceleration[dimention];
            u = Particle.Instances[i].InitialVelocity[dimention];
            t = SimulateController.maxTime;

            /* 
             * Calculating the maximum and minimum values of the particle during simulation
            */

            if (u >= 0 && a < 0)
            {
                //s = u^2 / 2a + r
                newMax = -Mathf.Pow(u, 2) / (2 * a) + initialPos;
                newMin = Case_One_Min(u, a, t) + initialPos;
            }
            else if (u <= 0 && a > 0)
            {
                //S = -u^2 / 2a (v^2 = 0)
                newMin = -Mathf.Pow(u,2)/(2 * a) + initialPos;
                newMax = u * t + 0.5f * a * Mathf.Pow(t, 2) + initialPos;
            }
            else if (a == 0 && u >= 0)
            {
                newMin = initialPos;
                //s = ut + r
                newMax = u * t + initialPos;
            }
            else if (a == 0 && u < 0)
            {
                // s = ut + r
                newMin = u * t + initialPos;
                newMax = initialPos;
            }
            else if (u > 0 && a > 0)
            {
                newMin = initialPos;
                // s = ut + 1/2 at^2 + r
                newMax = u * t + 0.5f * a * Mathf.Pow(t,2) +initialPos;
            }
            else if (u < 0 && a < 0)
            {
                newMax = initialPos;
                //s = ut + 1 / 2 at ^ 2 + r
                newMin = u * t + 0.5f * a * Mathf.Pow(t, 2) + initialPos;
            }
            else
            {
                //im not sure what other combo's exists so this covers any for the future
                Debug.Log("Additional combo");
                Debug.Log("u :" + u);
                Debug.Log("a :" + a);
                Debug.Log("t : " + t);
            }
            //Checking if newMin or newMax is < or > than the current min and max
            if (newMin < min[dimention])
            {
                min[dimention] = newMin;
            }
            if (newMax > max[dimention])
            {
                max[dimention] = newMax;
            }
        }
    }
    //Calculates the numer of prefabs which must be used in each dimention
    private static void CalculateNumberOfInstances()
    {
        Vector3 temp = (max - min) / sizeOfSprite;
        //Rounds all values inside vector up
        temp = MyMaths.Vector_Ceil(temp);
        for (int i = 0; i < 3; i++)
        {
            NumberOfInstances[i] = MyMaths.Magnitude(temp[i]);
        }
    }

    //Creates the prefabs
    private static void InstatiatePrefabs()
    {
        for (int i = 0;i<NumberOfInstances.x;i++)
        {
            for (int j =0;j<NumberOfInstances.y;j++)
            {
                //equation used to alternate between white and black squares stored in elements 0 and 1
                int value = Convert.ToInt32(0.5f * (1 + Mathf.Pow(-1, j - 1 + i)));
                Vector3 Position = new Vector3(
                    min.x  + sizeOfSprite * i,
                    min.y  + sizeOfSprite * j,
                    0);
                //Creates an object at Position with no roation
                GameObject temp = Instantiate(prefabs[value], Position, Quaternion.identity);
                //Scales opject up by sizeOfSprite
                temp.transform.localScale = new Vector3(
                    sizeOfSprite,
                    sizeOfSprite,
                    0);
            }
        }
    }

    //Method used when calculating vertex
    private static float Case_One_Min(float u, float a, float t)
    {
        //when t = 0,displacement = 0
        //s = ut + 1/2 at^2
        float maxTime = u * t + 0.5f * a * Mathf.Pow(t, 2);
        if (maxTime < 0)
        {
            return maxTime;
        }
        else
        {
            return 0;
        }
    }

    //Destroys anyobject with the input tag parameter in the scene
    public static void DestroyObejctsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

}
