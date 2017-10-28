using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBackground : MonoBehaviour {

    private static int offset = 4;
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
            GetMin(dimentions);
            GetMax(dimentions);
        }
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
                    min.x - offset + sizeOfSprite * i,
                    min.y - offset + sizeOfSprite * j,
                    0);
                Instantiate(prefabs[value], Position, Quaternion.identity);
            }
            n++;
        }
    }

    private static void CalculateNumberOfInstances()
    {
        Vector3 temp = (max - min) / sizeOfSprite + Vector3.one * offset;
        temp = MyMaths.Vector_Ceil(temp);
        for (int i =0;i<3;i++)
        {
            NumberOfInstances[i] = MyMaths.Magnitude(temp[i]);
        }
    }

    public static void GetMin(int dimention)
    {
        float initialPos;
        float finalPos;
        for (int i =0;i<Particle.Instances.Count;i++)
        {
            initialPos = Particle.Instances[i].InitialPosition[dimention];
            finalPos = initialPos + Particle.Instances[i].Displacement[dimention];
            if (initialPos < min[dimention])
            {
                min[dimention] = initialPos;
            }
            if (finalPos < min[dimention])
            {
                min[dimention] = finalPos;
            } 
        }
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
