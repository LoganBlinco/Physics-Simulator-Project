using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SimulateController : MonoBehaviour {


    //Stores all gameobjects which have been created for simulation
    public static List<GameObject> ParticleInstances;

    //simulation speed slider reference from UI 
    public static Slider speedInput;
    //refernece to the current simulation time Label in UI
    public static Text LabelTime;

    //change of time between frames
    static float deltaT;

    //Boolean state depending if the program is simulating or not
    public static bool isSimulating;
    //Simulation time which has occured in simulation
    public static float simulationTime;
    //maximum time for simulation
    public static float maxTime;

    //Speed for simulation to be ran at
    private static float _simulationSpeed;
    public static float simulationSpeed
    {
        get { return _simulationSpeed; }
        set { _simulationSpeed = MyMaths.Magnitude(value); }
    }

    //Ran to begin a simulation
    public static void OnSimulateClicked()
    {
        //Resetting varibales
        Clear();
        //gets max simulation time
        GetMaxTime();
        //Creates background of simulation
        GenerateBackground.CreateBackground();
        //resets instance of list
        ParticleInstances = new List<GameObject>();
        //Destroys the objects which have been created by the simulator
        DestroyObjects();
        simulationSpeed = speedInput.value;
        //Creating every particle which has a corresponding calculated value.
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            InstantiateParticle(i);
        }
        //Begins simulation
        isSimulating = true;
        //Resets the simulation time
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
    //Gets the maximum time from all particles to be simulated
    private static void GetMaxTime()
    {
        for (int i = 0; i < Particle.Instances.Count; i++)
        {
            //Checks if the particle time is greater than the maxTime
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
        //Destroys all objects in the scence with the tag given as argument
        GenerateBackground.DestroyObejctsWithTag(tag);
    }
    //Instantiates the prefab at the position required.
    private static void InstantiateParticle(int index)
    {
        Vector3 Position = Particle.Instances[index].InitialPosition;
        //Creating a unity object at position , with no rotation
        ParticleInstances.Add(Instantiate(Resources.Load("Sphere"), Position, Quaternion.identity) as GameObject);
        float Radius = Particle.Instances[index].Radius;
        //Scales up the object to have radius
        ParticleInstances[index].transform.localScale = Vector3.one * Radius;
    }

    //Ran when every frame is first loaded
    public void Update()
    {
        if (isSimulating)
        {
            //updating the speed for the simulation determined by the user
            simulationSpeed = speedInput.value;
            //time taken between frames multiplied by simulation speed
            deltaT = Time.fixedDeltaTime * simulationSpeed;
            //Controls movements of particle
            MoveParticles();
            //Updates veloicty by particle acceleration.
            UpdateVelocity();
            //updates the time label on the UI using current simulation time
            UpdateTimeLabel();

            //updating simulationTime by change
            simulationTime += deltaT;
        }
        //Checks if time is more than maxTime
        CheckTime();
        //Clamps value betwene 0 and the maxTime
        simulationTime = MyMaths.Clamp(simulationTime, 0, maxTime);
    }
    //Sets the time label to the currenty simulation time
    private void UpdateTimeLabel()
    {
        //rounds to 2 decimal places
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
            //Performs movement for the particle with index i
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
        //Calculating the position to move to
        Vector3 position = Particle.Instances[index].Displacement + Particle.Instances[index].InitialPosition;
        //Moves to position
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

    //Updates the velocity for a particle index
    private void UpdateVelocity_Controller(int index)
    {
        //Resets key so Suvat equations can be used
        Particle.Instances[index].Key[0] = "11011";
        Particle.Instances[index].Key[1] = "11011";
        Particle.Instances[index].Key[2] = "11011";
        Particle.Instances[index].Time = simulationTime;
        //solves suvat equations
        Particle.Instances[index] = SuvatSolvers.FindEquation(Particle.Instances[index]);
    }
}
