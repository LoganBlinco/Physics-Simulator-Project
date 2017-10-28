using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SimulateController : MonoBehaviour {

    public static List<GameObject> ParticleInstances;

    public static Slider speedInput;
    public static Text LabelTime;
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
        GetMaxTime();
        GenerateBackground.CreateBackground();
        ParticleInstances = new List<GameObject>();
        DestroyObjects();
        simulationSpeed = speedInput.value;
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            InstantiateParticle(i);
        }
        isSimulating = true;
        simulationTime = 0;
    }
    //Resets variables
    private static void Clear()
    {
        ParticleInstances = new List<GameObject>();
        isSimulating = false;
        simulationTime = 0;
        maxTime = 0;
        simulationSpeed = 0;
    }
    //Gets the maximum time which any of the simulations have
    private static void GetMaxTime()
    {
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            if (Particle.Instances[i].Time > maxTime)
            {
                maxTime = Particle.Instances[i].Time;
            }
        }
    }
    //Destroys objects with the Player Tag.Objects with the player tag are prefabs created for simulation previously.
    private static void DestroyObjects()
    {
        string tag = "Player";
        GenerateBackground.DestroyObejctsWithTag(tag);
    }
    //Instantiates the prefab at the position required.
    private static void InstantiateParticle(int index)
    {
        Vector3 Position = Particle.Instances[index].InitialPosition;
        //Creating a unity object 
        ParticleInstances.Add(Instantiate(Resources.Load("Sphere"), Position, Quaternion.identity) as GameObject);
        float Radius = Particle.Instances[index].Radius;
        ParticleInstances[index].transform.localScale = Vector3.one * Radius;
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
        UpdateTimeLabel();
        CheckTime();
        simulationTime += deltaT;
        simulationTime = MyMaths.Clamp(simulationTime, 0, maxTime);
    }
    //Sets the time label to the currenty simulation time
    private void UpdateTimeLabel()
    {
        string value2DP = simulationTime.ToString("n2");
        LabelTime.text = "Time :" + value2DP + "s";
    }
    //Checks if simulation time is greater than maxTime to end simulation
    private void CheckTime()
    {
        if (simulationTime >= maxTime)
        {
            isSimulating = false;
        }
    }

    //Moves every particle with an instance.
    private void MoveParticles()
    {
        for (int i =0;i<Particle.Instances.Count;i++)
        {
            MoveParticles_Controller(i);
        }
    }
    //Perfomrs the movement calculations using the change in time.
    private void MoveParticles_Controller(int index)
    {
        //Allows for Suvat equation to be used to determine displacement.
        Particle.Instances[index].Key[0] = "01111";
        Particle.Instances[index].Key[1] = "01111";
        Particle.Instances[index].Key[2] = "01111";
        //Changing time according to simulation
        Particle.Instances[index].Time = simulationTime;
        //Getting correct equation
        Particle.Instances[index] = SuvatSolvers.FindEquation(Particle.Instances[index]);
        Vector3 position = Particle.Instances[index].Displacement + Particle.Instances[index].InitialPosition;
        ParticleInstances[index].transform.position = position;
    }

    //Updates velocity of each particle by the time.deltatime
    private void UpdateVelocity()
    {
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            UpdateVelocity_Controller(i);
        }
    }

    private void UpdateVelocity_Controller(int index)
    {
        //Blanked out displacement so I can use v = u + at meaning the si
        Particle.Instances[index].Key[0] = "11011";
        Particle.Instances[index].Key[1] = "11011";
        Particle.Instances[index].Key[2] = "11011";
        Particle.Instances[index].Time = simulationTime;
        Particle.Instances[index] = SuvatSolvers.FindEquation(Particle.Instances[index]);
    }
}
