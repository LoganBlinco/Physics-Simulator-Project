using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GravitySimulationController : MonoBehaviour {

    #region Equation constants

    //Newtons universal gravitational constant
    static float NewtonG = 6.7f * Mathf.Pow(10, -11);

    //6 units is equal to the distance earth to moon
    static float newDistanceMod = 384 * Mathf.Pow(10, 6) / 6;
    //1 mass is equal to the earth mass
    static float newMassMod = 6.0f * Mathf.Pow(10, 24);
    //Module wide speed up
    static float timeMod = 100;
    //New force multiplier (the new G)
    public static float G = timeMod * NewtonG * newMassMod / Mathf.Pow(newDistanceMod, 2);

    #endregion

    #region Graph variables
    public GameObject GraphSpeed;
    public GameObject GraphAcceleration;

    //Time for each graph update
    //Lower value = more processing
    private float timePerUpdate = 1f;
    //Time remaining until next graph update
    private float timeTillUpdate = 0.0f;


    #endregion 

    #region Variables
    //Stores the time in which the simulation has been occuring for
    public float simulationTime = 0;
    //Displays to the user the time the simulation has occured for
    public Text Label_Time;

    //Speed multiplier determined by slider
    //Default value is 1
    public static float SimulationSpeed = 1;
    //Boolean state if simulation should be occuring or not
    public static bool isSimulating = false;
    //Change of time between frames
    private float deltaT;
    #endregion

    #region Update Methods

    void Update()
    {
        if (isSimulating == true)
        {
            //Controls updating the graph
            UpdateParticleGraphValues();
            //Time between frames multiplied by speed factor
            deltaT = Time.deltaTime * SimulationSpeed;
            UpdateTimeLabel();

            UpdateVelocity();
            UpdatePosition();

            Gravity_InputController.Instance.UpdateUI(newParticle.ParticleInstances[Gravity_InputController.Instance.ParticleIndexSelected]);
            simulationTime += deltaT;
        }
    }
    //Updates graph values for current particle selected
    private void UpdateParticleGraphValues()
    {
        //Only updates every X seconds
        if (timeTillUpdate <= 0)
        {
            //Updates the points stored for every planet created
            UpdateGraphValues();
            //Index of particle user has selected for graphing
            int index = Gravity_InputController.Instance.DropBoxGraphTarget.value;
            //Updating graphs
            Gravity_InputController.Instance.GraphAcceleration.GetComponent<GraphMaker>().CreateGraph(GravityPlanets.PlanetInstances[index].graphPointsAcceleration);
            Gravity_InputController.Instance.GraphSpeed.GetComponent<GraphMaker>().CreateGraph(GravityPlanets.PlanetInstances[index].graphPointsSpeed);

            timeTillUpdate = timePerUpdate;
        }
        else
        {
            timeTillUpdate -= Time.deltaTime;
        }
    }
    //Updates the values of points to plot for every planet
    private void UpdateGraphValues()
    {
        foreach(GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            planet.graphPointsSpeed.Add(new Vector2(simulationTime,planet.currentVelocity.magnitude));
            planet.graphPointsAcceleration.Add(new Vector2(simulationTime,GetAccelleration(planet).magnitude));
        }
    }

    private void UpdatePosition()
    {
        foreach (GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            planet.MyGameObject.transform.position = planet.MyGameObject.transform.position + planet.currentVelocity * deltaT;
        }
    }

    private void UpdateVelocity()
    {
        foreach(GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            Vector3 acceleration = GetAccelleration(planet);
            planet.currentVelocity = planet.currentVelocity + acceleration * deltaT;
        }
    }
    //Returns the acceleration due to gravity of a planet
    //Aceeleration from newtons law of gravitation
    //Then F = ma
    private Vector3 GetAccelleration(GravityPlanets attractor)
    {
        Vector3 sum = Vector3.zero;
        Vector3 positionDelta = Vector3.zero;
        foreach (GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            positionDelta = planet.MyGameObject.transform.position - attractor.MyGameObject.transform.position;
            //Cannot divide by 0
            if (positionDelta != Vector3.zero)
            {
                sum += planet.mass * positionDelta / Mathf.Pow(MyMaths.Vector_Magnitude(positionDelta),3);
            }
        }
        return G * sum;
    }

    //Updates time label to match with current simulation time
    private void UpdateTimeLabel()
    {
        //Rounding to 2 decimal places
        string value2DP = simulationTime.ToString("n2");
        Label_Time.text = "Time : " + value2DP + " s";
    }



    #endregion

}
