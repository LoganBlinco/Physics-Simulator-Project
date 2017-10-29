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

    private static Vector3 min;
    private static Vector3 max;
    private static Vector3 NumberOfInstances;

    public static void CreateBackground()
    {
        ClearVariables();
        DestroyObjects();
        for (int dimentions = 0;dimentions<3;dimentions++)
        {
            //Gets max and min
            GetVetex(dimentions);
        }
        min = min - Vector3.one * offset * sizeOfSprite;
        max = max + Vector3.one * offset * sizeOfSprite;
        CalculateNumberOfInstances();
        InstatiatePrefabs();
    }

    //Sets variable data to 0
    private static void ClearVariables()
    {
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
        float a;
        float u;
        float t;
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            newMin = 0;
            newMax = 0;

            initialPos = Particle.Instances[i].InitialPosition[dimention];
            a = Particle.Instances[i].Acceleration[dimention];
            u = Particle.Instances[i].InitialVelocity[dimention];
            t = SimulateController.maxTime;

            if (u >= 0 && a < 0)
            {
                newMax = Case_One_Max(u, 0, a) + initialPos;
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
                newMax = u * t + initialPos;
            }
            else if (a == 0 && u < 0)
            {
                newMin = u * t + initialPos;
                newMax = initialPos;
            }
            else if (u > 0 && a > 0)
            {
                newMin = initialPos;
                newMax = u * t + 0.5f * a * Mathf.Pow(t,2) +initialPos;
            }
            else if (u < 0 && a < 0)
            {
                newMax = initialPos;
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
            Debug.Log(newMin);
            Debug.Log(newMax);
            if (newMin < min[dimention])
            {
                min[dimention] = newMin;
            }
            if (newMax > max[dimention])
            {
                max[dimention] = newMax;
            }
        }
        Debug.Log(min);
        Debug.Log(max);
    }
    //Calculates the numer of prefabs which must be used in each dimention
    private static void CalculateNumberOfInstances()
    {
        Vector3 temp = (max - min) / sizeOfSprite;
        temp = MyMaths.Vector_Ceil(temp);
        for (int i = 0; i < 3; i++)
        {
            NumberOfInstances[i] = MyMaths.Magnitude(temp[i]);
        }
    }

    //Creates the prefabs
    private static void InstatiatePrefabs()
    {
        int n = 0;
        for (int i = 0;i<NumberOfInstances.x;i++)
        {
            for (int j =0;j<NumberOfInstances.y;j++)
            {
                //equation used to alternate between white and black
                int value = Convert.ToInt32(0.5f * (1 + Mathf.Pow(-1, j - 1 + n)));
                Vector3 Position = new Vector3(
                    min.x  + sizeOfSprite * i,
                    min.y  + sizeOfSprite * j,
                    0);
                GameObject temp = Instantiate(prefabs[value], Position, Quaternion.identity);
                temp.transform.localScale = new Vector3(
                    sizeOfSprite,
                    sizeOfSprite,
                    0);
            }
            n++;
        }
    }

    //Method used when calculating vertex
    private static float Case_One_Min(float u, float a, float t)
    {
        //when t = 0,displacement = 0
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

    //Method used when calculating vertex
    private static float Case_One_Max(float u, int v, float a)
    {
        return (v + u) * (v - u) / (2 * a);
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
