using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SimulateController : MonoBehaviour {

    public static List<GameObject> ParticleInstances;

    public static Slider speedInput;
    static float deltaT;
    public static bool isSimulating;
    public static float simulationTime;
    public static float maxTime;

    private static float _simulationSpeed;
    public static float simulationSpeed
    {
        get { return _simulationSpeed; }
        set { _simulationSpeed = MyMaths.Magnitude(value); }
    }
    public static void OnSimulateClicked()
    {
        Clear();
        GenerateBackground.CreateBackground();
        ParticleInstances = new List<GameObject>();

        DestroyObjects();
        simulationSpeed = speedInput.value;
        for (int i = 0;i<Particle.Instances.Count;i++)
        {
            InstantiateParticle(i);
            if (Particle.Instances[i].Time > maxTime)
            {
                maxTime = Particle.Instances[i].Time;
            }
        }
        isSimulating = true;
        simulationTime = 0;
    }
    private static void DestroyObjects()
    {
        string tag = "Player";
        GenerateBackground.DestroyObejctsWithTag(tag);
    }


    private static void InstantiateParticle(int index)
    {
        Vector3 Position = Particle.Instances[index].InitialPosition;
        Vector3 Velocity = Particle.Instances[index].InitialVelocity;
        //Creating a unity object 
        ParticleInstances.Add(Instantiate(Resources.Load("Sphere"), Position, Quaternion.identity) as GameObject);
        float Radius = Particle.Instances[index].Radius;
        ParticleInstances[index].transform.localScale = Vector3.one * Radius;
    }

    public static void Clear()
    {
        ParticleInstances = new List<GameObject>();
        isSimulating = false;
        simulationTime = 0;
        maxTime = 0;
        simulationSpeed = 0;
    }

    public void Update()
    {
        if (isSimulating)
        {
            simulationSpeed = speedInput.value;
            deltaT = Time.fixedDeltaTime * simulationSpeed;
            MoveParticles();
            UpdateVelocity();
            //UpdateGravity();
            //UpdateCollisions();
        }
        CheckTime();
        simulationTime += deltaT;
        simulationTime = MyMaths.Clamp(simulationTime, 0, maxTime);
    }

    private void CheckTime()
    {
        if (simulationTime >= maxTime)
        {
            isSimulating = false;
        }
    }

    private void MoveParticles()
    {
        for (int i =0;i<Particle.Instances.Count;i++)
        {
            MoveParticles_Controller(i);
        }
    }
    private void UpdateVelocity()
    {
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            UpdateVelocity_Controller(i);
        }
    }

    private void UpdateVelocity_Controller(int index)
    {
        Particle.Instances[index].Key[0] = "01011";
        Particle.Instances[index].Key[1] = "01011";
        Particle.Instances[index].Key[2] = "01011";
        Particle.Instances[index].Time = simulationTime;
        Particle.Instances[index] = SuvatSolvers.FindEquation(Particle.Instances[index]);
    }

    private void MoveParticles_Controller(int index)
    {
        //Done to find the new diosplacement
        Particle.Instances[index].Key[0] = "01111";
        Particle.Instances[index].Key[1] = "01111";
        Particle.Instances[index].Key[2] = "01111";
        Particle.Instances[index].Time = simulationTime;

        Particle.Instances[index] = SuvatSolvers.FindEquation(Particle.Instances[index]);
        Vector3 position = Particle.Instances[index].Displacement + Particle.Instances[index].InitialPosition;
        ParticleInstances[index].transform.position = position;
    }
}
