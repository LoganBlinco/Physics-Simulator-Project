using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SimulateController : MonoBehaviour {

    #region Varaibles
    //Stores all gameobjects which have been created for simulation
    public static List<GameObject> ParticleInstances;

    public GameObject velocityGraph;
    public GameObject displacementGraph;

    //simulation speed slider reference from UI 
    public static Slider speedInput;
    //refernece to the current simulation time Label in UI
    public static Text LabelTime;

    //change of time between frames
    static float deltaT;

    //Time between graph updates
    static float timePerGraphUpdate = 0.5f;
    //Time since last graph update
    static float timeTillGraphUpdate = 0.0f;



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
    #endregion

    #region Starting Methods

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

    #endregion

    #region Update Methods

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

            if (timeTillGraphUpdate <= 0)
            {
                //Updates all the particle values which are used for graphing
                UpdateParticleGraphingValues();
                //Updates points which are plotted on graph
                UpdateParticlePoints();

                timeTillGraphUpdate = timePerGraphUpdate;
            }
            else
            {
                timeTillGraphUpdate -= Time.deltaTime;
            }

            //updating simulationTime by change
            simulationTime += deltaT;

        }
        //Checks if time is more than maxTime
        CheckTime();
        //Clamps value betwene 0 and the maxTime
        simulationTime = MyMaths.Clamp(simulationTime, 0, maxTime);
    }

    #region Graphing updates
    //Plots graph
    private void UpdateParticlePoints()
    {
        int indexSelected = Suvat_UiController.instance.GraphDropBoxParticles.value;
        int dimention = Suvat_UiController.instance.GraphDropBoxDimention.value;
        //stores the velocity's of the particle in correct format
        List<Vector2> velocityList = new List<Vector2>();
        //stores the displacements of the particle
        List<Vector2> displacementList = new List<Vector2>();

        Particle tempParticle = Particle.Instances[indexSelected];
        for (int i = 0; i < tempParticle.ParticleValues.Count; i++)
        {
            velocityList.Add(new Vector2(
                tempParticle.ParticleValues[i].time,
                tempParticle.ParticleValues[i].velocity[dimention]));

            displacementList.Add(new Vector2(
                tempParticle.ParticleValues[i].time,
                tempParticle.ParticleValues[i].displacement[dimention]));
        }
        //Updates the graph
        velocityGraph.GetComponent<GraphMaker>()._tag = "VelocityGraphElemet";
        velocityGraph.GetComponent<GraphMaker>().CreateGraph(velocityList);
        displacementGraph.GetComponent<GraphMaker>()._tag = "DisplacementGraphElement";
        displacementGraph.GetComponent<GraphMaker>().CreateGraph(displacementList);
    }

    //Updates each particle being simulate's graphing values
    private void UpdateParticleGraphingValues()
    {
        List<Particle> ParticleList = Particle.Instances;
        for (int i =0;i< ParticleList.Count;i++)
        {
            ParticleProperties temp = new ParticleProperties
            {
                time = simulationTime,
                velocity = ParticleList[i].FinalVelocity,
                displacement = ParticleList[i].Displacement
            };
            Particle.Instances[i].ParticleValues.Add(temp);
        }

    }
    #endregion

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

    #endregion

    #region Moving Particles
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
    #endregion

    #region Updating Velocity
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
    #endregion
}
