using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBackground : MonoBehaviour {

    private static int offset = 3;
    private static int sizeOfSprite = 4;

    public static GameObject[] prefabs;

    private static Vector3 min;
    private static Vector3 max;
    private static Vector3 NumberOfInstances;

    public static void CreateBackground()
    {
        Clear();
        prefabs[0] = (Resources.Load("4x4 White") as GameObject);
        prefabs[1] = (Resources.Load("4x4 Black") as GameObject);

        string tag = "Background_prefab";
        DestroyObejctsWithTag(tag);

        for (int dimentions = 0;dimentions<3;dimentions++)
        {
            GetVetex(dimentions);
        }
        min = min - Vector3.one * offset * sizeOfSprite;
        max = max + Vector3.one * offset * sizeOfSprite;
        CalculateNumberOfInstances();
        InstatiatePrefabs();
    }

    private static void InstatiatePrefabs()
    {
        int n = 0;
        for (int i = 0;i<NumberOfInstances.x;i++)
        {
            for (int j =0;j<NumberOfInstances.y;j++)
            {
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

    private static void CalculateNumberOfInstances()
    {
        Vector3 temp = (max - min) / sizeOfSprite;
        temp = MyMaths.Vector_Ceil(temp);
        for (int i =0;i<3;i++)
        {
            NumberOfInstances[i] = MyMaths.Magnitude(temp[i]);
        }
    }

    public static void GetVetex(int dimention)
    {
        float initialPos;
        float finalPos;
        for (int i =0;i<Particle.Instances.Count;i++)
        {
            float newMin =0;
            float newMax =0;

            initialPos = Particle.Instances[i].InitialPosition[dimention];
            finalPos = initialPos + Particle.Instances[i].Displacement[dimention];
            float a = Particle.Instances[i].Acceleration[dimention];
            float u = Particle.Instances[i].InitialVelocity[dimention];
            float t = SimulateController.maxTime;

            if (u >= 0 && a < 0)
            {
                newMax = Case_One_Max(u, 0, a) + initialPos;
                newMin = Case_One_Min(u, a, t) + initialPos;
            }
            else if (u <= 0 && a > 0)
            {
                newMin = Case_One_Max(u, 0, a) + initialPos;
                newMax = Case_One_Min(u, a, t) + initialPos;
            }
            else if (a == 0 && u >= 0)
            {
                newMin = initialPos;
                newMax = u * t + initialPos;
            }
            else if (a== 0 && u < 0)
            {
                newMin = u * t + initialPos;
                newMax = initialPos;
            }
            else
            {
                //im not sure what other combo's exists
                Debug.Log("Additional combo");
                Debug.Log("u :" + u);
                Debug.Log("a :" + a);
                Debug.Log("t : "+t);
            }
            if (newMin < min[dimention])
            {
                min[dimention] = newMax;
            }
            if (newMax > max[dimention])
            {
                max[dimention] = newMax;
            }
        }
    }

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

    private static float Case_One_Max(float u, int v, float a)
    {
        return (v + u) * (v - u) / (2 * a);
    }

    public static void GetMax(int dimention)
    {
        float initialPos;
        float finalPos;
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            initialPos = Particle.Instances[i].InitialPosition[dimention];
            finalPos = initialPos + Particle.Instances[i].Displacement[dimention];
            if (initialPos > max[dimention])
            {
                max[dimention] = initialPos;
            }
            if (finalPos > max[dimention])
            {
                max[dimention] = finalPos;
            }
        }
    }


    public static void Clear()
    {
        prefabs = new GameObject[2];
        //first = new GameObject();
        //second = new GameObject();
        min = Vector3.zero;
        max = Vector3.zero;
        NumberOfInstances = Vector3.zero;
    }

    public static void DestroyObejctsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

}
